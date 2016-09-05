/*
 *  Filename:   IRButton.cs
 *  Author:     Manuel A. Cerda R.
 *  Date:       03-14-2016
 */
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Forms;

namespace FASTSelenium.ImageRecognition
{
    public class IRButton : IWebElement, ILocatable
    {
        #region Private Fields
        private IRFindsBy By { get; set; }

        public System.Windows.Point CenterPoint
        {
            get { return (new System.Windows.Rect(this.Location.Convert(), this.Size.Convert())).GetCenterPoint(); }
        }

        public System.Drawing.Point ClickableCenterPoint
        {
            get { return this.CenterPoint.Convert(); }
        }

        private void Enable()
        {
            if (!this.Enabled)
                IRHelpers.RetryUntil(this, TimeSpan.FromSeconds((double)IRConfig.waitTime));
        }
        
        private Dictionary<string, string> _attributes;
        private Dictionary<string, string> _cssValues;
        private ICoordinates _coordinates;
        private System.Windows.Point _LocationOnScreenOnceScrolledIntoView;
        #endregion

        public IRButton() { }

        public IRButton(IRFindsBy by)
        {
            this.By = by;
            this.Offset(this.By.OffsetLeft, this.By.OffsetTop);
            this.Text = by.Text;
            this._attributes = by.Attributes;
            this._cssValues = by.CssValues;
            this.TagName = "BUTTON";
        }


        public IRButton AtUpperLeft()
        {
            this.By.SearchPlanes.Add(IRSearchPlane.UpperLeft);

            return this;
        }

        public IRButton AtUpperRight()
        {
            this.By.SearchPlanes.Add(IRSearchPlane.UpperRight);

            return this;
        }

        public IRButton AtLowerLeft()
        {
            this.By.SearchPlanes.Add(IRSearchPlane.LowerLeft);

            return this;
        }

        public IRButton AtLowerRight()
        {
            this.By.SearchPlanes.Add(IRSearchPlane.LowerRight);

            return this;
        }

        public IRButton Offset(int dx, int dy, int? dX=null, int? dY=null)
        {
            this.By.SetOffset(dx, dy, dX ?? System.Convert.ToInt32(IRConfig.screenSize.Width), dY ?? System.Convert.ToInt32(IRConfig.screenSize.Height));

            return this;
        }

        public IRButton OverOffset(int dx, int dy, int? dX = null, int? dY = null)
        {
            this.Offset(this.By.OffsetLeft + dx, this.By.OffsetTop + dy, dX, dY);

            return this;
        }

        public IRButton DelayOnce(int timeout = 1)
        {
            IRHelpers.WaitUntil(TimeSpan.FromSeconds((double)timeout));

            return this;
        }

        public bool Visible()
        {
            if (this.Enabled)
                return true;

            try
            {
                IRHelpers.RetryUntil(this, TimeSpan.FromSeconds((double)IRConfig.waitTime), false);
                return true;
            }
            catch
            {
                if (!File.Exists(IRConfig.MediaPath + this.By.URI))
                    throw new Exception("Image URI does not exist");
                return false;
            }
        }

        public void Clear()
        {
            throw new NotImplementedException("Image Recognition Button does not support Clear()");
        }

        public IRButton ContextHighlight()
        {
            var pen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
            var rect = new System.Drawing.Rectangle(this.By.OffsetLeft, this.By.OffsetTop, this.By.Width, this.By.Height);

            IRDisplay.DrawHighlight(pen, rect);

            return this;
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

        public IRButton MultiClick(int i=1)
        {
            for (int j = 0; j < i; j++)
            {
                this.Click();
            }

            return this;
        }

        public void ContextClick()
        {
            this.Enable();
            IRMouse.Click(this.CenterPoint, button: MouseButtons.Right);
            this.DelayOnce(2);
        }

        public void Click(ModifierKeys modifier = ModifierKeys.None, MouseButtons button = MouseButtons.Left)
        {
            this.Enable();
            IRMouse.Click(this.CenterPoint, button: button, modifierKeys: modifier);
            this.DelayOnce();
        }

        public IRButton Hover()
        {
            this.Enable();
            IRMouse.Move(screenCoordinates: this.CenterPoint);
            this.DelayOnce();

            return this;
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
        
        public System.Drawing.Point Location { get; set; }

        public bool Selected { get; set; }

        public void SendKeys(string text)
        {
            throw new NotImplementedException("Image Recognition Button does not support SendKeys with String class");
        }

        public System.Drawing.Size Size { get; set; }

        public void Submit()
        {
            this.Click();
        }

        public string TagName { get; set; }

        public string Text { get; set; }

        public IWebElement FindElement()
        {
            var offset = this.By.GetOffset();

            System.Windows.Rect? searchSurface;
            if(this.By.SearchPlane != null)
            {
                searchSurface = this.By.SearchPlane;
            }
            else 
            {
                searchSurface = new System.Windows.Rect(
                    new System.Windows.Point(Convert.ToInt32(offset.X) + this.By.Left, Convert.ToInt32(offset.Y) + this.By.Top),
                    new System.Windows.Size((this.By.Width - this.By.Left), (this.By.Height - this.By.Top))
                );
            }

            if (!File.Exists(IRConfig.MediaPath + this.By.URI))
                throw new Exception("Image URI does not exist");

            var img = System.Drawing.Image.FromFile(IRConfig.MediaPath + this.By.URI);
                img.RotateFlip(this.By.RotateOrFlip);
            var bmp = new System.Drawing.Bitmap(img);
            var bmpFileName = this.By.URI.Replace('.', '_');

            IRHelpers.SaveInReportDir(bmp, location:"original", name:bmpFileName);

            var _IRButton = bmp.GetRectangle(searchSurface, bmpFileName);
            if (_IRButton.HasValue)
            {
                this.Location = _IRButton.Value.Location.Convert();
                this.Size = _IRButton.Value.Size.Convert();
                this.Displayed = true;
                this.Enabled = true;
                this.Coordinates = new IRCoordinate(this.Location.Convert());
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
                return this._LocationOnScreenOnceScrolledIntoView.Convert();
            }
            set
            {
                this._LocationOnScreenOnceScrolledIntoView = value.Convert();
            }
        }
    }
}
