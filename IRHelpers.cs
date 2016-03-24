using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FASTSelenium.ImageRecognition
{
    public static class IRHelpers
    {
        public static void InitElements<T>(T pageObject)
        {
            foreach (PropertyInfo property in pageObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(IRButton))
                    property.SetValue(pageObject, new IRButton(GetAttributeFrom<IRFindsBy>(pageObject, property.Name)));                
            }
        }

        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);

            return (T)property.GetCustomAttributes(attrType, false).First();
        }

        public static void RetryUntil(IRButton element, TimeSpan timeout)
        {
            try
            {
                var _startedTime = DateTime.Now.TimeOfDay.TotalMilliseconds - 1;
                var _waitTime = timeout.TotalMilliseconds;
                var findElementException = "None";
                while ((DateTime.Now.TimeOfDay.TotalMilliseconds - _startedTime) < _waitTime)
                {
                    try
                    {
                        element.FindElement();
                        return;
                    }
                    catch (Exception ex)
                    {
                        findElementException = ex.Message;
                    }
                };
                
                throw new Exception(string.Format("Time Interval of {0} second(s) with Exception: {1}", System.Convert.ToString(timeout.TotalSeconds), findElementException));
            }
            catch (Exception ex)
            {
                throw new Exception("IRHelpers.RetryUntil timed out: " + ex.Message);
            }
        }

        public static void WaitUntil(TimeSpan timeout)
        {            
            var _startedTime = DateTime.Now.TimeOfDay.TotalMilliseconds - 1;
            var _waitTime = timeout.TotalMilliseconds;
            while ((DateTime.Now.TimeOfDay.TotalMilliseconds - _startedTime) < _waitTime) { };
        }

        public static void SaveInReportDir(Bitmap b)
        {
            using (var tmpImage = new Bitmap(b))
            {
                if (!Directory.Exists(IRConfig.OutputPath.TrimEnd('\\')))
                    Directory.CreateDirectory(IRConfig.OutputPath.TrimEnd('\\'));

                var filename = "ScreenSample_" + DateTime.UtcNow.ToString("ddMMMyyyy_HHmmss_fffffff") + ".BMP";
                tmpImage.Save(Path.Combine(IRConfig.OutputPath, filename), ImageFormat.Bmp);
            }
        }
    }
}
