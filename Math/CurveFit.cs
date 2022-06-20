#define REVERSE_ORDER		// Maintains backwards compatibility

using System.Collections.Generic;
using System.Linq;
using SKKLib.Math.Data;

namespace SKKLib.Math
{
	public static class CurveFit
    {
		public static Polynomial PolynomialRegression(List<Point2<double>> points, int degree)
		{
			double[] xPts = new double[points.Count];
			double[] yPts = new double[points.Count];

			for (int i = 0; i < points.Count; i++)
			{
				xPts[i] = points[i].X;
				yPts[i] = points[i].Y;
			}

			double[] polyCo = MathNet.Numerics.Fit.Polynomial(xPts, yPts, degree);//.Reverse().ToArray();
#if REVERSE_ORDER
			polyCo = polyCo.Reverse().ToArray();
#endif
			return new Polynomial(polyCo);
		}
	}
}
