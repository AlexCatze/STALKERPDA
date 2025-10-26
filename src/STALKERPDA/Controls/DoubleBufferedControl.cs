using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using STALKERPDA.Utils;
using System.Windows.Forms;
using System.Drawing;
using OpenNETCF.Drawing.Imaging;

namespace STALKERPDA.Controls
{
    public class DoubleBufferedControl : UserControl, IBackgroundPaintProvider
    {
        protected Bitmap m_bmBuffer;
        protected Graphics m_gBuffer;
        protected ImagingFactoryClass m_factory;

        public DoubleBufferedControl()
        {
            m_bmBuffer = new Bitmap(1, 1);
            m_factory = new ImagingFactoryClass();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (m_bmBuffer != null)
                m_bmBuffer.Dispose();
            if (m_gBuffer != null)
                m_gBuffer.Dispose();

            m_bmBuffer = null;
            m_gBuffer = null;

            if (Width * Height == 0)
                return;
            m_bmBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            m_gBuffer = Graphics.FromImage(m_bmBuffer);
            SetupBackground();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.DrawImage(m_bmBuffer, 0, 0);
        }

        protected virtual void SetupBackground()
        {

        }

        #region IBackgroundPaintProvider Members

        public void PaintBackground(Graphics g, Rectangle targetRect, Rectangle sourceRect)
        {
            g.DrawImage(m_bmBuffer, targetRect, sourceRect, GraphicsUnit.Pixel);
        }

        #endregion

        protected IImage LoadImageFromResource(string path)
        {
            IImage img = null;
            if (string.IsNullOrEmpty(path)) return null;
            StreamOnFile sof = new StreamOnFile(GetType().Assembly.GetManifestResourceStream(path));
            if (sof == null) return null;
            m_factory.CreateImageFromStream(sof, out img);
            return img;
        }
    }
}
