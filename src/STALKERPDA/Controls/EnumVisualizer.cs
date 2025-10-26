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
    public partial class EnumVisualizer : TransparentControl
    {
        public List<string> Images { set {
            foreach(var img in value)
            {
                _images.Add(LoadImageFromResource(img));
            }
            Invalidate();
        } }

        private List<IImage> _images = new List<IImage>();

        [DefaultValue(0)]
        public int Value { set { _value = value; Invalidate(); } get { return _value; } }

        private int _value;

        public EnumVisualizer()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                IntPtr hdc = g.GetHdc();

                if (_value >= 0 && _value < _images.Count())
                {
                    var img = _images[_value];
                    if (img != null)
                    {
                        img.Draw(hdc, new RECT(0, 0, this.Width, this.Height), null);
                    }
                }
            }
        }
    }
}
