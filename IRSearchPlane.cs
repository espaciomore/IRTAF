/*
 *  Filename:   IRSearchPlane.cs
 *  Author:     Manuel A. Cerda R.
 *  Date:       03-14-2016
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASTSelenium.ImageRecognition
{
    public abstract class IRSearchPlane
    {
        public const int UpperLeft = 1;
        public const int UpperRight = 2;
        public const int LowerLeft = 3;
        public const int LowerRight = 4;

        public static System.Windows.Rect GetSearchPlane(List<int> searchPlanes, System.Windows.Size size)
        {
            var evenWidth = size.Width % 2 == 0 ? size.Width : size.Width + 1;
            var evenHeight = size.Height % 2 == 0 ? size.Height : size.Height + 1;

            var searchPlane = GetSearchPlane(searchPlanes, new System.Windows.Rect(0, 0, evenWidth, evenHeight));

            return searchPlane; 
        }

        private static System.Windows.Rect GetSearchPlane(List<int> searchPlanes, System.Windows.Rect usingPlane)
        {
            if (searchPlanes.Count() == 0)
            {
                return usingPlane;
            }

            System.Windows.Rect newUsingPlane;

            switch (searchPlanes.First())
            { 
                case IRSearchPlane.UpperLeft:
                    newUsingPlane = new System.Windows.Rect(usingPlane.Left, usingPlane.Top, usingPlane.Width / 2, usingPlane.Height / 2);
                    break;
                case IRSearchPlane.UpperRight:
                    newUsingPlane = new System.Windows.Rect(usingPlane.Left + (usingPlane.Width / 2), usingPlane.Top, usingPlane.Width / 2, usingPlane.Height / 2);
                    break;
                case IRSearchPlane.LowerLeft:
                    newUsingPlane = new System.Windows.Rect(usingPlane.Left, usingPlane.Top + (usingPlane.Height / 2), usingPlane.Width / 2, usingPlane.Height / 2);
                    break;
                case IRSearchPlane.LowerRight:
                    newUsingPlane = new System.Windows.Rect(usingPlane.Left + (usingPlane.Width / 2), usingPlane.Top + (usingPlane.Height / 2), usingPlane.Width / 2, usingPlane.Height / 2);
                    break;
                default:
                    newUsingPlane = usingPlane;
                    break;                    
            }

            searchPlanes.RemoveAt(0);

            return GetSearchPlane(searchPlanes, newUsingPlane);
        }
    }
}
