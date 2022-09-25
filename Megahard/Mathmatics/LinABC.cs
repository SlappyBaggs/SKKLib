using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Mathmatics
{
    /*
    public class LinABC
    {
        private static double OverFlow = 4.0 / 3.0;
        private static double MaxSensor = Math.Pow(2, 15);

        public static Int16[] GetLinABC(double fullScaleFlow, double[] flowArray, double[] sensorArray)
        {
            Int16[] ret = new Int16[3];
            List<Point2<double>> myList = new List<Point2<double>>();

            int c = flowArray.Length;

            // Add our normalized points...
            for (int i = 0; i < c; i++)
                myList.Add(new Point2<double>(sensorArray[i] / MaxSensor, flowArray[i] / (fullScaleFlow * OverFlow)));

            // Do the curve fit...
            //Polynomial p = CurveFit.Create(myList, 3);

            // Convert the coefficients into 2's Complement form...
            //for (int i = 0; i < 3; i++)
            //    ret[i] = PolynomialTo2Comp.Coefficient2Complement(p.Coefficients[i]);

            // Return LinABC...
            return ret;
        }
    }
    */
}
