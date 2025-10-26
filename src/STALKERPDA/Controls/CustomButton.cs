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
    public partial class CustomButton : TransparentControl
    {
        private ImagingFactoryClass m_factory;

        public string DefaultImage { get { return dimg; } set { dimg = value; GetImage(dimg, out defImage); } }
        public string PressedImage { get { return pimg; } set { pimg = value; GetImage(pimg, out pressImage); } }
        public string Caption { get; set; }
        public bool SwitchMode { get; set; }
        public bool IsPressed { get { return isPress; } set { isPress = value; Invalidate(); } }

        public bool Repeat { get; set; }

        [DefaultValue(400)]
        public int InitialDelay { set; get; }

        [DefaultValue(62)]
        public int RepeatInterval { set; get; }

        private string dimg, pimg;

        private IImage defImage, pressImage;
        private bool isPress;

        public CustomButton()
        {
            m_factory = new ImagingFactoryClass();

            InitialDelay = 400;
            RepeatInterval = 62;

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
                g.ReleaseHdc(hdc);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (SwitchMode)
                IsPressed = !IsPressed;
            else
                IsPressed = true;

            if (Repeat)
                timer1_Tick(null, EventArgs.Empty);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!SwitchMode) IsPressed = false;

            timer1.Enabled = false;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            OnTick.Invoke(this, e);
            if (timer1.Enabled)
                timer1.Interval = RepeatInterval;
            else
                timer1.Interval = InitialDelay;

            timer1.Enabled = true;
        }

        public event EventHandler OnTick;
    }
}
