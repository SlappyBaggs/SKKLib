// This file is terribly misnamed but I dont feel like messing with the version control problems changing it creates, ++Jeff

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Megahard.ComponentModel;
using System.Threading;

namespace Megahard.Threading
{

	public static class SyncContext
	{
		private static SynchronizationContext GetSyncContext() { return System.ComponentModel.AsyncOperationManager.SynchronizationContext; }

		public static EventHandler<T> CreateDelegate<T>(EventHandler<T> eh) where T : EventArgs
		{
			return new Invoker(eh).Call;
		}

		public static EventHandler CreateDelegate(EventHandler eh) 
		{
			return new Invoker(eh).Call;
		}


		public static Invoker CreateDelegate(Delegate d)
		{
			return new Invoker(d);
		}

		public class Invoker
		{
			private readonly Delegate eh_;
			private readonly SynchronizationContext context_;
			internal Invoker(Delegate eh)
			{
				context_ = System.ComponentModel.AsyncOperationManager.SynchronizationContext;
				eh_ = eh;
			}

			private void docall(params object[] p)
			{
				Exception invokedException = null;
				context_.Send(x => 
					{
						try
						{
							eh_.DynamicInvoke(p);
						}
						catch (Exception e)
						{
							invokedException = e;
						}
					}, null);

				if (invokedException != null)
				{
					throw invokedException;
				}
			}

			public void Call()
			{
				docall(null);
			}

			public void Call(object a1)
			{
				docall(a1);
			}

			public void Call(params object[] p)
			{
				docall(p);
			}

			public void Call(object a1, object a2)
			{
				docall(a1, a2);
			}
		}

	}
}
