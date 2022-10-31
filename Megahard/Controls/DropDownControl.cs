using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Controls
{
	public partial class DropDownControl : ComponentModel.ComponentBase
	{
		public DropDownControl()
		{
			InitializeComponent();
		}

		public DropDownControl(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				CloseDropDown();
				dropDownForm_ = null;
			}
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		Design.EditorDropForm dropDownForm_;

		public void OpenDropDown(Control ctl, Control refControl)
		{
			if (dropDownForm_ == null)
				dropDownForm_ = new Design.EditorDropForm();
			dropDownForm_.DoDropDown(ctl, refControl);
		}

		public void CloseDropDown()
		{
			if(dropDownForm_ != null)
				dropDownForm_.DoCloseDropDown();
		}
	}
}
