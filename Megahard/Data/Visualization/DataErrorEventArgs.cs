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
