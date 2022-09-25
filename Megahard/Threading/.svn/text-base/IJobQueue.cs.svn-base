using System;
namespace Megahard.Threading
{
	public interface IJobQueue : IDisposable
	{
		int MaxThreads { get; }
		string Name { get; }
		
		void Enqueue(Action job);
		void Stop();

		event EventHandler<Megahard.Threading.JobExceptionEventArgs> JobException;
	}

	public interface IPriorityJobQueue : IJobQueue
	{
		void Enqueue(Action job, Priority p);
	}

}
