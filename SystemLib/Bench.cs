using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static SKKLib.Console.SKKConsole;

namespace SKKLib.SystemLib
{
    public class SKKBench : IDisposable
    {
        public static SKKBench Get(string name, bool annoStart = false) => new SKKBench(name, annoStart);
        public static int Threshold = 250;

        private readonly Stopwatch watch = new Stopwatch();
        private readonly string benchName;
        private static int _depthI = 0;
        private static int depthI
        {
            get => _depthI;
            set
            {
                _depthI = value;
                depthS = new StringBuilder(depthVal.Length * depthI).Insert(0, depthVal, depthI).ToString();
            }
        }
        private static string depthS = "";
        private const string depthVal = "  ";

        public SKKBench(string name, bool annoStart = false)
        {
            benchName = name;
            if (annoStart) DBG($"SKKBenchMark: {depthS}{benchName} starting");
            watch.Start();
            depthI++;
        }

        public void Dispose()
        {
            watch.Stop();
            depthI--;
            string warn = (watch.ElapsedMilliseconds < Threshold) ? "" : "!!THRESHOLD WARNING!!";
            DBG($"SKKBenchMark: {warn}{depthS}{benchName} {watch.ElapsedMilliseconds} ms");
        }
    }
}
