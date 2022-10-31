using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SynchronizationContextAlias = System.Threading.SynchronizationContext;
using System.ComponentModel;

namespace Megahard
{
	public interface IEventExecutor
	{
		void FireEvent(EventHandler eh, object sender, EventArgs args, Action<Exception> onException);
		void FireEvent<T>(EventHandler<T> eh, object sender, T args, Action<Exception> onException) where T : EventArgs;
		void FireEvent(Delegate d, object sender, EventArgs args, Action<Exception> onException);
	}

	public static class EventExecutor 
	{
		private readonly static IEventExecutor instance_ = new SyncEventExecutor(false);
		private readonly static IEventExecutor async_ = new SyncEventExecutor(true);
		
		public static IEventExecutor DefaultSync { get { return instance_; } }
		public static IEventExecutor DefaultASync { get { return async_; } }

		public static IEventExecutor CreateASync(SynchronizationContextAlias context)
		{
			return new SyncEventExecutor(context, true); 
		}
		public static IEventExecutor CreateSync(SynchronizationContextAlias context)
		{
			return new SyncEventExecutor(context, false);
		}

		static void InternalFireEvent(Action fireEvent, Action<Exception> onException)
		{
			if (onException != null)
			{
				try
				{
					fireEvent();
				}
				catch (Exception e)
				{
					onException(e);
				}
			}
			else
			{
				fireEvent();
			}
		}


		public static IEventExecutor CreateASync(ISynchronizeInvoke invoker) { return new SyncEventExecutor(invoker, true); }
		public static IEventExecutor CreateSync(ISynchronizeInvoke invoker) { return new SyncEventExecutor(invoker, false); }

		private class SyncEventExecutor : IEventExecutor
		{
			readonly Action<Action, Action<Exception>> execEvent_;
			public SyncEventExecutor(SynchronizationContextAlias context, bool async)
			{
				System.Diagnostics.Debug.Assert(context != null, "context param to SyncEventExecutor cannot be null");
				if(async)
					execEvent_ = (fireEvent, onException) => context.Post(notused => InternalFireEvent(fireEvent, onException), null);
				else
					execEvent_ = (fireEvent, onException) => context.Send(notused => InternalFireEvent(fireEvent, onException), null);
			}

			public SyncEventExecutor(ISynchronizeInvoke invoker, bool async)
			{
				execEvent_ = (fireEvent, onException) =>
					{
						if (!invoker.InvokeRequired)
							InternalFireEvent(fireEvent, onException);
						else if (async)
							invoker.BeginInvoke((Action<Action, Action<Exception>>) InternalFireEvent, new object[]{ fireEvent, onException });
						else
							invoker.BeginInvoke((Action<Action, Action<Exception>>)InternalFireEvent, new object[] { fireEvent, onException });
					};
			}

			public SyncEventExecutor(bool async)
			{
				Action<Action, Action<Exception>> exec = (fireEvent, onException) => InternalFireEvent(fireEvent, onException);
				if (async)
				{
					execEvent_ = (fireEvent, onException) => exec.BeginInvoke(fireEvent, onException, res => exec.EndInvoke(res), null);
				}
				else
				{
					execEvent_ = exec;
				}
						
			}

			public void FireEvent(EventHandler eh, object sender, EventArgs args, Action<Exception> onException)
			{
				execEvent_(()=> eh(sender, args), onException);
			}

			public void FireEvent<T>(EventHandler<T> eh, object sender, T args, Action<Exception> onException) where T : EventArgs
			{
				execEvent_(() => eh(sender, args), onException);
			}

			public void FireEvent(Delegate d, object sender, EventArgs args, Action<Exception> onException)
			{
				execEvent_(() => d.DynamicInvoke(sender, args), onException);
			}
		}

	}
}
