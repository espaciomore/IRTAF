using OpenQA.Selenium.Interactions.Internal;
using System;
using System.Drawing;

namespace FASTSelenium.ImageRecognition
{
    public class IRCoordinate : ICoordinates
    {
        public IRCoordinate(Point p)
        {
            this.LocationInDom = p;
            this.LocationInViewport = p;
            this.LocationOnScreen = p;
        }

        public object AuxiliaryLocator
        {
            get { throw new NotImplementedException("IRCoordinate does not support AuxiliaryLocator()"); }
        }

        public Point LocationInDom { get; set; }

        public Point LocationInViewport { get; set; }

        public Point LocationOnScreen { get; set; }
    }
}
