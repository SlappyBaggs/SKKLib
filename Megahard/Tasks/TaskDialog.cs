using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Tasks
{
	public partial class TaskDialog : KryptonForm
	{
		public TaskDialog()
		{
			InitializeComponent();
		}

		static TaskDialog ShowMessage(string msg, string title)
		{
			var ret = new TaskDialog() { Text = title };
			var tb = new KryptonTextBox() { ReadOnly = true, Multiline = true, Text = msg, Dock = DockStyle.Fill };
			ret.Controls.Add(tb);
			return ret;
		}
	}
}
