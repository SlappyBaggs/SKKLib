using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Megahard.Threading
{
	public class Toggle
	{
		int val_;
		public bool TurnOff()
		{
			return Interlocked.Exchange(ref val_, 0) == 1;
		}
		public bool TurnOn()
		{
			return Interlocked.Exchange(ref val_, 1) == 1;
		}

		public bool IsOn { get { return Thread.VolatileRead(ref val_) == 1; } }
	}
}
