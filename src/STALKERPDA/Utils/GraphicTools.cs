using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using OpenNETCF.Drawing.Imaging;

namespace STALKERPDA
{
    class GraphicTools
    {
        private static Bitmap LoadTransparentBitmap(Stream stm)
        {
            ImagingFactory factory = new ImagingFactoryClass();
            IImage img = null;
            factory.CreateImageFromStream(new StreamOnFile(stm), out img);
            ImageInfo ii;
            img.GetImageInfo(out ii);
            Bitmap bm = new Bitmap((int)ii.Width, (int)ii.Height, ii.PixelFormat);
            Graphics g = Graphics.FromImage(bm);
            g.Clear(Color.Black);
            IntPtr hdc = g.GetHdc();
            img.Draw(hdc, new RECT(0, 0, bm.Width, bm.Height), null);
            g.ReleaseHdc(hdc);
            g.Dispose();
            Marshal.ReleaseComObject(img);
            Marshal.ReleaseComObject(factory);
            return bm;
        }

        public static void DrawTransparent(IntPtr hdcDest, Rectangle rcDest, IntPtr hdcSrc, Rectangle rcSrc, bool useTransparency, byte constAlpha)
        {
            BLENDFUNCTION bf = new BLENDFUNCTION();
            bf.AlphaFormat = useTransparency ? AlphaFormat.ALPHA : 0;
            bf.BlendFlags = 0;
            bf.BlendOp = BlendOp.SRC_OVER;
            bf.SourceConstantAlpha = constAlpha;
            AlphaBlend(hdcDest, rcDest.Left, rcDest.Top, rcDest.Width, rcDest.Height, hdcSrc, rcSrc.Left, rcSrc.Top, rcSrc.Width, rcSrc.Height, bf);
        }

        public static void DrawTransparentBitmap(Graphics g, Bitmap bm, Rectangle rcTarget, Rectangle rcSource)
        {
            IntPtr hdcTarget = g.GetHdc();
            using (Graphics gSource = Graphics.FromImage(bm))
            {
                IntPtr hdcSrc = gSource.GetHdc();
                try
                {
                    DrawTransparent(hdcTarget, rcTarget, hdcSrc, rcSource, true, 255);
                }
                finally
                {
                    gSource.ReleaseHdc(hdcSrc);
                    g.ReleaseHdc(hdcTarget);
                }
            }
        }

        public static void TransparentClearBitmap(Bitmap bmp)
        {
            System.Drawing.Imaging.BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, (System.Drawing.Imaging.PixelFormat)PixelFormat.F32bppARGB);
            for (int y = 0; y < bd.Height; y++)
            {
                IntPtr p = (IntPtr)(y * bd.Stride + bd.Scan0.ToInt64());
                for (int x = 0; x < bd.Stride; x++)
                    System.Runtime.InteropServices.Marshal.WriteByte(p, x, 0);
            }
            bmp.UnlockBits(bd);
        }



        [DllImport("coredll", SetLastError = true)]
        public static extern bool AlphaBlend(
            IntPtr hdcDest,
            int nXOriginDest,
            int nYOriginDest,
            int nWidthDest,
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc,
            int nYOriginSrc,
            int nWidthSrc,
            int nHeightSrc,
            BLENDFUNCTION blendFunction);
    }

    //
    // currentlly defined blend function
    //

    public enum BlendOp : byte
    {
        SRC_OVER = 0x00,
    }

    //
    // alpha format flags
    //

    public enum AlphaFormat : byte
    {
        ALPHA = 0x01,      // premultiplied alpha
        ALPHA_NONPREMULT = 0x02,      // non-premultiplied alpha
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BLENDFUNCTION
    {
        public BlendOp BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public AlphaFormat AlphaFormat;
    }

    public enum PixelFormat
    {
        Indexed = 0x00010000, // Indexes into a palette
        GDI = 0x00020000, // Is a GDI-supported format
        Alpha = 0x00040000, // Has an alpha component
        PAlpha = 0x00080000, // Pre-multiplied alpha
        Extended = 0x00100000, // Extended color 16 bits/channel
        Canonical = 0x00200000,

        Undefined = 0,
        DontCare = 0,

        F1bppIndexed = (1 | (1 << 8) | Indexed | GDI),
        F4bppIndexed = (2 | (4 << 8) | Indexed | GDI),
        F8bppIndexed = (3 | (8 << 8) | Indexed | GDI),
        F16bppRGB555 = (5 | (16 << 8) | GDI),
        F16bppRGB565 = (6 | (16 << 8) | GDI),
        F16bppARGB1555 = (7 | (16 << 8) | Alpha | GDI),
        F24bppRGB = (8 | (24 << 8) | GDI),
        F32bppRGB = (9 | (32 << 8) | GDI),
        F32bppARGB = (10 | (32 << 8) | Alpha | GDI | Canonical),
        F32bppPARGB = (11 | (32 << 8) | Alpha | PAlpha | GDI),
        F48bppRGB = (12 | (48 << 8) | Extended),
        F64bppARGB = (13 | (64 << 8) | Alpha | Canonical | Extended),
        F64bppPARGB = (14 | (64 << 8) | Alpha | PAlpha | Extended),
        Max = 15,
    }
}
