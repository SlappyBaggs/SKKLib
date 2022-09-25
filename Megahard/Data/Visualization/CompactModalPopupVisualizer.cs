﻿using System;
using ComponentFactory.Krypton.Toolkit;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	class CompactModalPopupVisualizer : IDataVisualizer
	{
		public CompactModalPopupVisualizer(IDataVisualizer visualizer)
		{
			if (visualizer == null)
				throw new ArgumentNullException("visualizer");
			_visualizer = visualizer;
			_visualizer.Committed += (s, a) =>
			{
				var copy = Committed;
				if (copy != null)
					copy(this, a);
			};
			_visualizer.Canceled += (s, a) =>
			{
				var copy = Canceled;
				if (copy != null)
					copy(this, a);
			};
			_visualizer.DataError += (s, a) =>
			{
				var copy = DataError;
				if (copy != null)
					copy(this, a);
			};

			var tb = new KryptonTextBox();
			var bspec = new ButtonSpecAny();
			bspec.Text = "...";
			tb.ButtonSpecs.Add(bspec);
			bspec.Click += delegate { InvokePopupEditor(); };
			_guiCtl = tb;
		}
		
		readonly IDataVisualizer _visualizer;
		readonly Control _guiCtl;
		KryptonForm _form;
		#region IDataVisualizer Members

		public DataObject Data
		{
			get
			{
				return _visualizer.Data;
			}
			set
			{
				_visualizer.Data = value;
				if (value == null)
				{
					_guiCtl.Text = "";
				}
				else
				{
					var val = value.GetValue();
					_guiCtl.Text = val != null ? val.ToString() : "";
				}
			}
		}

		public string GetStringRepresentation()
		{
			return _visualizer.GetStringRepresentation();
		}

		public bool AllowEditing
		{
			get
			{
				return _visualizer.AllowEditing;
			}
			set
			{
				_visualizer.AllowEditing = value;
			}
		}

		public void UpdateDisplay()
		{
			_visualizer.UpdateDisplay();
		}

		public void CommitChanges()
		{
			_visualizer.CommitChanges();
		}

		public void CancelChanges()
		{
			_visualizer.CancelChanges();
		}

		public event EventHandler Canceled;
		public event EventHandler Committed;
		public event EventHandler<DataErrorEventArgs> DataError;

		public UpdateMode UpdateMode
		{
			get
			{
				return _visualizer.UpdateMode;
			}
			set
			{
				_visualizer.UpdateMode = value;
			}
		}

		public CommitMode CommitMode
		{
			get
			{
				return _visualizer.CommitMode;
			}
			set
			{
				_visualizer.CommitMode = value;
			}
		}

		public Control GUIObject
		{
			get { return _guiCtl; }
		}

		public void InvokePopupEditor()
		{
			if(_form == null)
				_form = new Megahard.Design.ModalVisualizerForm(_visualizer);
			_form.ShowDialog(_guiCtl);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_form != null)
			{
				_form.Dispose();
				_form = null;
			}
			_visualizer.Dispose();
		}

		#endregion
	}

}