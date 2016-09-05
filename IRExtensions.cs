/*
 *  Filename:   IRExtensions.cs
 *  Author:     Manuel A. Cerda R.
 *  Date:       03-14-2016
 */
using System;

namespace FASTSelenium.ImageRecognition
{
    public static class RectangleExtensions
    {
        public static System.Drawing.Bitmap CaptureFromScreen(this System.Windows.Rect rect)
        {
            try
            {
                System.Drawing.Bitmap bmpScreenCapture = new System.Drawing.Bitmap(Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                using (System.Drawing.Graphics p = System.Drawing.Graphics.FromImage(bmpScreenCapture))
                {
                    try
                    {
                        p.CopyFromScreen(
                            sourceX: Convert.ToInt32(rect.Location.X), 
                            sourceY: Convert.ToInt32(rect.Location.Y), 
                            destinationX: 0, destinationY: 0, 
                            blockRegionSize: new System.Drawing.Size(Convert.ToInt32(rect.Size.Width), Convert.ToInt32(rect.Size.Height)),
                            copyPixelOperation: System.Drawing.CopyPixelOperation.SourceCopy
                        );
                    }
                    catch (ArgumentNullException ex) 
                    {
                        throw ex;
                    }
                    catch (NullReferenceException ex)
                    {
                        throw ex;
                    }
                    catch (ArgumentException ex)
                    {
                        throw ex;
                    }
                }

                return bmpScreenCapture;
            }
            catch (Exception ex)
            {
                throw new Exception("RectangleExtensions.CaptureFromScreen failed: " + ex.Message);
            }
        }

        public static System.Windows.Point GetCenterPoint(this System.Windows.Rect r)
        {
            var xCenter = ((r.Width % 2 != 0) ? (r.Width + 1) : (r.Width)) / 2;
            var yCenter = ((r.Height % 2 != 0) ? (r.Height + 1) : (r.Height)) / 2;

            return new System.Windows.Point(r.Location.X + xCenter, r.Location.Y + yCenter);
        }

        public static System.Drawing.Point ToDrawingPoint(this System.Windows.Point p)
        {
            return new System.Drawing.Point((int)p.X, (int)p.Y);
        }
    }

