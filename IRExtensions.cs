using System;

namespace FASTSelenium.ImageRecognition
{
    public static class RectangleExtensions
    {
        public static System.Drawing.Bitmap CaptureFromScreen(this System.Windows.Rect rect)
        {
            try
            {
                System.Drawing.Bitmap bmpScreenCapture = new System.Drawing.Bitmap(Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height));
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
                    catch { } //  TODO: investigate why there are random failures
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
                double right = searchSurface != null ? ((System.Windows.Rect)searchSurface).Right : System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                double bottom = searchSurface != null ? ((System.Windows.Rect)searchSurface).Bottom : System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                bool reverse = false;
                double x = left, y = top;
                while (x < (right - bmp1.Width))
                {
                    var imageRect = new System.Windows.Rect(new System.Windows.Point(x, y), new System.Windows.Size(bmp1.Width, bmp1.Height));
                    var imageCaptured = imageRect.CaptureFromScreen();

                    if (bmp1.SameAs(imageCaptured))
                    {
                        return imageRect;
                    }

                    if (IRConfig.canSaveScreenSamples)
                    {
                        if ((y == top && x == left) || (bmp1.GetPixel(0, 0) == imageCaptured.GetPixel(0, 0)))
                            IRHelpers.SaveInReportDir(imageCaptured);
                    }
                    else
                    {
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

            return null;
        }

        public static bool IsVisible(this System.Drawing.Bitmap bmp1, System.Windows.Rect? searchSurface = null)
        {
            return bmp1.GetRectangle(searchSurface).HasValue;
        }

        public static bool SameAs(this System.Drawing.Bitmap bmp1, System.Drawing.Bitmap bmp2)
        {
            try
            {
                if (!bmp1.Size.Equals(bmp2.Size))
                {
                    return false;
                }
                for (int i = 0; i < bmp1.Width; i = 2+i)
                {
                    for (int j = 1; j < bmp1.Height; j = 2+j)
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
