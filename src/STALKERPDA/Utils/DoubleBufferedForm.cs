﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OpenNETCF.Drawing.Imaging;

namespace STALKERPDA.Utils
{
    public class DoubleBufferedForm : Form, IBackgroundPaintProvider
    {
        protected Bitmap m_bmBuffer;
        protected Graphics m_gBuffer;
        protected ImagingFactoryClass m_factory;

        public DoubleBufferedForm()
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
        /*
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupBackground();
            Invalidate();
        }
        */
        protected virtual void SetupBackground()
        {

        }

        #region IBackgroundPaintProvider Members

        void IBackgroundPaintProvider.PaintBackground(Graphics g, Rectangle targetRect, Rectangle sourceRect)
        {
            g.DrawImage(m_bmBuffer, targetRect, sourceRect, GraphicsUnit.Pixel);
        }
        /*
        Image IBackgroundPaintProvider.BackgroundImage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }*/

        #endregion
    }
}
