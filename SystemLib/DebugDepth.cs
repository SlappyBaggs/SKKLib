using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.SystemLib
{
    public class SKKDebugDepth : IDisposable
    {
        /*
         * name - name of DebugDepth that just started
         * msg - start message for this DebugDepth
         * depth - depth of recursion at DebugDepth's start (not including their effect to the depth)
         */
        public delegate void SKKDebugDepthHandler(string name, string msg, int depth);
        public static event SKKDebugDepthHandler DebugDepthStart = delegate { };
        public static event SKKDebugDepthHandler DebugDepthMsg = delegate { };
        public static event SKKDebugDepthHandler DebugDepthDone = delegate { };

        public static List<SKKDebugDepth> depthList = new List<SKKDebugDepth>();

        public static SKKDebugDepth Get(string name, string msg) => new SKKDebugDepth(name, msg);

        //private string _debugDepthName;
        //private static string _currentDebugDepthName;
        private string _debugMsg;

        private string DebugDepthName { get; set; }
        //{
            //get => _debugDepthName;
            //set => _debugDepthName = _currentDebugDepthName = value;
        //}
        private static int depth = 0;

        internal bool HadKids { get; set; } = false;

        private SKKDebugDepth(string name, string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            depthList.Add(this);
            DebugDepthStart(DebugDepthName = name, _debugMsg = msg, depth++);
        }

        public void Msg(string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            DebugDepthMsg(DebugDepthName, msg, depth);
        }

        public static void Msg(string name, string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            DebugDepthMsg(name, msg, depth);
        }

        public void Dispose()
        {
            if (HadKids)
                DebugDepthDone(DebugDepthName, $"{_debugMsg} DONE", --depth);
            else
                --depth;
            depthList.RemoveAt(depthList.Count - 1);
        }
    }
}
