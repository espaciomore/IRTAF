using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows.Forms;

namespace FASTSelenium.ImageRecognition
{
    public class IRButton : IWebElement, ILocatable
    {
        #region Private Fields
        private IRFindsBy By { get; set; }

        private System.Windows.Point CenterPoint
        {
            get { return (new Rectangle(this.Location, this.Size)).GetCenterPoint(); }
        }

        private void Enable()
        {
            if (!this.Displayed)
                IRHelpers.WaitUntil(this, _this => _this.FindElement(), TimeSpan.FromSeconds(IRConfig.waitTime));
        }
        
        private Dictionary<string, string> _attributes;
        private Dictionary<string, string> _cssValues;
        private ICoordinates _coordinates;
        private System.Drawing.Point _LocationOnScreenOnceScrolledIntoView;
        #endregion

        public IRButton() { }

        public IRButton(IRFindsBy by)
        {
            this.By = by;
            this.Text = by.Text;
            this._attributes = by.Attributes;
            this._cssValues = by.CssValues;
            this.TagName = "BUTTON";
        }

        public IRButton Offset(int dx, int dy)
        {
            this.By.SetOffset(dx, dy);

            return this;
        }

        public IRButton DelayOnce(int timeout = 1)
        {
            try
            {
                IRHelpers.WaitUntil(this, _this => { while (true) { }; }, TimeSpan.FromSeconds(timeout));
            }
            catch { }

            return this;
        }

        public void Clear()
        {
            throw new NotImplementedException("Image Recognition Button does not support Clear()");
        }

        public void DoubleClick()
        {
            this.Enable();
            IRMouse.DoubleClick(this.CenterPoint);
            this.DelayOnce();
        }

        public void Click()
        {
            this.Enable();
            IRMouse.Click(this.CenterPoint);
            this.DelayOnce();
        }

        public void ContextClick()
        {
            this.Enable();
            IRMouse.Click(this.CenterPoint, button: MouseButtons.Right);
            this.DelayOnce();
        }

        public void Click(ModifierKeys modifier = ModifierKeys.None, MouseButtons button = MouseButtons.Left)
        {
            this.Enable();
            IRMouse.Click(this.CenterPoint, button: button, modifierKeys: modifier);
            this.DelayOnce();
        }

        public void Hover()
        {
            this.Enable();
            IRMouse.Move(screenCoordinates: this.CenterPoint);
            this.DelayOnce();
        }

        public bool Displayed { get; set; }

        public bool Enabled { get; set; }

        public string GetAttribute(string attributeName)
        {
            return _attributes[attributeName];
        }

        public string GetCssValue(string propertyName)
        {
            return _cssValues[propertyName];
        }

        public Point Location { get; set; }

        public bool Selected { get; set; }

        public void SendKeys(string text)
        {
            throw new NotImplementedException("Image Recognition Button does not support SendKeys with String class");
        }

        public Size Size { get; set; }

        public void Submit()
        {
            this.Click();
        }

        public string TagName { get; set; }

        public string Text { get; set; }

        public IWebElement FindElement()
        {
            var offset = this.By.GetOffset();
            var searchSurface = new System.Drawing.Rectangle(
                new Point((int)offset.X + this.By.Left, (int)offset.Y + this.By.Top),
                new Size((this.By.Right - this.By.Left), (this.By.Bottom - this.By.Top))
            );

            if (!File.Exists(IRConfig.MediaPath + this.By.URI))
                throw new Exception("Image URI does not exist");

            var img = System.Drawing.Image.FromFile(IRConfig.MediaPath + this.By.URI);
            img.RotateFlip(this.By.RotateOrFlip);
            var bmp = new System.Drawing.Bitmap(img);

            var _IRButton = bmp.GetRectangle(searchSurface);
            if (_IRButton.HasValue)
            {
                this.Location = _IRButton.Value.Location;
                this.Size = _IRButton.Value.Size;
                this.Displayed = true;
                this.Coordinates = new IRCoordinate(this.Location);
                this.LocationOnScreenOnceScrolledIntoView = this.Location;

                return this;
            }

            throw new Exception("IRButton.FindElement was unable to find element: " + this.By.URI);
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException("Image Recognition Button does not support FindElement with By class");
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException("Image Recognition Button does not support FindElements with By class");
        }

        public ICoordinates Coordinates
        {
            get
            {
                this.Enable();
                return this._coordinates;
            }
            set
            {
                this._coordinates = value;
            }
        }

        public System.Drawing.Point LocationOnScreenOnceScrolledIntoView
        {
            get
            {
                this.Enable();
                return this._LocationOnScreenOnceScrolledIntoView;
            }
            set
            {
                this._LocationOnScreenOnceScrolledIntoView = value;
            }
        }
    }
}
