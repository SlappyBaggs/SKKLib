using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Threading
{
	public class SynchronizedEventBacking<ArgType>  : IDisposable where ArgType : EventArgs
	{
		private EventHandler<ArgType> eh_;
		readonly SyncLock locker_;
		/// <summary>
		/// Provide the object for the SynchronizedEventBacking to lock on
		/// </summary>
		/// <param name="SyncRoot"></param>
		public SynchronizedEventBacking(SyncLock lockOb)
		{
			locker_ = lockOb;
		}

		public SynchronizedEventBacking()
		{
			locker_ = new SyncLock();
		}

		/// <summary>
		/// Is anyone subscribing to this event currently?		
		/// </summary>
		public bool HasListeners
		{
			get
			{
				using(locker_.Lock())
					return eh_ != null;
			}
		}

		public event EventHandler<ArgType> SynchronizedEvent
		{
			add
			{
				using(locker_.Lock())
					eh_ += value;
			}
			remove
			{
				using(locker_.Lock())
					eh_ -= value;
			}
		}
		public void RaiseEvent(object sender, ArgType args)
		{
			RaiseEvent(sender, args, EventExecutor.DefaultSync);
		}
		public void RaiseEvent(object sender, ArgType args, IEventExecutor exec)
		{
			EventHandler<ArgType> copy;
			using(locker_.Lock())
				copy = eh_;
			
			if (copy != null)
				exec.FireEvent(copy, sender, args, null);
		}

		#region IDisposable Members

		public void Dispose()
		{
			using(locker_.Lock())
				eh_ = null;
		}

		#endregion
	}

	public class SynchronizedEventBacking : IDisposable
	{
		private EventHandler eh_;
		private readonly SyncLock lock_;
		/// <summary>
		/// Provide the object for the SynchronizedEventBacking to lock on
		/// </summary>
		/// <param name="SyncRoot"></param>
		public SynchronizedEventBacking(SyncLock lockOb)
		{
			lock_ = lockOb;
		}

		public SynchronizedEventBacking()
		{
			lock_ = new SyncLock("SyncEvent");
		}

		/// <summary>
		/// Is anyone subscribing to this event currently?		
		/// </summary>
		public bool HasListeners
		{
			get
			{
				using (lock_.Lock())
					return eh_ != null;
			}
		}


		public event EventHandler SynchronizedEvent
		{
			add
			{
				using (lock_.Lock())
				{
					eh_ += value;
				}
			}
			remove
			{
				using (lock_.Lock())
				{
					eh_ -= value;
				}
			}
		}
		public void RaiseEvent(object sender, EventArgs args)
		{
			RaiseEvent(sender, args, EventExecutor.DefaultSync);
		}
		public void RaiseEvent(object sender, EventArgs args, IEventExecutor exec)
		{
			EventHandler copy = null;
			using (lock_.Lock())
			{
				copy = eh_;
			}
			if(copy != null)
				exec.FireEvent(copy, sender, args, null);
		}


		#region IDisposable Members

		public void Dispose()
		{
			using (lock_.Lock())
				eh_ = null;
		}

		#endregion
	}
}
