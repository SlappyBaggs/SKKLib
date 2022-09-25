using System.Threading;
using System.ComponentModel;
using System;
namespace Megahard.Threading
{
	public class JobQueueException : Exception
	{
		internal JobQueueException(string msg, JobQueueState state) : base(msg)
		{
			CurrentState = state;
		}

		public JobQueueState CurrentState { get; private set; }
	}
	
	public class JobExceptionEventArgs : EventArgs
	{
		internal JobExceptionEventArgs(Exception ex)
		{
			Exception = ex;
		}

		public Exception Exception { get; private set; }
	}

	public abstract class JobQueueBase : IDisposable, IJobQueue
	{
		private readonly SynchronizedEventBacking<JobExceptionEventArgs> eh_ = new SynchronizedEventBacking<JobExceptionEventArgs>();
		protected readonly SyncLock lockOb_;
		protected struct Job
		{
			public Action Action { get; set; }
			public bool Abort { get; set; }
		}

		public event EventHandler<JobExceptionEventArgs> JobException
		{
			add { eh_.SynchronizedEvent += value; }
			remove { eh_.SynchronizedEvent -= value; }
		}

		private readonly Thread[] jobThreads_;
		public int MaxThreads { get; private set; }
		public string Name { get; private set; }
		protected JobQueueBase(string name, int numThreads)
		{
			Name = name;
			MaxThreads = numThreads;
			lockOb_ = new SyncLock(name);
			jobThreads_ = new Thread[numThreads];
			for (int i = 0; i < numThreads; ++i)
			{
				jobThreads_[i] = new Thread(ProcessJobQueue) { Name = name + " #" + i.ToString() };
			}
		}

		protected virtual void OnJobException(JobExceptionEventArgs arg)
		{
			eh_.RaiseEvent(this, arg);
		}

		private JobQueueState state_;
		private void Start()
		{
			using (lockOb_.Lock())
			{
				if (state_ != JobQueueState.Undefined)
				{
					throw new JobQueueException("JobQueue can only be started from the undefined state", state_);
				}
				state_ = JobQueueState.Running;
				foreach (Thread thr in jobThreads_)
				{
					thr.Start();
				}
			}
		}

		public void Stop()
		{
			using (lockOb_.Lock())
			{
				if (state_ != JobQueueState.Running)
					return;
				state_ = JobQueueState.Stopping;
				for (int i = 0; i < jobThreads_.Length; ++i)
				{
					Enqueue(new Job { Abort = true });
				}

				for (int i = 0; i < jobThreads_.Length; ++i)
				{
					jobThreads_[i].Join();
				}
				state_ = JobQueueState.Stopped;
			}
			GC.SuppressFinalize(this);
		}

		private readonly Toggle queueStarted_ = new Toggle();
		public void Enqueue(Action job)
		{
			if (queueStarted_.TurnOn() == false)
			{
				Start();
			}
			Enqueue(new Job() { Action = job });
		}
		protected abstract void Enqueue(Job job);
		protected abstract Job Dequeue();

		void ProcessJobQueue()
		{
			while (true)
			{
				try
				{
					var job = Dequeue();
					if (job.Abort)
						return;
					job.Action();
				}
				catch (Exception ex)
				{
					OnJobException(new JobExceptionEventArgs(ex));
				}
			}
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Stop();
		}

		#endregion
	}

	public class JobQueue : JobQueueBase
	{
		private readonly OneWayMessageQueue<Job> queue_ = new OneWayMessageQueue<Job>();
		public JobQueue(string name, int numThreads)
			: base(name, numThreads)
		{
		}

		public JobQueue(string name)
			: base(name, 1)
		{
		}

		protected override void Enqueue(JobQueueBase.Job job)
		{
			queue_.Enqueue(job);
		}

		protected override JobQueueBase.Job Dequeue()
		{
			return queue_.Dequeue();
		}
	}

	public class PriorityJobQueue : JobQueueBase, IPriorityJobQueue
	{
		private readonly OneWayPriorityMessageQueue<Job> queue_ = new OneWayPriorityMessageQueue<Job>();
		public PriorityJobQueue(string name, int numThreads)
			: base(name, numThreads)
		{
		}
		public PriorityJobQueue(string name)
			: base(name, 1)
		{
		}

		protected override void Enqueue(JobQueueBase.Job job)
		{
			queue_.Enqueue(job);
		}

		protected override JobQueueBase.Job Dequeue()
		{
			return queue_.Dequeue();
		}

		public void Enqueue(Action job, Priority p)
		{
			queue_.Enqueue(new Job() { Action = job }, p);
		}
	}

}