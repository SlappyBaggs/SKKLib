using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using System.Windows.Forms;

namespace Megahard.Security
{
	public static class Utility
	{
		public static bool IsProcessElevated
		{
			get
			{
				WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				return pricipal.IsInRole(WindowsBuiltInRole.Administrator);
			}
		}
		public static void ElevateThisProgram()
		{
			var proc = Process.GetCurrentProcess();
			if (RunElevated(proc.MainModule.FileName))
			{
				bool closed = false;
				try
				{
					closed = proc.CloseMainWindow();
				}
				catch
				{
					proc.Kill();
					closed = true;
				}
				if (!closed)
					proc.Kill();
			}
		}
		public static bool RunElevated(string fileName)
		{
			ProcessStartInfo processInfo = new ProcessStartInfo();
			processInfo.Verb = "runas";
			processInfo.FileName = fileName;
			try
			{
				Process.Start(processInfo);
				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}