    public static class BitmapExtensions
    {
        public static System.Windows.Rect? GetRectangle(this System.Drawing.Bitmap bmp1, System.Windows.Rect? searchSurface = null)
        {
            try
            {
                double left = searchSurface != null ? ((System.Windows.Rect)searchSurface).Left : 0;
                double top = searchSurface != null ? ((System.Windows.Rect)searchSurface).Top : 0;
                double right = searchSurface != null && ((System.Windows.Rect)searchSurface).Right > 0 ? ((System.Windows.Rect)searchSurface).Right : System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                double bottom = searchSurface != null && ((System.Windows.Rect)searchSurface).Bottom > 0 ? ((System.Windows.Rect)searchSurface).Bottom : System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                bool reverse = false;
                double x = left, y = top;
                while (x < (right - bmp1.Width))
                {
                    var imageRect = new System.Windows.Rect(new System.Windows.Point(x, y), new System.Windows.Size(bmp1.Width * 1.2, bmp1.Height * 1.2));
                    var imageCaptured = imageRect.CaptureFromScreen();

                    if (imageCaptured != null)
                    {
                        if (imageCaptured.Contains(bmp1))
                        {
                            IRHelpers.SaveInReportDir(imageCaptured);
                            imageCaptured.Dispose();
                            bmp1.Dispose();

                            return imageRect;
                        }

                        if (IRConfig.canSaveScreenSamples)
                        {
                            if (IRConfig.saveAllSamples || (y == top && x == left) || (bmp1.GetPixel(0, 0) == imageCaptured.GetPixel(0, 0)))
                                IRHelpers.SaveInReportDir(imageCaptured);
                        }
                        
                        imageCaptured.Dispose();
                    }

                    if (reverse == false && y == (bottom - bmp1.Height))
                    {
                        reverse = true;
                        x++;
                    }
                    else if (reverse == true && y == top)
                    {
                        reverse = false;
                        x++;
                    }
                    else
                    {
                        y = reverse ? (y - 1) : (y + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BitmapExtensions.GetRectangle failed: " + ex.Message);
            }
            finally
            {
                bmp1.Dispose();
            }

            return null;
        }

        public static bool IsVisible(this System.Drawing.Bitmap bmp1, System.Windows.Rect? searchSurface = null)
        {
            return bmp1.GetRectangle(searchSurface).HasValue;
        }

        public static bool Contains(this System.Drawing.Bitmap bmp1, System.Drawing.Bitmap bmp2)
        {
            try
            {
                try
                {
                    var threshold = (bmp1.Width * bmp1.Height * 0.4);
                    var hitCount = 0;

                    for (int i = 0; i < (bmp1.Width * 0.8); i = 1 + i)
                    {
                        for (int j = 0; j < (bmp1.Height * 0.8); j = 1 + j)
                        {
                            if (hitCount >= threshold)
                                return true;
                            else
                                hitCount = 0;

                            for (int m = 0; m < (bmp2.Width - i); m = 1 + m)
                            {
                                var p1 = bmp1.GetPixel(i + m, j + 0);
                                var p2 = bmp2.GetPixel(m, 0);
                                if (!p1.SameAs(p2))
                                    break;

                                for (int n = 0; n < (bmp2.Height - j); n = 1 + n)
                                {
                                    p1 = bmp1.GetPixel(i + m, j + n);
                                    p2 = bmp2.GetPixel(m, n);
                                    if (!p1.SameAs(p2))
                                        break;
                                    else
                                        hitCount++;
                                }
                            }
                        }
                    }

                    return (hitCount >= threshold);
                }
                catch (ArgumentNullException ex)
                {
                    throw ex;
                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
                catch (ArgumentException ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BitmapExtensions.SameAs failed: " + ex.Message);
            }
        }

        public static bool SameAs(this System.Drawing.Color c1, System.Drawing.Color c2)
        {
            return Math.Abs(c1.A - c2.A) <= 10 && Math.Abs(c1.B - c2.B) <= 5 && Math.Abs(c1.G - c2.G) <= 5 && Math.Abs(c1.R - c2.R) <= 5;
        }
    }
    
    public static class PointExtensions
    {
        public static System.Drawing.Color GetColor(this System.Windows.Point p)
        {
            System.Windows.Rect rect = new System.Windows.Rect(p, new System.Windows.Size(1, 1));
            using (System.Drawing.Bitmap map = rect.CaptureFromScreen())
            {
                return map.GetPixel(0, 0);
            }
        }

        public static System.Windows.Point Translate(this System.Windows.Point p1)
        {
            System.Windows.Size from = IRConfig.screenSize;
            System.Windows.Size to = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Convert();

            var p2 = new System.Windows.Point(p1.X + (to.Width - from.Width), p1.Y + (to.Height - from.Height));
            var currentPoint = new System.Windows.Point(
                p1.X + (p2.X > to.Width ? Math.Abs(to.Width - from.Width) : 0),
                p1.Y + (p2.Y > to.Height ? Math.Abs(to.Height - from.Height) : 0)
            );

            return currentPoint;
        }

        public static System.Windows.Size ToSize(this System.Drawing.Point p)
        {
            return new System.Windows.Size(p.X, p.Y);
        }

        public static System.Drawing.Point Convert(this System.Windows.Point p)
        {
            return new System.Drawing.Point(System.Convert.ToInt32(p.X), System.Convert.ToInt32(p.Y));
        }

        public static System.Windows.Point Convert(this System.Drawing.Point p)
        {
            return new System.Windows.Point(System.Convert.ToDouble(p.X), System.Convert.ToDouble(p.Y));
        }
    }

    public static class SizeExtensions
    {
        public static System.Drawing.Size Convert(this System.Windows.Size p)
        {
            return new System.Drawing.Size(System.Convert.ToInt32(p.Width), System.Convert.ToInt32(p.Height));
        }

        public static System.Windows.Size Convert(this System.Drawing.Size p)
        {
            return new System.Windows.Size(System.Convert.ToDouble(p.Width), System.Convert.ToDouble(p.Height));
        }    
    }
}
