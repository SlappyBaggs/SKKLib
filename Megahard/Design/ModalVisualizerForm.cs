using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Design
{
	partial class ModalVisualizerForm : KryptonForm
	{
		public ModalVisualizerForm()
		{
			InitializeComponent();
		}

		public ModalVisualizerForm(Data.Visualization.IDataVisualizer editor)
		{
			if (editor == null)
				throw new ArgumentNullException("editor", "DataVisualizer cannot be null");
			InitializeComponent();
			_editor = editor;
			editor.GUIObject.Dock = DockStyle.Fill;
			var sz = editor.GUIObject.GetPreferredSize(Size);
			if (sz.IsEmpty)
				sz = editor.GUIObject.Size;
			int wdiff = sz.Width - kryptonSplitContainer1.Panel1.Width;
			int hdiff = sz.Height - kryptonSplitContainer1.Panel1.Height;
			if (wdiff > 0)
				Width = Width + wdiff;
			if (hdiff > 0)
				Height = Height + hdiff;
			kryptonSplitContainer1.Panel1.Controls.Add(_editor.GUIObject);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

		}
		private void cancelButton__Click(object sender, EventArgs e)
		{
			_editor.CancelChanges();
			Close();
		}

		private void okButton__Click(object sender, EventArgs e)
		{
			_editor.CommitChanges();
			Close();
		}
		readonly Data.Visualization.IDataVisualizer _editor;

	}
}