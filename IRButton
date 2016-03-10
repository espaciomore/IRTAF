using Microsoft.VisualStudio.TestTools.UITesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FASTSelenium.ImageRecognition
{
    public class IRButton : IWebElement, ILocatable
    {
        private IRFindsBy By { get; set; }

        private Point CenterPoint
        {
            get { return (new Rectangle(this.Location, this.Size)).GetCenterPoint(); }
        }

        public IRButton() { }

        public IRButton(IRFindsBy by)
        {
            this.By = by;
            this.Text = by.Text;
            this._attributes = by.Attributes;
            this._cssValues = by.CssValues;
            this.TagName = "BUTTON";
        }

        public void Clear()
        {
            throw new NotImplementedException("Image Recognition Button does not support Clear()");
        }

        public void Click()
        {
            if (!this.Displayed)
                IRHelpers.WaitUntil(this, _this => _this.FindElement(), TimeSpan.FromSeconds(IRConfig.waitTime));
            Mouse.Click(button: System.Windows.Forms.MouseButtons.Left, modifierKeys: System.Windows.Input.ModifierKeys.None, screenCoordinate: this.CenterPoint);
        }

        public void Click(System.Windows.Input.ModifierKeys modifier = System.Windows.Input.ModifierKeys.None, System.Windows.Forms.MouseButtons button = System.Windows.Forms.MouseButtons.Left)
        {
            if (!this.Displayed)
                IRHelpers.WaitUntil(this, _this => _this.FindElement(), TimeSpan.FromSeconds(IRConfig.waitTime));
            Mouse.Click(button: button, modifierKeys: modifier, screenCoordinate: this.CenterPoint);
        }

        public void MoveToElement()
        {
            if (!this.Displayed)
                IRHelpers.WaitUntil(this, _this => _this.FindElement(), TimeSpan.FromSeconds(IRConfig.waitTime));
            Mouse.Move(screenCoordinate: this.CenterPoint);
        }

        public bool Displayed { get; set; }

        public bool Enabled { get; set; }

        private Dictionary<string, string> _attributes;
        public string GetAttribute(string attributeName)
        {
            return _attributes[attributeName];
        }

        private Dictionary<string, string> _cssValues;
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
            var searchSurface = new System.Drawing.Rectangle(new Point(this.By.Left, this.By.Top), new Size(this.By.Right - this.By.Left, this.By.Bottom - this.By.Top));

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
            }

            return this;
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException("Image Recognition Button does not support FindElement with By class");
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException("Image Recognition Button does not support FindElements with By class");
        }

        public ICoordinates Coordinates { get; set; }

        public Point LocationOnScreenOnceScrolledIntoView { get; set; }
    }
}
