using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Megahard.ComponentModel.deprecated
{
	// I hate to do it because this took me a day or two to code, but this EnhancedEvent idea
	// is pretty terrible.  It works, as designed perfectly.  But grabbing the async context at time of event registration
	// is a terrible usage heuristic, essentually, the AutoInvoker is all we need and it is up the code registering the eventhandler
	// to use the auto autoInvoke
	// I'll keep this around though just in case I want to revist this

	class EnhancedEventAsyncResult<DelegateType> : IAsyncResult
	{
		internal IAsyncResult OriginalAsyncResult { get; private set; }
		internal DelegateType OriginalDelegate { get; private set; }

		internal EnhancedEventAsyncResult(IAsyncResult ar, DelegateType eh)
		{
			OriginalDelegate = eh;
			OriginalAsyncResult = ar;
		}

		#region IAsyncResult Members

		public object AsyncState
		{
			get { return OriginalAsyncResult.AsyncState; }
		}

		public System.Threading.WaitHandle AsyncWaitHandle
		{
			get { return OriginalAsyncResult.AsyncWaitHandle; }
		}

		public bool CompletedSynchronously
		{
			get { return OriginalAsyncResult.CompletedSynchronously; }
		}

		public bool IsCompleted
		{
			get { return OriginalAsyncResult.IsCompleted; }
		}

		#endregion
	}
	public class EnhancedEvent<ArgType> where ArgType : EventArgs
	{
		private EnhancedEvent()
		{
		}
		private readonly object locker_ = new object();

		// key is original event handler, value is the new Async delegate
		private Dictionary<EventHandler<ArgType>, EventHandler<ArgType>> dict_;

		protected void Add(EventHandler<ArgType> value)
		{
			lock (locker_)
			{
				if (dict_ == null)
					dict_ = new Dictionary<EventHandler<ArgType>, EventHandler<ArgType>>();

				EventHandler<ArgType> autoInvoker = Megahard.Threading.SyncContext.CreateDelegate<ArgType>(value);
				dict_.Add(value, autoInvoker);
			}
		}

		protected void Remove(EventHandler<ArgType> value)
		{
			lock (locker_)
			{
				if (dict_ == null)
					return;

				if(dict_.ContainsKey(value))
				{
					dict_.Remove(value);
					
					if (dict_.Count == 0)
						dict_ = null;
				}
			}
		}

		public void Invoke(object sender, ArgType e)
		{
			EventHandler<ArgType>[] copy;
			lock (locker_)
			{
				if (dict_ == null)
					return;
				copy = new EventHandler<ArgType>[dict_.Values.Count];
				dict_.Values.CopyTo(copy, 0);
			}

			foreach (EventHandler<ArgType> d in copy)
				d(sender, e);
		}

		public IAsyncResult BeginInvoke(object sender, ArgType e, AsyncCallback callBack, object userState)
		{
			EventHandler<ArgType> eh = Invoke;
			AsyncCallback wrapCallBack = null;
			EnhancedEventAsyncResult<EventHandler<ArgType>> wrapResult = null;
			if (callBack != null)
				wrapCallBack = ar => callBack(wrapResult);
			IAsyncResult asyncRes = eh.BeginInvoke(sender, e, wrapCallBack, userState);
			wrapResult = new EnhancedEventAsyncResult<EventHandler<ArgType>>(asyncRes, eh);
			return wrapResult;
		}

		public void EndInvoke(IAsyncResult res)
		{
			var ar = (EnhancedEventAsyncResult<EventHandler<ArgType>>)res;
			ar.OriginalDelegate.EndInvoke(ar.OriginalAsyncResult);
		}

		public static EnhancedEvent<ArgType> operator +(EnhancedEvent<ArgType> enh, EventHandler<ArgType> eh)
		{
			if (enh == null)
				enh = new EnhancedEvent<ArgType>();
			enh.Add(eh);
			return enh;
		}
		public static EnhancedEvent<ArgType> operator -(EnhancedEvent<ArgType> enh, EventHandler<ArgType> eh)
		{
			if (enh != null)
				enh.Remove(eh);
			return enh;
		}
	}

	public class EnhancedEvent
	{
		private EnhancedEvent()
		{
		}

		private readonly object locker_ = new object();
		private Dictionary<Delegate, EventHandler> dict_;
		

		private void Add(Delegate value)
		{
			lock (locker_)
			{
				if (dict_ == null)
					dict_ = new Dictionary<Delegate, EventHandler>();

				EventHandler autoInvoker = Megahard.Threading.SyncContext.CreateDelegate(value).Call;
				dict_.Add(value, autoInvoker);
			}
		}

		private void Remove(Delegate value)
		{
			lock (locker_)
			{
				if (dict_ == null)
					return;
				if(dict_.ContainsKey(value))
				{
					dict_.Remove(value);

					if (dict_.Count == 0)
						dict_ = null;
				}
			}
		}

		public IAsyncResult BeginInvoke(object sender, EventArgs e, AsyncCallback callBack, object userState)
		{
			EventHandler eh = Invoke;
			AsyncCallback wrapCallBack = null;
			EnhancedEventAsyncResult<EventHandler> wrapResult = null;
			if(callBack != null)
				wrapCallBack = ar=>callBack(wrapResult);
			IAsyncResult asyncRes = eh.BeginInvoke(sender, e, wrapCallBack, userState);
			wrapResult = new EnhancedEventAsyncResult<EventHandler>(asyncRes, eh);
			return wrapResult;
		}

		public void EndInvoke(IAsyncResult res)
		{
			var ar = (EnhancedEventAsyncResult<EventHandler>)res;
			ar.OriginalDelegate.EndInvoke(ar.OriginalAsyncResult);
		}

		public void Invoke(object sender, EventArgs e)
		{
			EventHandler[] copy;
			lock (locker_)
			{
				if (dict_ == null)
					return;
				copy = new EventHandler[dict_.Values.Count];
				dict_.Values.CopyTo(copy, 0);
			}

			foreach (EventHandler d in copy)
				d(sender, e);
		}

		public static EnhancedEvent operator +(EnhancedEvent e, Delegate d)
		{
			if (e == null)
				e = new EnhancedEvent();
			e.Add(d);
			return e;
		}

		public static EnhancedEvent operator -(EnhancedEvent e, Delegate d)
		{
			if(e != null)
				e.Remove(d);
			return e;
		}
	}
}
