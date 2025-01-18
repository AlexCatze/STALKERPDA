using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using STALKERPDA.Utils;
using System.Drawing;
using OpenNETCF.Drawing.Imaging;

namespace STALKERPDA.Controls
{
    public class TransparentControl : DoubleBufferedControl
    {
        public TransparentControl() : base()
        {

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
        }
    }
}
