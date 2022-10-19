using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.TypeLib.Random
{
    public static class RandomStrings
    {
        private static System.Random myRand = new System.Random();

        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static void SetRandomSeed(int seed) => myRand = new System.Random(seed);

        public static string RandomString(int num)
        {
            char[] ret = new char[num];
            while (--num >= 0) ret[num] = chars[myRand.Next(chars.Length)];
            return new string(ret);
        }
    }
}
