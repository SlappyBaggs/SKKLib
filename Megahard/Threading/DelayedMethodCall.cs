using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Megahard.Threading
{
	public enum DelayedMethodCallState
	{
		Unknown,
		Executed,
		Executing,
		Canceled,
		Delaying,
	}
	public abstract class DelayedMethodCall 
	{
		protected DelayedMethodCall()
		{
			event_ = new ManualResetEvent(false);
			stopWatch_.Reset();
		}

		protected abstract void Exec(object state);
		void CallBack(object state, bool timeout)
		{
			var key = lockOb_.Lock();
			try
			{
				if (State == DelayedMethodCallState.Delaying)
				{
					State = DelayedMethodCallState.Executing;
					Exec(state);
				}
			}
			finally
			{
				if(State == DelayedMethodCallState.Executing)
					State = DelayedMethodCallState.Executed;
				waitHandle_.Unregister(null);
				event_.Close();

				key.Unlock();
			}
		}

		class NoArgCallBack : DelayedMethodCall
		{
			protected override void Exec(object state)
			{
				Func();
				Func = null;
			}
			public Action Func { get; set; }
		}
		class ArgCallBack<T> : DelayedMethodCall
		{
			protected override void Exec(object state)
			{
				Func((T)state);
				Func = null;
			}

			public Action<T> Func { get; set; }
		}

		void Start(TimeSpan delay, object arg)
		{
			using (lockOb_.Lock())
			{
				State = DelayedMethodCallState.Delaying;
				execDelay_ = delay;
				stopWatch_.Start();
				waitHandle_ = System.Threading.ThreadPool.RegisterWaitForSingleObject(event_, CallBack, arg, delay, true);
			}
		}

		public static DelayedMethodCall Call(TimeSpan delay, Action callBack)
		{
			var cb = new NoArgCallBack() { Func = callBack };
			cb.Start(delay, null);
			return cb;
		}

		public static DelayedMethodCall Call<T>(TimeSpan delay, T arg, Action<T> callBack)
		{
			var cb = new ArgCallBack<T>() { Func = callBack };
			cb.Start(delay, arg);
			return cb;
		}

		public enum CancelResult
		{
			Canceled,
			AlreadyCanceled,
			NotCanceled
		}
		public CancelResult Cancel()
		{
			using (lockOb_.Lock())
			{
				if (State == DelayedMethodCallState.Canceled)
					return CancelResult.AlreadyCanceled;
				if (State == DelayedMethodCallState.Delaying)
				{
					State = DelayedMethodCallState.Canceled;
					waitHandle_.Unregister(null);
					event_.Close();
					return CancelResult.Canceled;
				}
				return CancelResult.NotCanceled;
			}
		}
		public CancelResult CancelIf(TimeSpan ts)
		{
			using (lockOb_.Lock())
			{
				if (TimeTillExecution > ts)
				{
					return Cancel();
				}
				return CancelResult.NotCanceled;
			}

		}
		public TimeSpan TimeTillExecution
		{
			get
			{
				return execDelay_ - stopWatch_.Elapsed;
			}
		}

		public DelayedMethodCallState State
		{
			get;
			private set;
		}

		TimeSpan execDelay_;
		readonly Stopwatch stopWatch_ = new Stopwatch();
		protected ManualResetEvent event_;
		const string LockName = "DelayedMethodCall";
		protected readonly SyncLock lockOb_ = new SyncLock("LockName");
		protected RegisteredWaitHandle waitHandle_;
	}
}
