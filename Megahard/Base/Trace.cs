using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Debug
{
	public class Trace
	{
		public Trace(string name)
		{
			source_ = new System.Diagnostics.TraceSource(name);
		}

		[System.Diagnostics.Conditional("TRACE")]
		[System.Diagnostics.DebuggerStepThrough]
		public void Info(string msg)
		{
			source_.TraceInformation(msg);
		}

		[System.Diagnostics.Conditional("TRACE")]
		[System.Diagnostics.DebuggerStepThrough]
		public void Info(string msg, params object[] args)
		{
			source_.TraceInformation(msg, args);
		}

		[System.Diagnostics.Conditional("TRACE")]
		[System.Diagnostics.DebuggerStepThrough]
		public void Indent()
		{
			foreach (System.Diagnostics.TraceListener l in source_.Listeners)
			{
				l.IndentLevel += 1;
			}
		}

		[System.Diagnostics.Conditional("TRACE")]
		[System.Diagnostics.DebuggerStepThrough]
		public void UnIndent()
		{
			foreach (System.Diagnostics.TraceListener l in source_.Listeners)
			{
				l.IndentLevel -= 1;
			}
		}

		readonly System.Diagnostics.TraceSource source_;
	}
}
