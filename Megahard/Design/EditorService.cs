﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using Megahard.ExtensionMethods;

namespace Megahard.Design
{
	/// <summary>
	/// The <strong>IWindowsFormsEditorService</strong> that allows you to
	/// drop dialog and UI type editors for a <see cref="GenericValueEditor"/>.
	/// </summary>
	public class EditorService : IWindowsFormsEditorService
	{
		/// <summary>
		/// The control that uses this service.
		/// </summary>
		readonly Control editControl_;
		EditorDropForm dropDownForm_;

		public EditorService(Control editor)
		{
			this.editControl_ = editor;
		}
		
		public void DropDownControl(Control ctl)
		{
			if (dropDownForm_ == null)
				dropDownForm_ = new EditorDropForm();
			dropDownForm_.DoDropDown(ctl, editControl_);
		}

		public void CloseDropDown()
		{
			if(dropDownForm_ != null)
				dropDownForm_.DoCloseDropDown();
		}

		public DialogResult ShowDialog(Form dialog)
		{
			return dialog.ShowDialog(editControl_);
		}
	}
}