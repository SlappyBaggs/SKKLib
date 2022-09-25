using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	public partial class ExceptionVisualizer : DataVisualizer
	{
		public ExceptionVisualizer()
		{
			InitializeComponent();
			Controls.Remove(dropPanel_);
			Height = hdr_.Height;
		}

		protected override void OnObjectChanged(ObjectChangedEventArgs args)
		{
			if (AutoUpdate && args.PropertyName.Root == "Data")
				UpdateDisplay();
			base.OnObjectChanged(args);
		}

		Exception Exception
		{
			get { return GetDataValue<Exception>(); }
		}

		public override void UpdateDisplay()
		{
			var e = Exception;
			if (e != null)
			{
				hdr_.Values.Description = e.Message;
				msgLong_.Text = e.Message;
				stackTrace_.Text = e.StackTrace;
			}
			else
			{
				hdr_.Values.Description = "";
				msgLong_.Text = "";
				stackTrace_.Text = "";
			}
		}

		private void dropButtonSpec__Click(object sender, EventArgs e)
		{
			dropPanel_.MinimumSize = dropPanel_.Size;
			dropDownControl_.OpenDropDown(dropPanel_, this);
		}


	}
}
