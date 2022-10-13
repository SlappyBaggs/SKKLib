using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.SystemLib
{
    /// <summary>
    /// The SKKDebugDepth layers class
    /// </summary>
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

        /// <summary>
        /// Gets an instance to a new debug layer
        /// </summary>
        /// <param name="name">The name of the debug layer>/param>
        /// <param name="msg">Initial message for debug layer</param>
        /// <returns>A SKKDebugDepth.</returns>
        public static SKKDebugDepth Get(string name, string msg) => new SKKDebugDepth(name, msg);

        /// <summary>
        /// Bool representing if this layer has any layers created inside of it
        /// </summary>
        internal bool HadKids { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the debug depth name.
        /// </summary>
        private string DebugDepthName { get; set; }
        
        // Current depth
        private static int depth = 0;
        
        // Initial message for this layer
        private string _debugMsg;
        
        // Stopwatch to time life of layer
        private readonly Stopwatch watch = new Stopwatch();


        /// <summary>
        /// Constructor for a new debug layer
        /// Private, so can only be called within the Get() function 
        /// In general, 'name' is name of class debug layer is created in, and
        /// 'msg' is name of the method layer is created in, but they can be 
        /// whatever values are helpful/useful
        /// </summary>
        /// <param name="name">The name of the debug layer>/param>
        /// <param name="msg">Initial message for debug layer</param>
        private SKKDebugDepth(string name, string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            depthList.Add(this);
            DebugDepthStart(DebugDepthName = name, _debugMsg = msg, depth++);
            watch.Start();
        }

        /// <summary>
        /// Send a message to DepthDebug without adding a depth layer
        /// Called in an existing debug layer, which already has the name, so no need to pass it in as a parameter
        /// </summary>
        /// <param name="msg">The message to send.</param>
        public void Msg(string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            DebugDepthMsg(DebugDepthName, msg, depth);
        }

        /// <summary>
        /// Send a message to DepthDebug without adding a depth layer
        /// Static function so a previous debug layer is not needed
        /// </summary>
        /// <param name="name">The name of message sender.</param>
        /// <param name="msg">The message to send</param>
        public static void Msg(string name, string msg)
        {
            if (depthList.Count != 0) depthList[depthList.Count - 1].HadKids = true;
            DebugDepthMsg(name, msg, depth);
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            watch.Stop();
            DebugDepthDone(DebugDepthName, $"{_debugMsg} DONE", --depth, HadKids, watch.ElapsedMilliseconds);
            depthList.RemoveAt(depthList.Count - 1);
        }
    }
}
