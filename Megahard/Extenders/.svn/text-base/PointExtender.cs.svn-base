using System;
namespace System.Drawing
{
	public static class mhPointExtender
	{
		public static double Magnitude(this Point p)
		{
			return Math.Sqrt(p.MagnitudeSquared());
		}

		public static double MagnitudeSquared(this Point p)
		{
			return p.X * p.X + p.Y * p.Y;
		}

		public static Point Diff(this Point p1, Point p2)
		{
			p1.X -= p2.X;
			p1.Y -= p2.Y;
			return p1;
		}
	}
}