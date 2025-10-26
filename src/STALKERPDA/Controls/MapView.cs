using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using STALKERPDA.Utils;
using OpenNETCF.Drawing;
using OpenNETCF.Drawing.Imaging;
using STALKERPDA.Utils;

namespace STALKERPDA.Controls
{
    public partial class MapView : DoubleBufferedControl
    {
        public const int MIN_ZOOM = 11, MAX_ZOOM = 17, DEFAULT_ZOOM = 14;
        public const int TILE_SIDE = 256, SCROLL_SPEED = 25;

        private double x, y, lat, lon;
        private int zoom = DEFAULT_ZOOM;

        private int todrawx = 1, todrawy = 1;

        //private int offsetx, offsety;
        
        protected Bitmap mapBuffer;
        protected Graphics mapGraphics;
        protected GraphicsEx mapGraphicsEx;

        protected MapTileProvider TileProvider = new MapTileProvider();
        protected GPSProvider GPS = new GPSProvider();

        private GPSProvider.LatLon DefaultPos = new GPSProvider.LatLon(50.50150086776309, 30.4982178814705);

        protected IImage PlayerIcon;
        protected ImageInfo PlayerIconInfo;

        public void SetCenterLatLon(double _lat, double _lon)
        {
            lat = _lat; lon = _lon;
            CalculateXY();
        }

        public void SetZoom(int z)
        {
            zoom = Math.Max(MIN_ZOOM, Math.Min(MAX_ZOOM, z));
            CalculateXY();
        }

        public GPSProvider.LatLon GetPlayerPos()
        {
            return GPS.IsGpsAvailable() ? GPS.GetLatLon() : DefaultPos;
        }

        public STALKERPDA.Utils.GPSProvider.GpsState GetGpsState()
        {
            return GPS.GetGpsState();
        }

