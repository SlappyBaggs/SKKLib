#define USE_THIS1

using System;
using System.Drawing;
using System.Drawing.Imaging;
using SKKLib.Handlers.Win32;
//using RGiesecke.DllExport;

namespace SKKLib.Imaging
{
    public delegate void ImagingErrorEventHandler(Exception e);
    public delegate void ImagingDebugEventHandler(string dbg);

    public static class ImageCore
    {
        private static object _locker = new object();

        public static string ClassifyColor(Color c)
        {
            float hue = c.GetHue();
            float sat = c.GetSaturation();
            float lgt = c.GetBrightness();

            if (lgt < 0.2) return "Blacks";
            if (lgt > 0.8) return "Whites";

            if (sat < 0.25) return "Grays";

            if (hue < 30) return "Reds";
            if (hue < 90) return "Yellows";
            if (hue < 150) return "Greens";
            if (hue < 210) return "Cyans";
            if (hue < 270) return "Blues";
            if (hue < 330) return "Magentas";
            return "Reds";
        }

        public static bool ColorsWithinTolerance(Color c1, Color c2, int tol = 0)
        {
            /*
            DoDebug("ColorsWithinTolerance(" + c1.ToString() + ", " + c2.ToString() + ", " + tol.ToString() + ")");
            DoDebug("  R: " + (c1.R - tol).ToString() + " / " + c2.R.ToString() + " / " + (c1.R + tol).ToString());
            DoDebug("  G: " + (c1.G - tol).ToString() + " / " + c2.G.ToString() + " / " + (c1.G + tol).ToString());
            DoDebug("  B: " + (c1.B - tol).ToString() + " / " + c2.B.ToString() + " / " + (c1.B + tol).ToString());
            */
            /*
            bool[] b = new bool[6];
            b[0] = (c1.R - tol) <= c2.R;
            b[1] = (c1.R + tol) >= c2.R;
            b[2] = (c1.G - tol) <= c2.G;
            b[3] = (c1.G + tol) >= c2.G;
            b[4] = (c1.B - tol) <= c2.B;
            b[5] = (c1.B + tol) >= c2.B;
            string res = string.Empty;
            foreach (bool bb in b) res += (bb ? "YES" : "no") + ", ";
            DoDebug(c1.ToString() + "==" + c2.ToString() + " - " + res);
            */
            return (((c1.R - tol) <= c2.R) &&
                ((c1.R + tol) >= c2.R) &&
                ((c1.G - tol) <= c2.G) &&
                ((c1.G + tol) >= c2.G) &&
                ((c1.B - tol) <= c2.B) &&
                ((c1.B + tol) >= c2.B));
        }


        public static event ImagingErrorEventHandler ImagingErrorEvent;
        public static event ImagingDebugEventHandler ImagingDebugEvent;


        private static void DoImageError(Exception e)
        {
            if (ImagingErrorEvent != null) ImagingErrorEvent(e);
        }

        private static void DoDebug(string s)
        {
            if (ImagingDebugEvent != null) ImagingDebugEvent(s + Environment.NewLine);
        }

        private static Size size1 = new Size(1, 1);

        private static Bitmap currentScreen_ = null;
        public static Bitmap CurrentScreen { get { if (currentScreen_ == null) UpdateScreen(); return currentScreen_; } private set { currentScreen_ = value; } }

        public static void UpdateScreen()
        {
            if (currentScreen_ != null) currentScreen_.Dispose();
            CurrentScreen = ScreenGrabber.CaptureScreen();
        }

        public static Color GetColorFromScreen(Point p) => GetColorFromScreen(p.X, p.Y);

        /*
                [DllExport()]
                public static Int32 GetColFromScr(int x, int y) => GetColorFromScreen(x, y).ToArgb();

                [DllExport()]
                public static Color GetColorFromScreenPY(int x, int y) => GetColorFromScreen(x, y);
        */
        public static Color GetColorFromScreen(int x, int y)
        {
#if BADONGO
            return CurrentScreen.GetPixel(x, y);
#else
            Color ret = Color.Transparent;
            using (Bitmap bm = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    try
                    {
                        g.CopyFromScreen(x, y, 0, 0, size1);
                        ret = bm.GetPixel(0, 0);
                    }
                    catch (Exception ex)
                    {
                        DoImageError(ex);
                    }
                }
            }
            return ret;
#endif
        }

