using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace SKKLib.Handlers
{
    public class WindowHandler
    {
        public static Dictionary<string, IntPtr> GetWindowHandles()
        {
            Dictionary<string, IntPtr> windowHandles = new Dictionary<string, IntPtr>();

            foreach (Process window in Process.GetProcesses())
            {
                window.Refresh();

                if (window.MainWindowHandle != IntPtr.Zero)
                {
                    windowHandles[window.ProcessName] = window.MainWindowHandle;
                }
            }
            return windowHandles;
        }

        public static void Focus(string pName)
        {
            if (Process.GetProcessesByName(pName).Count() > 0)
            {
                IntPtr p = Process.GetProcessesByName(pName)[0].MainWindowHandle;
                Win32.User32.SetForegroundWindow(p);
            }
        }
    }
}
