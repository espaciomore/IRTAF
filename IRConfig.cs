/*
 *  Filename:   IRConfig.cs
 *  Author:     Manuel A. Cerda R.
 *  Date:       03-14-2016
 */
namespace FASTSelenium.ImageRecognition
{
    public static class IRConfig
    {
        public static bool canSaveScreenSamples = false;
        
        public static bool saveAllSamples = false;

        public static int waitTime = 60;
        
        public static readonly System.Windows.Size screenSize = new System.Windows.Size(1920, 1080);

        public static string MediaPath { get; set; }

        public static string OutputPath { get; set; }
    }    
}
