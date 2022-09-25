using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Megahard.Threading
{
	/// <summary>
	/// This class provides a reentrant Toggle which can be useful for when you want to check a state and exit if the state is already
	/// on, and if not on turn it on.  So usage would be something like this:
	/// 
	/// Toggle toggle_;
	/// void someMethod()
	/// {
	///		if(toggle_.TurnOn())
	///			return;
	///		... do some code
	///		toggle_.TurnOff();
	///	}
	/// </summary>
	public class Toggle
	{
		int val_;

		/// <summary>
		/// Turns the toggle off, and returns the value it had before this call, all done automically
		/// </summary>
		/// <returns>Value of the state before this call</returns>
		public bool TurnOff()
		{
			return Interlocked.Exchange(ref val_, 0) == 1;
		}

		/// <summary>
		/// Turns the toggle on, and returns the value it had before this call, all done automically
		/// </summary>
		/// <returns>Value of the state before this call</returns>
		public bool TurnOn()
		{
			return Interlocked.Exchange(ref val_, 1) == 1;
		}

		public bool IsOn { get { return Thread.VolatileRead(ref val_) == 1; } }
	}
}
