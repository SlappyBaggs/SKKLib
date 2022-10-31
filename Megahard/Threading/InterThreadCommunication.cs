using System;
using System.Threading;
using System.Collections.Generic;

namespace Megahard.Threading
{
	public class OneWayLossyMessageDispatcher<msgtype>
	{
		public bool WaitForValue(out msgtype msg, TimeSpan timeout)
		{
			lock (lockOb_)
			{
				bool ret = Monitor.Wait(lockOb_, timeout);
				if (ret == true)
				{
					msg = msg_;
				}
				else // timedout
				{
					msg = default(msgtype);
				}
				return ret;
			}
		}
		public msgtype WaitForValue()
		{
			msgtype msg;
			WaitForValue(out msg, TimeSpan.FromMilliseconds(-1));
			return msg;
		}
		public void DispatchMessageALL(msgtype msg)
		{
			lock (lockOb_)
			{
				msg_ = msg;
				Monitor.PulseAll(lockOb_);
			}

		}
		public void DispatchMessageONE(msgtype msg)
		{
			lock (lockOb_)
			{
				msg_ = msg;
				Monitor.Pulse(lockOb_);
			}
		}

		private readonly object lockOb_ = new object();
		private msgtype msg_;
	}
	public class OneWayMessageQueue<msgtype>
	{
		public OneWayMessageQueue()
		{
			lockOb_ = new object();
		}

		public OneWayMessageQueue(object lockOb)
		{
			lockOb_ = lockOb;
		}
		public bool Dequeue(out msgtype msg, TimeSpan timeout)
		{
			lock (lockOb_)
			{
				if (queue_.Count > 0)
				{
					msg = queue_.Dequeue();
					return true;
				}

				while (Monitor.Wait(lockOb_, timeout))
				{
					// We must do this check because another could have made it into the lock and passed the Count > 0 check before
					// ever making to the wait, so we would wake up to find and empty queue so we must wait again
					// This is not theory, I actually saw this happen in testing ++Jeff
					if (queue_.Count > 0)
					{
						msg = queue_.Dequeue();
						return true;
					}
				}
				msg = default(msgtype);
				return false;
			}
		}

		public msgtype Dequeue()
		{
			msgtype ret;
			Dequeue(out ret, TimeSpan.FromMilliseconds(-1));
			return ret;
		}

		public void Enqueue(msgtype msg)
		{
			lock (lockOb_)
			{
				queue_.Enqueue(msg);
				Monitor.Pulse(lockOb_);
			}
		}

		public void Clear()
		{
			lock (lockOb_)
			{
				queue_.Clear();
			}
		}

		public int Count
		{
			get
			{
				lock (lockOb_)
				{
					return queue_.Count;
				}
			}
		}

		private readonly object lockOb_;
		private readonly Queue<msgtype> queue_ = new Queue<msgtype>();
	}

	public enum Priority { High, Medium, Low };
	public class OneWayPriorityMessageQueue<msgtype>
	{
		public OneWayPriorityMessageQueue()
		{
			lockOb_ = new object();
		}

		public OneWayPriorityMessageQueue(object lockOb)
		{
			lockOb_ = lockOb;
		}
		public bool Dequeue(out msgtype msg, TimeSpan timeout)
		{
			lock (lockOb_)
			{

				foreach (var q in queue_)
				{
					if (q.Count > 0)
					{
						msg = q.Dequeue();
						return true;
					}
				}

				while (Monitor.Wait(lockOb_, timeout))
				{
					foreach (var q in queue_)
					{
						if (q.Count > 0)
						{
							msg = q.Dequeue();
							return true;
						}
					}
				}
				msg = default(msgtype);
				return false;
			}
		}

		public msgtype Dequeue()
		{
			msgtype ret;
			Dequeue(out ret, TimeSpan.FromMilliseconds(-1));
			return ret;
		}

		public void Enqueue(msgtype msg)
		{
			Enqueue(msg, Priority.Medium);
		}

		public void Enqueue(msgtype msg, Priority priority)
		{
			if (!Enum.IsDefined(typeof(Priority), priority))
				throw new ArgumentException("Invalid priority", "priority");

			lock (lockOb_)
			{

				queue_[(int)priority].Enqueue(msg);
				Monitor.Pulse(lockOb_);
			}
		}

		public void Clear()
		{
			lock (lockOb_)
			{
				foreach(var q in queue_)
					q.Clear();
			}
		}

		public int CountHigh
		{
			get
			{
				lock (lockOb_)
				{
					return queue_[(int)Priority.High].Count;
				}
			}
		}

		public int CountMedium
		{
			get
			{
				lock (lockOb_)
				{
					return queue_[(int)Priority.Medium].Count;
				}
			}
		}

		public int CountLow
		{
			get
			{
				lock (lockOb_)
				{
					return queue_[(int)Priority.Low].Count;
				}
			}
		}

		public int CountTotal
		{
			get
			{
				lock (lockOb_)
				{
					int count = 0;
					foreach (var q in queue_)
						count += q.Count;
					return count;
				}
			}
		}



		private readonly object lockOb_;
		private readonly Queue<msgtype>[] queue_ = { new Queue<msgtype>(), new Queue<msgtype>(), new Queue<msgtype>() };

	}

}