        public static Bitmap GetBMPFromScreen(Rectangle r) => GetBMPFromScreen(r.X, r.Y, r.Width, r.Height);
        public static Bitmap GetBMPFromScreen(int x, int y, int w, int h) => GetBMPFromScreen(x, y, w, h, PixelFormat.Format32bppArgb);
        public static Bitmap GetBMPFromScreen(int x, int y, int w, int h, PixelFormat pf)
        {
            try
            {
                //ret = new Bitmap(w, h, pf);// PixelFormat.Format32bppArgb);// .Format24bppRgb);
                using (Bitmap ret = new Bitmap(w, h, pf))
                {
                    using (Graphics g = Graphics.FromImage(ret))
                    {
                        //Rectangle rectX = new Rectangle(x, y, w, h);// Screen.AllScreens[1].Bounds;
                        Rectangle rect = new Rectangle(x, y, w, h);// Screen.AllScreens[1].Bounds;

                        // Do these do anything???
                        //rectX.Width = w;
                        //rectX.Height = h;
                        try
                        {
                            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
                        }
                        catch (Exception e)
                        {
                            DoImageError(e);
                            //System.Diagnostics.Debugger.Break();
                        }
                    }
                    return (Bitmap)ret.Clone();
                }
            }
            catch (ArgumentException e)
            {
                DoImageError(e);
            }
            return null;
        }

        public static Image ResizeIMG(Image img, float fac) { lock (_locker) { return ResizeIMG(img, (int)(img.Width * fac), (int)(img.Height * fac)); } }
        public static Image ResizeIMG(Image img, int w, int h)
        {
            lock (_locker)
            {
                try
                {
                    using (Image ret = new Bitmap(w, h))
                    {
                        using (Graphics g = Graphics.FromImage(ret))
                        {
                            g.DrawImage(img, 0, 0, w, h);
                        }
                        return (Image)ret.Clone();
                    }
                }
                catch (Exception e)
                {
                    DoImageError(e);
                }
                return null;
            }
        }

        public static Bitmap ResizeBMP(Bitmap bmp, float fac) { lock (_locker) { return ResizeBMP(bmp, (int)(bmp.Width * fac), (int)(bmp.Height * fac)); } }
        public static Bitmap ResizeBMP(Bitmap bmp, int w, int h)
        {
            lock (_locker)
            {
                try
                {
                    using (Bitmap ret = new Bitmap(w, h))
                    {
                        using (Graphics g = Graphics.FromImage(ret))
                        {
                            g.DrawImage(bmp, 0, 0, w, h);
                        }
                        return (Bitmap)ret.Clone();
                    }
                }
                catch (Exception e)
                {
                    DoImageError(e);
                }
                return null;
            }
        }

        private static Rectangle destRec = new Rectangle(0, 0, 140, 108);


        public static Bitmap GetSubBMP(Bitmap bmp, Rectangle r) => GetSubBMP(bmp, r.X, r.Y, r.Width, r.Height);
        public static Bitmap GetSubBMP(Bitmap bmp, Point p, Size s) => GetSubBMP(bmp, p.X, p.Y, s.Width, s.Height);
        public static Bitmap GetSubBMP(Bitmap bmp, int sx, int sy, Size s) => GetSubBMP(bmp, sx, sy, s.Width, s.Height);
        public static Bitmap GetSubBMP(Bitmap bmp, Point p, int w, int h) => GetSubBMP(bmp, p.X, p.Y, w, h);
        public static Bitmap GetSubBMP(Bitmap bmp, int sx, int sy, int w, int h) => GetSubBMP(bmp, destRec, sx, sy, w, h);
        public static Bitmap GetSubBMP(Bitmap bmp, Rectangle dr, int sx, int sy, int w, int h)
        {
            try
            {
                using (Bitmap ret = new Bitmap(w, h))
                {
                    using (Graphics g = Graphics.FromImage(ret))
                    {
                        g.DrawImage(bmp, dr, sx, sy, w, h, GraphicsUnit.Pixel);
                    }
                    return (Bitmap)ret.Clone();
                }
            }
            catch (Exception e)
            {
                DoImageError(e);
            }
            return null;
        }

        public static Bitmap AddRect(Bitmap bmp, Rectangle r, Color c, float w = 1.0f)
        {
            try
            {
                using (Pen p = new Pen(c, w))
                {
                    return AddRect(bmp, r, p);
                }
            }
            catch (Exception ex)
            {
                DoImageError(ex);
            }
            return bmp;
        }

