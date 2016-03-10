using System;
using System.Collections.Generic;
using System.Drawing;

namespace FASTSelenium.ImageRecognition
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IRFindsBy : System.Attribute
    {
        public string URI { get; set; }
        public RotateFlipType RotateOrFlip { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Offset_X { get; set; }
        public int Offset_Y { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, string> CssValues { get; set; }

        public IRFindsBy() : base() { }
    }
}
