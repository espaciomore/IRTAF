using System;
using System.Collections.Generic;
using System.Drawing;

namespace FASTSelenium.ImageRecognition
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IRFindsBy : System.Attribute
    {
        public readonly int Left = 0;
        public readonly int Top = 0;

        public string URI { get; set; }
        public RotateFlipType RotateOrFlip { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, string> CssValues { get; set; }

        protected int Offset_X { get; set; }
        protected int Offset_Y { get; set; }

        public IRFindsBy() : base() { }

        public void SetOffset(int dx, int dy)
        {
            this.Offset_X = dx;
            this.Offset_Y = dy;
        }

        public System.Windows.Point GetOffset()
        {
            return new System.Windows.Point(this.Offset_X, this.Offset_Y);
        }
    }
}
