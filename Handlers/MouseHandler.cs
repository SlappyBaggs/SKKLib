using System;
using System.Drawing;
using System.Windows.Forms;

namespace SKKLib.Handlers
{
    public static class MouseHandler
    {
        [Flags]
        public enum MouseEventFlags : uint
        {
            MOVE = 0x00000001,
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            XDOWN = 0x00000080,
            XUP = 0x00000100,
            WHEEL = 0x00000800,
            ABSOLUTE = 0x00008000,
        }

        // Use the values of this enum for the 'dwData' parameter
        // to specify an X button when using MouseEventFlags.XDOWN or
        // MouseEventFlags.XUP for the dwFlags parameter.
        public enum MouseEventDataXButtons : uint
        {
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        // mouse_event moves in a coordinate system where
        // (0, 0) is in the upper left corner and
        // (65535,65535) is in the lower right corner.
        private static Point ConvertPoint(Form form_, Point p)
        {
            Rectangle screen_bounds = Screen.GetBounds(form_.PointToScreen(p));
            return new Point(
                p.X * 65535 / screen_bounds.Width,
                p.Y * 65535 / screen_bounds.Height);
        }

        public static void MouseMove(Form form_, int x, int y) => MouseMove(form_, new Point(x, y));
        public static void MouseMove(Form form_, Point p)
        {
            if (form_.InvokeRequired)
            {
                form_.Invoke((Action)delegate { MouseMove(form_, p); });
            }
            else
            {
                Point np = ConvertPoint(form_, p);

                // Move the mouse
                Win32.User32.mouse_event(
                    (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE),
                    (uint)np.X, (uint)np.Y, 0, 0);
            }
        }
        public static void MouseLDown(Form form_, int x, int y) => MouseLDown(form_, new Point(x, y));
        public static void MouseLDown(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.LEFTDOWN));
        public static void MouseLUp(Form form_, int x, int y) => MouseLUp(form_, new Point(x, y));
        public static void MouseLUp(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.LEFTUP));
        public static void MouseLClick(Form form_, int x, int y) => MouseLClick(form_, new Point(x, y));
        public static void MouseLClick(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP));
        
        public static void MouseMDown(Form form_, int x, int y) => MouseMDown(form_, new Point(x, y));
        public static void MouseMDown(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MIDDLEDOWN));
        public static void MouseMUp(Form form_, int x, int y) => MouseMUp(form_, new Point(x, y));
        public static void MouseMUp(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MIDDLEUP));
        public static void MouseMClick(Form form_, int x, int y) => MouseMClick(form_, new Point(x, y));
        public static void MouseMClick(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MIDDLEDOWN | MouseEventFlags.MIDDLEUP));

        public static void MouseRDown(Form form_, int x, int y) => MouseRDown(form_, new Point(x, y));
        public static void MouseRDown(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.RIGHTDOWN));
        public static void MouseRUp(Form form_, int x, int y) => MouseRUp(form_, new Point(x, y));
        public static void MouseRUp(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.RIGHTUP));
        public static void MouseRClick(Form form_, int x, int y) => MouseRClick(form_, new Point(x, y));
        public static void MouseRClick(Form form_, Point p) => MouseClick_(form_, p, (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.RIGHTDOWN | MouseEventFlags.RIGHTUP));

        private static void MouseClick_(Form form_, Point p, uint Flags)
        {
            if (form_.InvokeRequired)
            {
                form_.Invoke((Action)delegate { MouseClick_(form_, p, Flags); });
            }
            else
            {
                Point np = ConvertPoint(form_, p);
                Win32.User32.mouse_event(Flags, (uint)np.X, (uint)np.Y, 0, 0);
            }
        }
    }
}
