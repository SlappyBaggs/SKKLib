using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
	public static class mhControlExtender
	{
		public static ResultType AutoInvoke<T, ResultType>(this T ctl, Func<ResultType> code) where T : Control
		{
			if (ctl.InvokeRequired)
			{
				return (ResultType)ctl.BeginInvoke(code);
			}
			else
			{
				return code();
			}
		}

		public static void AutoInvoke(this Control ctl, MethodInvoker code)
		{
			if (ctl.InvokeRequired)
			{
				ctl.BeginInvoke(code);
			}
			else
				code();
		}

		public static void AutoBeginInvoke(this Control ctl, MethodInvoker code)
		{
			if (ctl.InvokeRequired)
				ctl.BeginInvoke(code);
			else
				code();
		}

		public static Rectangle GetScreenBounds(this Control ctl)
		{
			var r = ctl.Bounds;
			if (ctl.Parent != null)
				return ctl.Parent.RectangleToScreen(r);
			return ctl.RectangleToScreen(r);
		}

		public static void BeginInvoke(this Control ctl, Action func)
		{
			ctl.BeginInvoke(func);
		}
	}
}
