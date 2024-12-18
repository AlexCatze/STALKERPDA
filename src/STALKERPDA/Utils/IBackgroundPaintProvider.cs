using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace STALKERPDA.Utils
{
    public interface IBackgroundPaintProvider
    {
        void PaintBackground(Graphics g, Rectangle targetRect, Rectangle sourceRect);
        //Image BackgroundImage { get; set; }
    }
}
