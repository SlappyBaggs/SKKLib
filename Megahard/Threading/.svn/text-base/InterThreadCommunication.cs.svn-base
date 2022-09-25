using System;
using System.Threading;
using System.Collections.Generic;

namespace Megahard.Threading
{
	/// <summary>
	/// A mechanism which allows one thread to send a message to one or more listening threads
	/// This is a lossy transmission, if the messgae is sent while there are no listeners, then
	/// the message is lost.
	/// The sender receives absolutely no feedback as to how many (if any) threads actually received
	/// the message
	/// This class is of limited usefulness actually, check out FutureValue for something more useful
	/// </summary>
	/// <typeparam name="msgtype"></typeparam>
	/// 
	public class OneWayLossyMessageDispatcher<msgtype>
	{
		/// <summary>
		/// Wait for a value from the dispatcher
		/// </summary>
		/// <param name="value"></param>
		/// <param name="timeout"></param>
		/// <returns>true if value was received, false if we timed out</returns>
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

		/// <summary>
		/// Waits for a message with infinite timeout
		/// </summary>
		/// <returns></returns>
		public msgtype WaitForValue()
		{
			msgtype msg;
			WaitForValue(out msg, TimeSpan.FromMilliseconds(-1));
			return msg;
		}


		/// <summary>
		/// Dispatches the message to ALL the threads that are listening
		/// </summary>
		/// <param name="msg"></param>
		public void DispatchMessageALL(msgtype msg)
		{
			lock (lockOb_)
			{
				msg_ = msg;
				Monitor.PulseAll(lockOb_);
			}

		}

		/// <summary>
		/// Sends the message and only dispatches it to ONE thread in the wait queue
		/// </summary>
		/// <param name="msg"></param>
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

	/// <summary>
	/// A message queue for usage by threads, messages can be posted or waited to the queue
	/// The posting thread effectively sends messages to the waiting threads, but receives
	/// no feedback as to when the message is dequeued (if ever) hence the one way part of the name
	/// </summary>
	/// <typeparam name="msgtype"></typeparam>
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