        private void CalculateXY()
        {
            x = (lon + 180) / 360 * Math.Pow(2, zoom);
            y = (1 - Math.Log(Math.Tan(lat * Math.PI / 180) + 1 / Math.Cos(lat * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, zoom);
            TileProvider.UpdateCacheList((int)x, (int)y, zoom);
            Invalidate();
        }

        delegate void VoidDelegate();

        public MapView()
        {
            m_factory = new ImagingFactoryClass();
            InitializeComponent();
            mapBuffer = new Bitmap(1, 1);

            VoidDelegate del = delegate() { this.Invalidate(); };

            TileProvider.OnWaitedTileArrived += (a, b) => { this.Invoke(del); };
            GPS.OnPosUpdated += (a, b) => { this.Invoke(del); };

            PlayerIcon = LoadImageFromResource("STALKERPDA.Images.Ui.MapIcons.PlayerIcon.png");
            PlayerIcon.GetImageInfo(out PlayerIconInfo);

            SetZoom(18);
            var latlon = GetPlayerPos();
            SetCenterLatLon(latlon.Lat, latlon.Lon);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (mapGraphicsEx != null)
                mapGraphicsEx.Dispose();
            if (mapGraphics != null)
                mapGraphics.Dispose();
            if (mapBuffer != null)
                mapBuffer.Dispose();

            mapBuffer = new Bitmap(ClientRectangle.Width - 2, ClientRectangle.Height - 2);
            mapGraphics = Graphics.FromImage(mapBuffer);
            mapGraphicsEx = GraphicsEx.FromGraphics(mapGraphics);

            todrawx = (int)Math.Ceiling((float)mapBuffer.Width / (float)TILE_SIDE) + 1;
            todrawy = (int)Math.Ceiling((float)mapBuffer.Height / (float)TILE_SIDE) + 1;
        }

        protected override void SetupBackground()
        {
            m_gBuffer.DrawRectangle(new Pen(Color.FromArgb(93, 95, 95), 1), 0, 0, this.Width - 1, this.Height - 1);//TODO: Move to background
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(93, 95, 95), 1), 0, 0, this.Width - 1, this.Height - 1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            using (var g = e.Graphics)
            {
                int x0 = (int)Math.Floor(x), y0 = (int)Math.Floor(y);
                int dx = (int)(TILE_SIDE * (x - x0)), dy = (int)(TILE_SIDE * (y - y0));

                int leftspace = (this.Width / 2) - dx;
                int todrawleft = (int)Math.Ceiling((float)leftspace / (float)TILE_SIDE);

                int topspace = (this.Height / 2) - dy;
                int todrawtop = (int)Math.Ceiling((float)topspace / (float)TILE_SIDE);

                for (int i = 0; i < todrawx; i++ )
                {
                    int xoff = i - todrawleft;

                    for (int k = 0; k < todrawy; k++)
                    {
                        int yoff = k - todrawtop;
                        var tile = GetTile(x0 + xoff, y0 + yoff, zoom);
                        mapGraphicsEx.DrawImage(tile, (this.Width / 2) - dx + (TILE_SIDE * xoff), (this.Height / 2) - dy + (TILE_SIDE * yoff));
                        //tile.Dispose();
                    }
                }

                //g.DrawImage(mapBuffer, 1,1);
                var hdc = g.GetHdc();
                var gex = GraphicsEx.FromHdc(hdc);
                gex.CopyGraphics(mapGraphicsEx, new Rectangle(1,1,this.Width-2, this.Height-2));

                var latlon = GetPlayerPos();
                DrawIcon(latlon.Lat, latlon.Lon, PlayerIcon, hdc);

                //g.ReleaseHdc(hdc);
                gex.Dispose();
                //g.FillRectangle(new SolidBrush(Color.Red), Width / 2, Height / 2, 1, 1);
            }
        }

        private void DrawIcon(double lat, double lon, IImage icon, IntPtr hdc)
        {
            var xy = LatLonToScreenCoord(lat, lon);
            ImageInfo imginfo;
            icon.GetImageInfo(out imginfo);

            int xs = (int)(xy.X - imginfo.Width / 2);
            int ys = (int)(xy.Y - imginfo.Height/2);

            icon.Draw(hdc, new RECT(xs, ys, (int)PlayerIconInfo.Width + xs, (int)PlayerIconInfo.Height + ys), null);
        }

        private Vector2 LatLonToScreenCoord(double lat, double lon)
        {
            int _x = (int)(((lon + 180) / 360 * Math.Pow(2, zoom) - x) * TILE_SIDE + Width / 2 );
            int _y = (int)(((1 - Math.Log(Math.Tan(lat * Math.PI / 180) + 1 / Math.Cos(lat * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, zoom) - y) * TILE_SIDE + Height / 2 );

            return new Vector2(_x, _y);
        }

        private BitmapEx GetTile(int x, int y, int z)
        {
            return TileProvider.GetBitmap(x, y, z);
        }

        private void customButton5_Click(object sender, EventArgs e)// Center to character
        {
            SetZoom(18);
            var latlon = GetPlayerPos();
            SetCenterLatLon(latlon.Lat, latlon.Lon);
        }

        private void customButton3_Click(object sender, EventArgs e)// Increase zoom
        {
            SetZoom(zoom + 1);
        }

        private void customButton9_Click(object sender, EventArgs e)// Reset zoom
        {
            SetZoom(DEFAULT_ZOOM);
        }

        private void customButton7_Click(object sender, EventArgs e)// Decrease zoom
        {
            SetZoom(zoom - 1);
        }

        private void customButton2_OnTick(object sender, EventArgs e)
        {
            Offset(0, 1);
        }

        private void customButton6_OnTick(object sender, EventArgs e)
        {
            Offset(1, 0);
        }

        private void customButton8_OnTick(object sender, EventArgs e)
        {
            Offset(0, -1);
        }

        private void customButton4_OnTick(object sender, EventArgs e)
        {
            Offset(-1, 0);
        }

        public void ProcessKeyCode(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    Offset(-1, 0);
                    break;

                case Keys.Down:
                    Offset(1, 0);
                    break;

                case Keys.Left:
                    Offset(0, -1);
                    break;

                case Keys.Right:
                    Offset(0, 1);
                    break;

                case Keys.Enter:
                    SetZoom(18);
                    var latlon = GetPlayerPos();
                    SetCenterLatLon(latlon.Lat, latlon.Lon);
                    break;

                case Keys.F1:
                    SetZoom(zoom - 1);
                    break;

                case Keys.F2:
                    SetZoom(zoom + 1);
                    break;
            }
        }

        private void Offset(int _x, int _y)
        {
            //offsetx += _x * SCROLL_SPEED;
            //offsety += _y * SCROLL_SPEED;
            SetCenterLatLon(lat + _y / Math.Pow(2, zoom) * SCROLL_SPEED, lon + _x / Math.Pow(2, zoom) * SCROLL_SPEED);
            //Invalidate();
        }
    }
}
