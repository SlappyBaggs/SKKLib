using System;
using System.Diagnostics;

namespace SKKLib.SystemLib
{
    public class SKKBench : IDisposable
    {
        /*
         * name - name of BenchMark that just started
         * depth - depth of recursion at bench's start (not including their effect to the depth)
         * annoStart - does bench want it's start announced? (holdover from previous implementation)
         */
        public delegate void SKKBenchStartHandler(string name, int depth, bool annoStart);
        public static event SKKBenchStartHandler BenchStart = delegate { };

        /*
         * name - name of BenchMark that just ended
         * depth - depth of recursion at bench's end (not including their effect to the depth)
         * ms - time in milliseconds of bench's life
         */
        public delegate void SKKBenchDoneHandler(string name, int depth, long ms);
        public static event SKKBenchDoneHandler BenchDone = delegate { };

        public static SKKBench Get(string name, bool annoStart = true) => new SKKBench(name, annoStart);

        private readonly Stopwatch watch = new Stopwatch();
        private readonly string benchName;
        private static int depth =  0;

        private SKKBench(string name, bool annoStart = true)
        {
            benchName = name;
            BenchStart(name, depth, annoStart);
            watch.Start();
            depth++;
        }

        public void Dispose()
        {
            watch.Stop();
            depth--;
            BenchDone(benchName, depth, watch.ElapsedMilliseconds);
        }
    }
}
