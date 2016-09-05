/*
 *  Filename:   IRHelpers.cs
 *  Author:     Manuel A. Cerda R.
 *  Date:       03-14-2016
 */
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
        public static double startedTime;
        public static double expectedTime;

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

        public static void RetryUntil(IRButton element, TimeSpan timeout, bool canRetry = true)
        {
            try
            {
                startedTime = DateTime.Now.TimeOfDay.TotalMilliseconds - 1;
                expectedTime = timeout.TotalMilliseconds;
                var findElementException = "None";
                do {
                    try
                    {
                        element.FindElement();
                        return;
                    }
                    catch (Exception ex)
                    {
                        findElementException = ex.Message;
                    }
                } while (canRetry && IsTimeOver());
                
                throw new Exception(string.Format("Time Interval of {0} second(s) with Exception: {1}", System.Convert.ToString(timeout.TotalSeconds), findElementException));
            }
            catch (Exception ex)
            {
                throw new Exception("IRHelpers.RetryUntil timed out: " + ex.Message);
            }
        }

        public static bool IsTimeOver()
        {
            return (DateTime.Now.TimeOfDay.TotalMilliseconds - startedTime) < expectedTime;
        }

        public static void WaitUntil(TimeSpan timeout)
        {            
            var _startedTime = DateTime.Now.TimeOfDay.TotalMilliseconds - 1;
            var _waitTime = timeout.TotalMilliseconds;
            while ((DateTime.Now.TimeOfDay.TotalMilliseconds - _startedTime) < _waitTime) { };
        }

        public static void SaveInReportDir(Bitmap b, string location = "", string name = "Sample")
        {
            using (var tmpImage = new Bitmap(b))
            {
                var outputPath = IRConfig.OutputPath.TrimEnd('\\');
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                
                var filename = string.Format("{0}_{1}_{2}.{3}", name, DateTime.UtcNow.ToString("ddMMMyyyy_HHmmss_fffffff"), location, "BMP");
                tmpImage.Save(Path.Combine(IRConfig.OutputPath, filename), ImageFormat.Bmp);
            }
        }
    }
}
