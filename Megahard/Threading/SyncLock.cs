using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Megahard.Threading
{
	public class SyncLock
	{
		public SyncLock(string name)
		{
			name_ = name ?? string.Empty;
		}

		public SyncLock() : this(null) { }

		readonly object lockOb_ = new object();
		readonly string name_;

		public LockKey Lock()
		{
			Monitor.Enter(lockOb_);
			return new LockKey(lockOb_);
		}

		public LockKey Lock(TimeSpan timeout)
		{
			if (!Monitor.TryEnter(lockOb_, timeout))
				throw new LockTimeoutException(this);
			return new LockKey(lockOb_);
		}

		public string Name
		{
			get { return name_; }
		}

		public override string ToString()
		{
			return string.Format("SyncLock {0}", Name);
		}

		public void Exec(Action<LockKey> action)
		{
			using (LockKey key = Lock())
			{
				action(key);
			}
		}
	}
	public struct LockKey : IDisposable
	{
		internal LockKey(object lockOb)
		{
			lockOb_ = lockOb;
		}

		object lockOb_;
		public void Unlock()
		{
			if (lockOb_ != null)
			{
				Monitor.Exit(lockOb_);
				lockOb_ = null;
			}
		}

		public void Pulse()
		{
			Monitor.Pulse(lockOb_);
		}

		public void PulseAll()
		{
			Monitor.PulseAll(lockOb_);
		}

		public void Wait()
		{
			Monitor.Wait(lockOb_);
		}
		public bool Wait(TimeSpan timeout)
		{
			return Monitor.Wait(lockOb_, timeout);
		}

		public void Dispose()
		{
			Unlock();
		}
	}

	public class LockTimeoutException : Exception
	{
		internal LockTimeoutException(SyncLock lockOb)
		{
			lockOb_ = lockOb;
		}
		readonly SyncLock lockOb_;
	}
}
