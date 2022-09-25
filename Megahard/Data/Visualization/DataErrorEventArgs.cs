using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public class DataErrorEventArgs : EventArgs
	{
		public DataErrorEventArgs(Exception ex)
		{
			propException_ = ex;
		}

		#region Handled Property
		
		bool propHandled_;

		/// <summary>
		/// If set to true, it means the error was handled in some way by some code somewhere
		/// The object firing the event uses this flag to determine if it needs to take some action concerning the error
		/// </summary>
		public bool Handled
		{
			get { return propHandled_; }
			set
			{
				propHandled_ = value;
			}
		}
		#endregion

		#region Exception Exception { get; readonly; }
		readonly Exception propException_;
		public Exception Exception
		{
			get { return propException_; }
		}
		#endregion
	}
}
