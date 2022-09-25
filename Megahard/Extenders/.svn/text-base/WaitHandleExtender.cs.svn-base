using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading
{
	public static class mhWaitHandleExtender
	{
		static bool WouldBlock(this WaitHandle wh)
		{
			return wh.WaitOne(0, false) == false;
		}
	}
}
