using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard
{
	public class ProgressReporter : IDisposable
	{
		private Action<int> indicateProgress_;
		private Action<string> setStatus_;
		private Action<string> done_;
		private Action<string> cancel_;
		private bool ended_;

		public ProgressReporter(Action<int> indicator, Action<string> setstatus, Action<string> done, Action<string> cancel)
		{
			indicateProgress_ = indicator;
			setStatus_ = setstatus;
			done_ = done;
			cancel_ = cancel;
		}

		public void IndicateProgress()
		{
			indicateProgress_(1);
		}

		public void IndicateProgress(int i)
		{
			indicateProgress_(i);
		}

		public void SetStatus(string s)
		{
			setStatus_(s);
		}

		public void Done(string msg)
		{
			done_(msg);
			end();
		}

		public void Cancel(string msg)
		{
			cancel_(msg);
			end();
		}

		private void end()
		{
			done_ = null;
			cancel_ = null;
			indicateProgress_ = null;
			setStatus_ = null;
			ended_ = true;
		}

		public bool HasEnded { get { return ended_; } }

		#region IDisposable Members

		public void Dispose()
		{
			if (!ended_)
			{
				Done("");
				end();
			}
		}

		#endregion
	}

	public interface  IProgressIndicator
	{
		ProgressReporter StartProgressReportableOperation(int totalSteps, string statusTxt);
		string StatusText { get; set; }
		void ShowStatus(string txt, TimeSpan displayTime, bool clearProgBar);
	}
}