        public static Bitmap AddRect(Bitmap bmp, Rectangle r, Brush b, float w = 1.0f)
        {
            try
            {
                using (Pen p = new Pen(b, w))
                {
                    return AddRect(bmp, r, p);
                }
            }
            catch (Exception ex)
            {
                DoImageError(ex);
            }
            return bmp;
        }
        
        public static Bitmap AddRect(Bitmap bmp, Rectangle r, Pen p)
        {
            try
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawRectangle(p, r);
                }
            }
            catch (Exception ex)
            {
                DoImageError(ex);
            }

            return bmp;
        }
    }

    public static class ScreenGrabber
    {
        public static ImageFormat ImgFormat { get; set; } = ImageFormat.Bmp;

        /*
         *  -1 for 'imgW' or 'imgH' means to use screen Width and Height
         */
        
        // No parameters
        public static Bitmap CaptureScreen() => CaptureScreen(ImgFormat);
       
        // ImageFormat only
        public static Bitmap CaptureScreen(ImageFormat imageFormat) => CaptureScreen(0, 0, imageFormat);

        // Source Rectangle Only
        public static Bitmap CaptureScreen(Rectangle srcRect) => CaptureScreen(srcRect, ImgFormat);

        // Source Rectangle and Image Format
        public static Bitmap CaptureScreen(Rectangle srcRect, ImageFormat imageFormat) => CaptureScreen(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, imageFormat);

        // srcX, srcY only
        public static Bitmap CaptureScreen(int srcX, int srcY) => CaptureScreen(srcX, srcY, ImgFormat);

        // srcX, srcY and ImageFormat only
        public static Bitmap CaptureScreen(int srcX, int srcY, ImageFormat imageFormat) => CaptureScreen(srcX, srcY, -1, -1, ImgFormat);

        // X, Y, Width, Height only
        public static Bitmap CaptureScreen(int srcX, int srcY, int imgW, int imgH) => CaptureScreen(srcX, srcY, imgW, imgH, ImgFormat);
        
        // X, Y, Width, Height and ImageFormat
        public static Bitmap CaptureScreen(int srcX, int srcY, int imgW, int imgH, ImageFormat imageFormat)
        {

#if !USE_THIS1
            return ImageCore.GetBMPFromScreen(srcX, srcY, imgW, imgH);
#else
            // SHould we check is 'srcX' or 'srcY' is greater than 'imgW' or 'imgH'?
            int hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
            int hdcDest = GDI32.CreateCompatibleDC(hdcSrc);

            // Grab the screen W and H
            int screenW = GDI32.GetDeviceCaps(hdcSrc, 8);
            int screenH = GDI32.GetDeviceCaps(hdcSrc, 10);

            // If 'imgW' or 'imgH' is -1, use 'srcW' or 'srcH' instead
            // Otherwise, use what was passed in for 'imgW' and 'imgH'
            int ww = (imgW == -1) ? screenW : imgW;
            int hh = (imgH == -1) ? screenH : imgH;

            if((ww + srcX) > screenW) ww = screenW - srcX;
            if((hh + srcY) > screenH) hh = screenH - srcY;
            imgW = ww; 
            imgH = hh;
#endif

#if! USE_THIS2
            return ImageCore.GetBMPFromScreen(srcX, srcY, imgW, imgH);
#else
            int hBitmap = GDI32.CreateCompatibleBitmap(
                hdcSrc,
                ww,
                hh);
            GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(
                hdcDest,                            // Handle to Dest
                0,                                  // Dest X
                0,                                  // Dest Y
                ww,                                 // Width
                hh,                                 // Height
                hdcSrc,                             // Handle to Source
                srcX,                               // Source X
                srcY,                               // Source Y
                0x00CC0020);                        // dwRop Raster Operation
            /*using (*/
            Bitmap ret = new Bitmap(
                Image.FromHbitmap(new IntPtr(hBitmap)),
                Image.FromHbitmap(new IntPtr(hBitmap)).Width,
                Image.FromHbitmap(new IntPtr(hBitmap)).Height)/*)*/;
            //{
                Cleanup(hBitmap, hdcSrc, hdcDest);
                return ret;
            //}
#endif
        }


        private static void Cleanup(int hBitmap, int hdcSrc, int hdcDest)
        {
            User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteObject(hBitmap);
        }
    }
}
