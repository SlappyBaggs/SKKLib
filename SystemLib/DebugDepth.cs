﻿using System;
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
        
        // Event handlers
        public delegate void SKKDebugDepthHandler(string name, string msg, int depth);
        public delegate void SKKDebugDepthDoneHandler(string name, string msg, int depth, bool hadKids, double ms);
        
        // Events
        public static event SKKDebugDepthHandler DebugDepthStart = delegate { };
        public static event SKKDebugDepthHandler DebugDepthMsg = delegate { };
        public static event SKKDebugDepthDoneHandler DebugDepthDone = delegate { };

        // Master list of current layers
        public static List<SKKDebugDepth> depthList = new List<SKKDebugDepth>();
        public static SKKDebugDepth Get(string name, string msg) => new SKKDebugDepth(name, msg);
        internal bool HadKids { get; set; } = false;
        private string DebugDepthName { get; set; }
        
        // Current depth
        private static int depth = 0;
        
        // Initial message for this layer
        private string _debugMsg;
        
        // Stopwatch to time life of layer
        private readonly Stopwatch watch = new Stopwatch();
        private SKKDebugDepth(string name, string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            depthList.Add(this);
            DebugDepthStart(DebugDepthName = name, _debugMsg = msg, depth++);
            watch.Start();
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
            watch.Stop();
            DebugDepthDone(DebugDepthName, $"{_debugMsg} DONE", --depth, HadKids, watch.ElapsedMilliseconds);
            depthList.RemoveAt(depthList.Count - 1);
        }
    }
}
