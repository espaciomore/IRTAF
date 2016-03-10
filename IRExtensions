using System;
using System.Drawing;

namespace FASTSelenium.ImageRecognition
{
    public static class RectangleExtensions
    {
        public static Bitmap CaptureFromScreen(this Rectangle rect)
        {
            try
            {
                Bitmap bmpScreenCapture = new Bitmap(rect.Width, rect.Height);
                using (Graphics p = Graphics.FromImage(bmpScreenCapture))
                {
                    try
                    {
                        p.CopyFromScreen(sourceX: rect.Location.X, sourceY: rect.Location.Y, destinationX: 0, destinationY: 0, blockRegionSize: rect.Size, copyPixelOperation: CopyPixelOperation.SourceCopy);
                    }
                    catch (Exception)
                    {
                        //  TODO: investigate why there are random failures
                    }
                }

                return bmpScreenCapture;
            }
            catch (Exception ex)
            {
                throw new Exception("RectangleExtensions.CaptureFromScreen failed: " + ex.Message);
            }
        }

        public static Point GetCenterPoint(this Rectangle r)
        {
            var xCenter = ((r.Width % 2 != 0) ? (r.Width + 1) : (r.Width)) / 2;
            var yCenter = ((r.Height % 2 != 0) ? (r.Height + 1) : (r.Height)) / 2;

            return new Point(r.Location.X + xCenter, r.Location.Y + yCenter);
        }
    }

    public static class BitmapExtensions
    {
        public static Rectangle? GetRectangle(this Bitmap bmp1, Rectangle? searchSurface = null)
        {
            try
            {
                int left = searchSurface != null ? ((Rectangle)searchSurface).Left : 0;
                int top = searchSurface != null ? ((Rectangle)searchSurface).Top : 0;
                int right = searchSurface != null ? ((Rectangle)searchSurface).Right : System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int bottom = searchSurface != null ? ((Rectangle)searchSurface).Bottom : System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                bool reverse = false;
                int x = left, y = top;
                while (x < (right - bmp1.Width))
                {
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

                    var imageRect = new Rectangle(new Point(x, y), new Size(bmp1.Width, bmp1.Height));
                    var imageCaptured = imageRect.CaptureFromScreen();

                    if (bmp1.SameAs(imageCaptured))
                    {
                        if (IRConfig.canSaveScreenSamples)
                            IRHelpers.SaveInReportDir(imageCaptured);

                        return imageRect;
                    }

                    if (IRConfig.canSaveScreenSamples)
                        IRHelpers.SaveInReportDir(imageCaptured);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BitmapExtensions.GetRectangle failed: " + ex.Message);
            }

            return null;
        }

        public static bool IsVisible(this Bitmap bmp1, Rectangle? searchSurface = null)
        {
            return bmp1.GetRectangle(searchSurface).HasValue;
        }

        public static bool SameAs(this Bitmap bmp1, Bitmap bmp2)
        {
            try
            {
                if (!bmp1.Size.Equals(bmp2.Size))
                {
                    return false;
                }
                for (int i = 0; i < bmp1.Width; ++i)
                {
                    for (int j = 0; j < bmp1.Height; ++j)
                    {
                        var p1 = bmp1.GetPixel(i, j);
                        var p2 = bmp2.GetPixel(i, j);
                        if (p1 != p2)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("BitmapExtensions.SameAs failed: " + ex.Message);
            }
        }
    }
    
    public static class PointExtensions
    {
        public static Color GetColor(this Point p)
        {
            Rectangle rect = new Rectangle(p, new Size(1, 1));
            Bitmap map = rect.CaptureFromScreen();
            Color c = map.GetPixel(0, 0);
            map.Dispose();

            return c;
        }
    }
}
