using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Drawing.Imaging;

namespace STALKERPDA.Controls
{
    public partial class TabSelector : TransparentControl
    {
        public List<string> TabNames = new List<string>();

        public List<string> TabIcons
        {
            set
            {
                foreach (var img in value)
                {
                    _images.Add(LoadImageFromResource(img));
                }
                Invalidate();
            }
        }

        private List<IImage> _images = new List<IImage>();

        [EditorBrowsable()]
        public int MidWidthOpen {get; set;}

        [EditorBrowsable()]
        public int ItemOffset { get; set; }

        [EditorBrowsable()]
        public int MidWidthClosed { get; set; }

        [EditorBrowsable()]
        [DefaultValue(0)]
        public int SelectedIndex { get; set; }

        public event EventHandler OnTabChanged;

        private IImage ImageLeft, ImageCenter, ImageRight, ImageLeft_Selected, ImageCenter_Selected, ImageRight_Selected;

        private List<int> StartCoords = new List<int>(), EndCoords = new List<int>();

        private ImageInfo ImageLeftInfo, ImageCenterInfo, ImageRightInfo, ImageLeftInfo_Selected, ImageCenterInfo_Selected, ImageRightInfo_Selected;

        public TabSelector()
        {
            InitializeComponent();

            ImageLeft = LoadImageFromResource("STALKERPDA.Images.Ui.Buttons.Button1.Left.png");
            ImageCenter = LoadImageFromResource("STALKERPDA.Images.Ui.Buttons.Button1.Center.png");
            ImageRight = LoadImageFromResource("STALKERPDA.Images.Ui.Buttons.Button1.Right.png");

            ImageLeft_Selected = LoadImageFromResource("STALKERPDA.Images.Ui.Buttons.Button1_Pressed.Left.png");
            ImageCenter_Selected = LoadImageFromResource("STALKERPDA.Images.Ui.Buttons.Button1_Pressed.Center.png");
            ImageRight_Selected = LoadImageFromResource("STALKERPDA.Images.Ui.Buttons.Button1_Pressed.Right.png");

            ImageLeft.GetImageInfo(out ImageLeftInfo);
            ImageCenter.GetImageInfo(out ImageCenterInfo);
            ImageRight.GetImageInfo(out ImageRightInfo);

            ImageLeft_Selected.GetImageInfo(out ImageLeftInfo_Selected);
            ImageCenter_Selected.GetImageInfo(out ImageCenterInfo_Selected);
            ImageRight_Selected.GetImageInfo(out ImageRightInfo_Selected);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            StartCoords.Clear();
            EndCoords.Clear();

            using (var g = e.Graphics)
            {
                IntPtr hdc = g.GetHdc();

                int x = 0, prevX = 0, i = 0;

                foreach (var text in TabNames)
                {
                    var selected = i == SelectedIndex;

                    if(!selected)
                        ImageLeft.Draw(hdc, new RECT(x, 0, x + (int)ImageLeftInfo.Width, (int)ImageLeftInfo.Height), null);
                    else
                        ImageLeft_Selected.Draw(hdc, new RECT(x, 0, x + (int)ImageLeftInfo_Selected.Width, (int)ImageLeftInfo_Selected.Height), null);

                    x += (int)ImageLeftInfo.Width;

                    StartCoords.Add(x-(ItemOffset/2));
                    if (!selected)
                        ImageCenter.Draw(hdc, new RECT(x, 0, x += (selected ? MidWidthOpen : MidWidthClosed), (int)ImageCenterInfo.Height), null);
                    else
                        ImageCenter_Selected.Draw(hdc, new RECT(x, 0, x += (selected ? MidWidthOpen : MidWidthClosed), (int)ImageCenterInfo_Selected.Height), null);

                    EndCoords.Add(x+(ItemOffset/2));

                    if (!selected)
                        ImageRight.Draw(hdc, new RECT(x, 0, x+= (int)ImageRightInfo.Width, (int)ImageRightInfo.Height), null);
                    else
                        ImageRight_Selected.Draw(hdc, new RECT(x, 0, x += (int)ImageRightInfo_Selected.Width, (int)ImageRightInfo_Selected.Height), null);
                    //x += (int)ImageRightInfo.Width;

                    if (selected)
                    {
                        var size = g.MeasureString(text, Font);
                        g.DrawString(text, Font, new SolidBrush(ForeColor), prevX + ((x - prevX) - size.Width) / 2, (Height - size.Height) / 2);
                    }
                    else
                    {
                        if (_images.Count > i)
                        {
                            ImageInfo info;
                            _images[i].GetImageInfo(out info);
                            int x0 = prevX + ((x - prevX) - (int)info.Width) / 2;
                            int y0 = (int)(Height - info.Height) / 2;
                            _images[i].Draw(hdc, new RECT(x0, y0, x0 + (int)info.Width, y0+ (int)info.Height), null);
                        }
                    }

                    x -= ItemOffset;
                    prevX = x;
                    i++;
                }

                g.ReleaseHdc(hdc);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            int i = 0;
            foreach (var startCoord in StartCoords)
            {
                if (e.X >= startCoord)
                {
                    if (e.X <= EndCoords[i])
                    {
                        if (SelectedIndex != i)
                        {
                            SelectedIndex = i;
                            Invalidate();
                            OnTabChanged.Invoke(this, null);
                        }
                    }
                }

                    i++;
            }
        }

    }
}
