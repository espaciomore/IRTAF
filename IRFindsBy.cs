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

        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public Dictionary<string, string> CssValues = new Dictionary<string, string>();
        public Dictionary<string, System.Windows.Point> OffsetValues = new Dictionary<string, System.Windows.Point>();
        

        public IRFindsBy() : base() { }


        protected int GetOffset_X()
        {
            return GetOffset_X(this.GetOffsetKey());
        }

        private int GetOffset_X(string OffsetKey)
        {
            if (!OffsetValues.ContainsKey(OffsetKey))
                throw new Exception("IRFindsBy.Offset_X does not exist for " + OffsetKey);

            return System.Convert.ToInt32(OffsetValues[OffsetKey].X);
        }

        protected int GetOffset_X(int dX, int dY)
        {
            var OffsetKey = this.GetOffsetKey(dX, dY);

            return GetOffset_X(OffsetKey);
        }

        protected int GetOffset_Y()
        {
            return GetOffset_Y(this.GetOffsetKey());
        }

        private int GetOffset_Y(string OffsetKey)
        {
            if (!OffsetValues.ContainsKey(OffsetKey))
                throw new Exception("IRFindsBy.Offset_Y does not exist for " + OffsetKey);

            return System.Convert.ToInt32(OffsetValues[OffsetKey].Y);
        }

        protected int GetOffset_Y(int dX, int dY)
        {
            var OffsetKey = this.GetOffsetKey(dX, dY);

            return GetOffset_Y(OffsetKey);
        }

        public void SetOffset(int dx, int dy, int dX, int dY)
        {
            var offsetKey = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#}x{1:#}", dX, dY);
            OffsetValues.Add(offsetKey, new System.Windows.Point(dx, dy));
        }

        public System.Windows.Point GetOffset()
        {
            return new System.Windows.Point(this.GetOffset_X(), this.GetOffset_Y());
        }

        private System.Windows.Size GetScreenSize()
        {
            return System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Convert();
        }

        private string GetOffsetKey(int? dX=null, int? dY=null)
        {
            var screenSize = GetScreenSize();
            var OffsetKey = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#}x{1:#}", dX ?? 0, dY ?? 0);

            if (!this.OffsetValues.ContainsKey(OffsetKey))
            {
                OffsetKey = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#}x{1:#}", screenSize.Width, screenSize.Height);
            }

            if (!this.OffsetValues.ContainsKey(OffsetKey))
            {
                OffsetKey = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#}x{1:#}", IRConfig.screenSize.Width, IRConfig.screenSize.Height);
            }

            return OffsetKey;
        }
    }
}
