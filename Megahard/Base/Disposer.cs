using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard
{
	public class Disposer : IDisposable
	{
		public Disposer(Action dispose)
		{
			dispose_ = dispose;
		}
		Action dispose_;
		readonly Threading.SyncLock locker_ = new Threading.SyncLock("Disposer");
		public void Dispose()
		{
			using (locker_.Lock())
			{
				if (dispose_ != null)
				{
					dispose_();
					dispose_ = null;
				}
			}
		}
	}
}
