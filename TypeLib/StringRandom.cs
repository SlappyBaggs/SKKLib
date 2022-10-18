using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.TypeLib
{
    public static class StringRandom
    {
        private static Random myRand = new Random();

        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static void SetRandomSeed(int seed) => myRand = new Random(seed);

        public static string RandomString(int num)
        {
            char[] ret = new char[num];
            while (--num >= 0) ret[num] = chars[myRand.Next(chars.Length)];
            return new string(ret);
        }
    }
}
