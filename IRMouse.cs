using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace FASTSelenium.ImageRecognition
{
    public static class IRMouse
    {
        private const uint MOUSEEVENTF_MOVE = (uint)0x0001;
        private const uint MOUSEEVENTF_ABSOLUTE = (uint)0x8000;
        private const uint MOUSEEVENTF_LEFTDOWN = (uint)0x0002;
        private const uint MOUSEEVENTF_LEFTUP = (uint)0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = (uint)0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = (uint)0x0010;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        internal static extern UIntPtr GetMessageExtraInfo();

        public static void Click(System.Windows.Point screenCoordinates, ModifierKeys modifierKeys = ModifierKeys.None, MouseButtons button = MouseButtons.Left)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    LeftClick(screenCoordinates);
                    break;
                case MouseButtons.Right:
                    RightClick(screenCoordinates);
                    break;
            }
        }

        public static void DoubleClick(System.Windows.Point screenCoordinates)
        {
            Move(screenCoordinates);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)screenCoordinates.X, (uint)screenCoordinates.Y, (uint)0, GetMessageExtraInfo());
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)screenCoordinates.X, (uint)screenCoordinates.Y, (uint)0, GetMessageExtraInfo());
        }

        public static void DragAndDrop(System.Windows.Point screenCoordinates1, System.Windows.Point screenCoordinates2, ModifierKeys modifierKeys = ModifierKeys.None)
        {
            Move(screenCoordinates1);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)screenCoordinates1.X, (uint)screenCoordinates1.Y, (uint)0, GetMessageExtraInfo());
            Move(screenCoordinates2);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)screenCoordinates2.X, (uint)screenCoordinates2.Y, (uint)0, GetMessageExtraInfo());
        }

        public static void Move(System.Windows.Point screenCoordinates)
        {
            SetCursorPos((int)screenCoordinates.X, (int)screenCoordinates.Y);
            Microsoft.VisualStudio.TestTools.UITesting.Mouse.Location = screenCoordinates.ToDrawingPoint();
            Cursor.Position = screenCoordinates.ToDrawingPoint();
            //  MOUSEEVENTF_MOVE did not work as expected.
            mouse_event(MOUSEEVENTF_ABSOLUTE, (uint)screenCoordinates.X, (uint)screenCoordinates.Y, (uint)0, GetMessageExtraInfo());
        }

        private static void LeftClick(System.Windows.Point screenCoordinates)
        {
            Move(screenCoordinates);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)screenCoordinates.X, (uint)screenCoordinates.Y, (uint)0, GetMessageExtraInfo());
        }

        private static void RightClick(System.Windows.Point screenCoordinates)
        {
            Move(screenCoordinates);
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)screenCoordinates.X, (uint)screenCoordinates.Y, (uint)0, GetMessageExtraInfo());
        }

    }
}
