using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpenNETCF.Drawing.Imaging;
using STALKERPDA.Utils;

namespace STALKERPDA.Controls
{
    public partial class CustomButton : UserControl
    {
        private ImagingFactoryClass m_factory;

        public string DefaultImage { get { return dimg; } set { dimg = value; GetImage(dimg, out defImage); } }
        public string PressedImage { get { return pimg; } set { pimg = value; GetImage(pimg, out pressImage); } }
        public string Caption { get; set; }
        public bool SwitchMode { get; set; }
        public bool IsPressed { get { return isPress; } set { isPress = value; Invalidate(); } }

        private string dimg, pimg;

        private IImage defImage, pressImage;
        private bool isPress;

        public CustomButton()
        {
            m_factory = new ImagingFactoryClass();

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            IBackgroundPaintProvider bgPaintProvider = Parent as IBackgroundPaintProvider;
            if (bgPaintProvider != null)
            {
                Rectangle rcPaint = e.ClipRectangle;
                rcPaint.Offset(Left, Top);
                bgPaintProvider.PaintBackground(e.Graphics, e.ClipRectangle, rcPaint);
            }

            //base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                IntPtr hdc = g.GetHdc();

                var img = IsPressed ? pressImage : defImage;

                if (img != null)
                {
                    img.Draw(hdc, new RECT(0, 0, this.Width, this.Height), null);
                }

                if (!string.IsNullOrEmpty(Caption))
                {
                    var size = g.MeasureString(Caption, Font);
                    g.DrawString(Caption, Font, new SolidBrush(ForeColor), (Width - size.Width) / 2, (Height - size.Height) / 2);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);  
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (SwitchMode)
                IsPressed = !IsPressed;
            else
                IsPressed = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!SwitchMode) IsPressed = false;
        }

        private void GetImage(string path, out IImage img)
        {
            img = null;
            if (string.IsNullOrEmpty(path)) return;
            StreamOnFile sof = new StreamOnFile(GetType().Assembly.GetManifestResourceStream(path));
            if (sof == null) return;
            m_factory.CreateImageFromStream(sof, out img);
            //ImageInfo ii;
            //imgBlank.GetImageInfo(out ii);
        }
    }
}
