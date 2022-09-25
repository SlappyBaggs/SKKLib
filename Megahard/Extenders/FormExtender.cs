using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
	public static class mhFormExtender
	{
		public static void CenterAtMouseCursor(this Form frm)
		{
			frm.CenterAtPoint(Form.MousePosition);
		}

		public static void CenterAtPoint(this Form frm, Point pt)
		{
			Size sz = frm.Size;
			Screen scr = Screen.FromPoint(pt);

			Rectangle rect = frm.DesktopBounds;
			rect.Location = pt;
			rect.Offset(-rect.Width / 2, -rect.Height / 2);

			Rectangle limit = scr.WorkingArea;
			if (rect.Width > limit.Width)
				rect.Width = limit.Width;
			if (rect.Height > limit.Height)
				rect.Height = limit.Height;


			int bottomOffset = Math.Max(scr.Bounds.Height - scr.WorkingArea.Height, 120);

			if (limit.Left > rect.Left)
				rect.X = limit.Left + 1;
			else if (limit.Right < rect.Right)
				rect.X = limit.Right - 1 - rect.Width;

			if ((limit.Bottom - bottomOffset) < rect.Bottom)
				rect.Y = limit.Bottom - bottomOffset - rect.Height;
			else if (limit.Top > rect.Top)
				rect.Y = limit.Top + 1;

			frm.DesktopBounds = rect;
		}

		public static void CenterAtPoint(this Form frm, int x, int y)
		{
			frm.CenterAtPoint(new Point(x, y));
		}
	}
}
