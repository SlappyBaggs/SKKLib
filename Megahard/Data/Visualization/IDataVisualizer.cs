using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public interface IDataVisualizer : IDisposable
	{
		DataObject Data { get; set; }
		string GetStringRepresentation();
		bool AllowEditing { get; set; }
		void UpdateDisplay();
		void CommitChanges();
		void CancelChanges();

		event EventHandler Canceled;
		event EventHandler Committed;
		event EventHandler<DataErrorEventArgs> DataError;
		UpdateMode UpdateMode { get; set; }
		CommitMode CommitMode { get; set; }
		Control GUIObject { get; }
		void InvokePopupEditor();
	}
	
	public enum UpdateMode { Automatic, Manual };
	public enum CommitMode { Automatic, Manual };
}
