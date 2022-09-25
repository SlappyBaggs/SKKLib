using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public interface IDataVisualizer : IDisposable
	{

		/// <summary>
		/// This is the data that we are showing a visual representation of
		/// </summary>
		DataObject Data { get; set; }

		/// <summary>
		/// Returns a string representation of the data, as well as possible, all visualizers should try to provide some meaningful way to implement this
		/// </summary>
		/// <returns></returns>
		string GetStringRepresentation();

		/// <summary>
		/// This means the editor will allow editing, not that the data is actually editable, if the data itself is read only then even though
		/// this value is true, the data still should not be able to be changed, default should be true
		/// </summary>
		bool AllowEditing { get; set; }

		/// <summary>
		/// Forces the editor to update from its data
		/// </summary>
		void UpdateDisplay();

		/// <summary>
		/// Forces the visualizer to apply the current value to the DataObject
		/// </summary>
		void CommitChanges();

		/// <summary>
		/// Forces the visualizer to revert back to value stored in its DataObject
		/// </summary>
		void CancelChanges();

		event EventHandler Canceled;
		event EventHandler Committed;

		/// <summary>
		/// This EventHandler will be called whenever an error occurs during the visualizers processing of the Data
		/// </summary>
		event EventHandler<DataErrorEventArgs> DataError;

		/// <summary>
		/// Automatic: external changes to Data will be noticed and this editor will update its display
		/// Manual: external changes to Data are ignore, the display will not be updated until you call RefreshDisplay
		/// </summary>
		UpdateMode UpdateMode { get; set; }

		/// <summary>
		/// Automatic: changes made to Data via this editor happen immediately
		/// Manual: changes made to Data are not applied until you call CommitChanges
		/// </summary>
		CommitMode CommitMode { get; set; }

		/// <summary>
		/// The actual control for this visualizer, in future might could make this more generic so same visualizer could work
		/// in multiple gui systems
		/// </summary>
		Control GUIObject { get; }

		/// <summary>
		/// so totally hack, this is just for a quick fix
		/// </summary>
		void InvokePopupEditor();
	}
	
	public enum UpdateMode { Automatic, Manual };
	public enum CommitMode { Automatic, Manual };
}
