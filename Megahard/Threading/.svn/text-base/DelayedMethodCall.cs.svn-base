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
		/// <summary>
		/// State is not defined, this shouldnt happen really
		/// </summary>
		Unknown,
		/// <summary>
		/// Execution is over and done with
		/// </summary>
		Executed,

		/// <summary>
		/// Currently executing, ie running the method
		/// </summary>
		Executing,

		/// <summary>
		/// Method was canceled, it never ran
		/// </summary>
		Canceled,

		/// <summary>
		/// Waiting to execute the method
		/// </summary>
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


		/// <summary>
		/// Cancels the DelayedMethodCall if TimeTillExecution exceedes argument
		/// </summary>
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

		/// <summary>
		/// If negative, it means it either already executed, is about to execute or it got canceled and never will execute
		/// you need to call other funcs to find more info out about the state of things
		/// </summary>
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
