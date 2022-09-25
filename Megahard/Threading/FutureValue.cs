using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.Threading
{
	public class FutureException : Exception
	{
		internal FutureException(Exception inner) : this("FutureException", inner) { }
		protected FutureException(string msg, Exception inner) : base(msg, inner) { }

		public override string Message
		{
			get
			{
				var inner = base.InnerException;
				if(inner == null)
					return base.Message;

				var sb = new StringBuilder(base.Message);
				sb.Append(Environment.NewLine).Append(inner.GetType().Name).Append(Environment.NewLine);
				sb.Append(inner.Message);
				return sb.ToString();
			}
		}
	}
	public class FutureTimedOut : FutureException
	{
		internal FutureTimedOut() : base("FutureTimedOut", null) { }
	}
	
	public interface IFutureSetter<ValueType> : IDisposable
	{
		void Set(ValueType val);

		/// <summary>
		/// Equivalent of calling dispose, used for when for whatever reason we cannot set the future value
		/// </summary>
		void Cancel();

		/// <summary>
		/// Causes an exception to be thrown to any thread waiting on the future value
		/// </summary>
		/// <param name="e">The exception to throw</param>
		void ThrowFutureException(Exception e);
	}

	public interface IFutureValue : IValue
	{
		object GetValue(TimeSpan timeOut);

		/// <summary>
		/// Get the value, but only if it can be done without blocking the thread. 
		/// </summary>
		/// <param name="value">the value will be placed here if it is immediately available</param>
		/// <returns>true if the value was retrieved, false otherwise</returns>
		bool GetValue(out object value);
	}

	public class FutureValueSetEventArgs<T> : EventArgs
	{
		internal FutureValueSetEventArgs(T val)
		{
			val_ = val;
		}

		internal FutureValueSetEventArgs(Exception ex)
		{
			ex_ = ex;
		}

		public Exception Exception
		{
			get { return ex_; }
		}
		public T Value 
		{ 
			get 
			{
				if (ex_ != null)
					throw new FutureException(ex_);
				return val_; 
			} 
		}

		readonly T val_;
		readonly Exception ex_;
	}

	public static class FutureValue
	{
		public static FutureValue<T> Create<T>(T val)
		{
			return new FutureValue<T>(val);
		}
	}
	public enum FutureState { Current, Future, Unspecified };
	/// <summary>
	/// Represents a value that will be available sometime in the future
	/// It can be in 2 states:
	///   - Current
	///   - Future
	///   In the current state, all reads of the value will return immediately
	///   while in future state, the read will block until the value is available
	///   
	/// </summary>
	public class FutureValue<ValueType> : IFutureValue
	{
		#region Constructors

		public FutureValue()
		{
			valueSetEventHandler_ = new SynchronizedEventBacking<FutureValueSetEventArgs<ValueType>>(lockOb_);
			state_ = FutureState.Unspecified;
		}

		/// <summary>
		/// Constructs the FutureValue in the current state using the supplied initial value
		/// </summary>
		public FutureValue(ValueType val)
		{
			state_ = FutureState.Current;
			val_ = val;
			valueSetEventHandler_ = new SynchronizedEventBacking<FutureValueSetEventArgs<ValueType>>(lockOb_);
		}

		#endregion

		#region Private Fields

		readonly SyncLock lockOb_ = new SyncLock("FutureValue");
		private ValueType val_;
		private Exception exception_;
		private FutureState state_;
		readonly private SynchronizedEventBacking<FutureValueSetEventArgs<ValueType>> valueSetEventHandler_;

		#endregion

		
		/// <summary>
		/// Get the value if it is current, otherwise wait on it
		/// if a timeout occurs, a timeout exception is thrown
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public virtual ValueType GetValue(TimeSpan timeout)
		{
			using (var key = lockOb_.Lock())
			{
				if (state_ == FutureState.Current)
				{
					if (exception_ != null)
						throw new FutureException(exception_);
					return val_;
				}

				if(!key.Wait(timeout))
					throw new FutureTimedOut();
				if (exception_ != null)
					throw new FutureException(exception_);
				return val_;
			}
		}

		public event EventHandler<FutureValueSetEventArgs<ValueType>> ValueSet
		{
			add { valueSetEventHandler_.SynchronizedEvent += value; }
			remove { valueSetEventHandler_.SynchronizedEvent -= value; }
		}

		public override string ToString()
		{
			try
			{
				return GetValue(TimeSpan.FromMilliseconds(5)).ToString();
			}
			catch(FutureTimedOut)
			{
				return "FutureValue (timedout)";
			}
		}
		/// <summary>
		/// Retrieve the value, waiting if necessary until the end of time
		/// </summary>
		/// <returns></returns>
		public ValueType GetValue()
		{
			return GetValue(TimeSpan.FromMilliseconds(-1));
		}

		/// <summary>
		/// Gets the value if it is available without any blocking and return true
		/// otherwise return false
		/// </summary>
		public virtual bool GetValue(out ValueType value)
		{
			using (lockOb_.Lock())
			{
				if (state_ == FutureState.Current)
				{
					if (exception_ != null)
						throw new FutureException(exception_);
					value = val_;
					return true;
				}
				value = default(ValueType);
				return false;
			}
		}


		public ValueType Value
		{
			get
			{
				return GetValue();
			}
		}

		class FutureSetter : IFutureSetter<ValueType>
		{
			internal FutureSetter(Action<ValueType> a, Action<Exception> cancel)
			{
				set_ = a;
				cancel_ = cancel;
			}
			~FutureSetter()
			{
				Cancel();
			}

			public void Set(ValueType val)
			{
				if (set_ == null)
					throw new NullReferenceException("The FutureValue has already been set via delegate created from BeginSet");
				set_(val);
				set_ = null;
			}

			public void Cancel()
			{
				Cancel(null);
			}

			void Cancel(Exception e)
			{
				if (set_ != null)
				{
					set_ = null;
					cancel_(e);
					cancel_ = null;
				}
				GC.SuppressFinalize(this);
			}

			public void ThrowFutureException(Exception e)
			{
				Cancel(e);
			}
			Action<ValueType> set_;
			Action<Exception> cancel_;

			#region IDisposable Members

			public void Dispose()
			{
				Cancel();
			}

			#endregion
		}
		
		/// <summary>
		/// Begin setting the future value, if it is already being set InvalidOperationException is thrown
		/// </summary>
		/// <returns>A delegate used to set the value when it is available</returns>
		public virtual IFutureSetter<ValueType> BeginSetValue()
		{
			using (lockOb_.Lock())
			{
				if (state_ == FutureState.Future)
					throw new InvalidOperationException("FutureValue already in FutureState when BeginSetValue called");

				state_ = FutureState.Future;
				exception_ = null;
				FutureSetter fs = new FutureSetter(EndSet, CancelSet);
				return fs;
			}
		}

		/// <summary>
		/// Sets the value *right* now, as long as an existing BeginSet is not in progress
		/// </summary>
		/// <param name="val"></param>
		/// <returns>Returns true if the value was set, false otherwise.  False would indicate a BeginSet is outstanding</returns>
		public bool SetValue(ValueType val)
		{
			using (lockOb_.Lock())
			{
				if (state_ == FutureState.Future)
					return false;
				EndSet(val);
				return true;
			}
		}


		/// <summary>
		/// Sets the value of the Future.  Each BeginSet needs a matching EndSet
		/// </summary>
		/// <param name="val"></param>
		void EndSet(ValueType val)
		{
			using(var key = lockOb_.Lock())
			{
				val_ = val;
				state_ = FutureState.Current;
				valueSetEventHandler_.RaiseEvent(this, new FutureValueSetEventArgs<ValueType>(val_));
				key.PulseAll();
			}
		}

		void CancelSet(Exception e)
		{
			using (var key = lockOb_.Lock())
			{
				exception_ = e;
				state_ = FutureState.Current;
				valueSetEventHandler_.RaiseEvent(this, new FutureValueSetEventArgs<ValueType>(exception_));
				key.PulseAll();
			}
		}

		public FutureState State
		{
			get
			{
				using (lockOb_.Lock())
					return state_;
			}
		}


		#region IFutureValue Members

		object IFutureValue.GetValue(TimeSpan timeOut)
		{
			return GetValue(timeOut);
		}

		object IValue.Value
		{
			get { return this.Value; }
		}

		bool IFutureValue.GetValue(out object value)
		{
			ValueType v;
			bool b = GetValue(out v);
			value = v;
			return b;
		}

		#endregion
	}
}