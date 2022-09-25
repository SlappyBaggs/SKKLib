using System;

namespace System.Drawing
{
	public static class mhIconExtender
	{
		public static Bitmap ToBitmap(this Icon icon, int width, int height)
		{
			if (icon == null)
				return null;
			if (icon.Width == width && icon.Height == height)
				return icon.ToBitmap();
			using (var newIcon = new Icon(icon, width, height))
			{
				return newIcon.ToBitmap();
			}
		}
	}
}
