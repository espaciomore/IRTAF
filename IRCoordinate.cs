using OpenQA.Selenium.Interactions.Internal;

namespace FASTSelenium.ImageRecognition
{
    public class IRCoordinate : ICoordinates
    {
        public IRCoordinate(System.Windows.Point p)
        {
            this.AuxiliaryLocator = p.Convert();
            this.LocationInDom = p.Convert();
            this.LocationInViewport = p.Convert();
            this.LocationOnScreen = p.Convert();
        }

        public object AuxiliaryLocator { get; set; }

        public System.Drawing.Point LocationInDom { get; set; }

        public System.Drawing.Point LocationInViewport { get; set; }

        public System.Drawing.Point LocationOnScreen { get; set; }
    }
}
