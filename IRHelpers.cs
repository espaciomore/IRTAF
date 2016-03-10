using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

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

        public static void WaitUntil(IRButton element, Action<IRButton> action, TimeSpan timeout)
        {
            var operation = new ParameterizedThreadStart(obj => action((IRButton)obj));
            Thread bigStackThread = new Thread(operation, 1024 * 1024);

            bigStackThread.Start(element);
            if (bigStackThread.Join(timeout) == false)
            {
                throw new Exception("IRHelpers.WaitUntil timed out: " + bigStackThread.ThreadState);
            }
        }

        public static void SaveInReportDir(Bitmap b)
        {
            using (var tmpImage = new Bitmap(b))
            {
                if (!Directory.Exists(IRConfig.OutputPath.TrimEnd('\\')))
                    Directory.CreateDirectory(IRConfig.OutputPath.TrimEnd('\\'));
                tmpImage.Save(IRConfig.OutputPath + "ScreenSample_" + DateTime.UtcNow.ToString("ddMMMyyyy_HHmmss_fffffff") + ".BMP", ImageFormat.Bmp);
            }
        }
    }
}
