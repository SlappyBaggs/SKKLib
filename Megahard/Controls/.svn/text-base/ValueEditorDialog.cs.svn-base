using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Megahard.ExtensionMethods;
namespace Megahard
{
	public partial class ValueEditorDialog : Form
	{
		public ValueEditorDialog()
		{
			InitializeComponent();
			this.Shown += (s, e) => this.CenterAtMouseCursor();
			okButton_.MouseClick += (s, e) =>
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				};
		}

		int lastY = 0;

		readonly List<Data.Controls.DataBox> genEditors_ = new List<Megahard.Data.Controls.DataBox>();
		public void AddEditItem(string label, object val)
		{
			AddEditItem(label, val, null);
		}
		public void AddEditItem(string label, object val, string property)
		{
			Label lbl = new Label() { Text = label };
			var db = new Data.Controls.DataBox();
			panel_.Controls.Add(lbl);
			panel_.Controls.Add(db);

			db.Left = panel_.Width / 2;
			db.Width = panel_.Width / 2 - 4;
			db.Data = new Megahard.Data.DataObject(val, property);
			lastY += db.Height + 2;
			genEditors_.Add(db);
		}

		public object GetValue(int i)
		{
			return genEditors_[i].Data.GetValue();
		}

		public T GetValue<T>(int i)
		{
			return genEditors_[i].Data.GetValue<T>();
		}

	}
}
