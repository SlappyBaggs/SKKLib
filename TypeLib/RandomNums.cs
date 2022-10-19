using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.TypeLib.Random
{
    public static class RandomNums
    {
        private static System.Random myRand = new System.Random();

        public static void SetRandomSeed(int seed) => myRand = new System.Random(seed);

        public static int RandInt(int min, int max) => myRand.Next(min, max);

        public static double RandDouble(double min, double max) => min + max * myRand.NextDouble();

        public static DateTime RandDateDays(DateTime startDate, int maxDays) => startDate.AddDays(RandInt(0, maxDays));
        
        public static DateTime RandDateYears(DateTime startDate, int maxYears) => RandDateDays(startDate, maxYears * 365);
    }
}
