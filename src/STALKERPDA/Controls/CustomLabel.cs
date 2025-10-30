using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace STALKERPDA.Controls
{
    public partial class CustomLabel : TransparentControl
    {
        public CustomLabel()
        {
            InitializeComponent();
        }

        [DefaultValue("")]
        public string Value { set { _value = value; Invalidate(); } get { return _value; } }

        private string _value;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                if (!string.IsNullOrEmpty(_value))
                {
                    var size = g.MeasureString(_value, Font);
                    g.DrawString(_value, Font, new SolidBrush(ForeColor), (Width - size.Width) / 2, (Height - size.Height) / 2);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }
    }
}
