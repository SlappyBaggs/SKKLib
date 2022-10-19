using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.TypeLib.Random
{
    public static class RandomNames
    {
        private static PersonNameGenerator nameGenerator = new PersonNameGenerator();

        public static string RandomFirstAndLastName() => nameGenerator.GenerateRandomFirstAndLastName();

        public static (string First, string Last) RandomFirstAndLastNameTuple()
        {
            string[] names = nameGenerator.GenerateRandomFirstAndLastName().Split(' ');
            return (names[0], names[1]);
        }
    }
}
