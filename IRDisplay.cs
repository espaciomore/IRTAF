/*
 *  Filename:   IRDisplay.cs
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
    public static class IRDisplay
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        public static void DrawHighlight(System.Drawing.Pen pen, System.Drawing.Rectangle rect)
        {
            System.Drawing.Graphics.FromHdc(CreateDC("DISPLAY", null, null, IntPtr.Zero)).DrawRectangle(pen, rect);
        }
    }
}
