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

namespace STALKERPDA.Controls
{
    public partial class MapView : DoubleBufferedControl
    {
        private const int MIN_ZOOM = 10, MAX_ZOOM = 18, DEFAULT_ZOOM = 14;
        private const int TILE_SIDE = 256, SCROLL_SPEED = 10;

        private double x, y, lat, lon;
        private int zoom = DEFAULT_ZOOM;

        private int todrawx = 1, todrawy = 1;

        private int offsetx, offsety;
        
        protected Bitmap mapBuffer;
        protected Graphics mapGraphics;
        protected GraphicsEx mapGraphicsEx;

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

        private void CalculateXY()
        {
            x = (lon + 180) / 360 * Math.Pow(2, zoom);
            y = (1 - Math.Log(Math.Tan(lat * Math.PI / 180) + 1 / Math.Cos(lat * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, zoom);
            Invalidate();
        }


        public MapView()
        {
            InitializeComponent();
            mapBuffer = new Bitmap(1, 1);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

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
                        mapGraphics.DrawImage(tile, (this.Width / 2) - dx + (TILE_SIDE * xoff) + offsetx, (this.Height / 2) - dy + (TILE_SIDE * yoff) + offsety);
                        tile.Dispose();
                    }
                }

                //g.DrawImage(mapBuffer, 1,1);

                var gex = GraphicsEx.FromGraphics(g);
                gex.CopyGraphics(mapGraphicsEx, new Rectangle(1,1,this.Width-2, this.Height-2));
                //gex.Dispose();

                g.FillRectangle(new SolidBrush(Color.Red), Width / 2, Height / 2, 1, 1);
            }
        }

        private Bitmap GetTile(int x, int y, int z)
        {
            var i = new Bitmap(TILE_SIDE, TILE_SIDE);

            var color = (((x + y) % 2) > 0) ? Color.FromArgb(0, 0, 255) : Color.FromArgb(0, 255, 0);
            var font = new Font(FontFamily.GenericSerif, 6, FontStyle.Bold);
            string text = x + " " + y + " " + z;

            using (var g = Graphics.FromImage(i))
            {
                var mes = g.MeasureString(text, font);

                g.FillRectangle(new SolidBrush(color), 0, 0, TILE_SIDE, TILE_SIDE);

                g.DrawString(text, font, new SolidBrush(Color.Black), (TILE_SIDE - mes.Width) / 2, (TILE_SIDE - mes.Height) / 2);
            }

            return i;
            //return new Bitmap(Path.GetDirectoryName (Assembly.GetExecutingAssembly ().GetName ().CodeBase)+"/vt1.jpg");
        }

        private void customButton5_Click(object sender, EventArgs e)// Center to character
        {
            SetZoom(18);
            SetCenterLatLon(50.43339731489526, 30.521802666744918);
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
            Offset(0, -1);
        }

        private void customButton6_OnTick(object sender, EventArgs e)
        {
            Offset(-1, 0);
        }

        private void customButton8_OnTick(object sender, EventArgs e)
        {
            Offset(0, 1);
        }

        private void customButton4_OnTick(object sender, EventArgs e)
        {
            Offset(1, 0);
        }

        private void Offset(int _x, int _y)
        {
            offsetx += _x * SCROLL_SPEED;
            offsety += _y * SCROLL_SPEED;
            Invalidate();
        }
    }
}
