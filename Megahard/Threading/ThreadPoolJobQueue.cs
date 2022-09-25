using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semaphore = System.Threading.Semaphore;
using RegisteredWaitHandleAlias = System.Threading.RegisteredWaitHandle;
using MonitorAlias = System.Threading.Monitor;
using ManualResetEventAlias = System.Threading.ManualResetEvent;
namespace Megahard.Threading
{
	public enum JobQueueState { Undefined, Running, Stopping, Stopped }
	/// <summary>
	/// This is a job queue that is serviced by threads from the System thread ThreadPool
	/// It is a separate class from the regular JobQueue because other than usage semantics, there
	/// is almost no code overlap
	/// </summary>
	public class ThreadPoolJobQueue : IJobQueue, IDisposable
	{
		private readonly SynchronizedEventBacking<JobExceptionEventArgs> eh_ = new SynchronizedEventBacking<JobExceptionEventArgs>();

		private readonly Semaphore sem_;
		private readonly RegisteredWaitHandleAlias regWH_;
		private readonly Queue<Action> jobQ_ = new Queue<Action>();
		readonly SyncLock lockOb_;
		private int activeThreads_;
		private ManualResetEventAlias empty_;

		JobQueueState state_;

		public ThreadPoolJobQueue(string name, int maxActiveJobs)
		{
			state_ = JobQueueState.Running;
			Name = name;
			lockOb_ = new SyncLock(name);
			MaxThreads = maxActiveJobs;
			sem_ = new Semaphore(0, maxActiveJobs);
			regWH_ = System.Threading.ThreadPool.RegisterWaitForSingleObject(sem_, ProcessJob, null, System.Threading.Timeout.Infinite, false);
		}

		public ThreadPoolJobQueue(string name)
			: this(name, 1)
		{
		}

		private void ProcessJob(object arg, bool timedout)
		{
			try
			{
				while (true)
				{
					Action job;
					using (lockOb_.Lock())
					{
						if (jobQ_.Count == 0)
						{
							if (state_ == JobQueueState.Stopping)
								empty_.Set();
							return;
						}
						job = jobQ_.Dequeue();
					}
					ExecuteJob(job);
				}
			}
			finally
			{
				lock (jobQ_)
				{
					--activeThreads_;
				}
			}
		}

		private void ExecuteJob(Action job)
		{
			try
			{
				job();
			}
			catch(Exception e)
			{
				OnJobException(new JobExceptionEventArgs(e));
			}
		}

		public void Enqueue(Action job)
		{
			using (lockOb_.Lock())
			{
				if (state_ != JobQueueState.Running)
					throw new InvalidOperationException("ThreadPoolJobQueue: Cannot enqueue more jobs, pool is stopped");

				jobQ_.Enqueue(job);
				if (activeThreads_ < MaxThreads && activeThreads_ < jobQ_.Count) 
				{
					++activeThreads_;
					sem_.Release();
				}
			}
		}

		protected virtual void OnJobException(JobExceptionEventArgs arg)
		{
			eh_.RaiseEvent(this, arg);
		}


		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Stop();
		}

		#endregion

		#region IJobQueue Members

		public int MaxThreads { get; private set; }

		public string Name { get; private set; }

		public void Stop()
		{
			using(lockOb_.Lock())
			{
				if (jobQ_.Count == 0)
				{
					state_ = JobQueueState.Stopped;
				}
				else
				{
					empty_ = new ManualResetEventAlias(false);
					state_ = JobQueueState.Stopping;
				}
			}
			if(empty_ != null)
				empty_.WaitOne();
			regWH_.Unregister(null);
			sem_.Close();
			state_ = JobQueueState.Stopped;
		}

		public event EventHandler<JobExceptionEventArgs> JobException
		{
			add { eh_.SynchronizedEvent += value; }
			remove { eh_.SynchronizedEvent -= value; }
		}



		#endregion
	}
}
