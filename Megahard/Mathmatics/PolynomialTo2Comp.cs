using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Mathmatics
{
    public class PolynomialTo2Comp
    {
        public static int Coefficient2Complement(double d, int numBits)
        {
            int ret = (int)(((int)Math.Floor(Math.Abs(d)) << (numBits - 3)) + Math.Abs(d - (int)d) * Math.Pow(2.0, numBits - 3));
            if (d < 0) ret = (~ret + 1) & (int)(Math.Pow(2, numBits) - 1);
            return ret;
        }

        public static double Complement2Coefficient(int i, int numBits)
        {
            return (((i & (3 << (numBits - 3))) >> (numBits - 3)) + ((i & (int)(Math.Pow(2, numBits - 3) - 1)) / Math.Pow(2, numBits - 3))) * ((i < 0 ? -1 : 1));
        }
    }
}














































/*
            int ret = 0;
            bool neg = (d < 0);
            if (neg) d *= -1;

            int f = Convert.ToInt32(Math.Floor(d));
            double b = d - f;

            if (f > 3)
            {
                // We have a problem...
                throw new ArgumentOutOfRangeException("d", "Int portion greater than 3: " + d.ToString("F4"));
            }

            // Build the return value: the front shifted 13 places + the back decimal times 2^13
            int i1 = f << (numBits - 3);
            int i2 = (int)(b * Math.Pow(2.0, numBits - 3));
            ret = i1 + i2;
            //ret = Convert.ToInt32((f << (numBits - 3)) + b * Math.Pow(2, numBits - 3));

            // If we're negative, put the return vlue in 2's complement form...
            if (neg)
            {
                // Invert and add one...
                ret = (~ret + 1) & (int)(Math.Pow(2, numBits) - 1);
            }

            return ret;*/






/*
            double ret = 0;
            int pmo = (int)Math.Pow(2.0, numBits - 1) - 1;

            bool neg = (i >= Math.Pow(2, numBits - 1));

            if (neg)
                i = ~i + 1;

            i = i & pmo;

            int fa = 3 << (numBits - 3);

            int f = (i & fa) >> (numBits - 3);
            int b = i & ((int)Math.Pow(2.0, numBits - 3) - 1);

            ret = f + (b / Math.Pow(2, numBits - 3));
            if (neg) ret *= -1;
            
            return ret;*/
