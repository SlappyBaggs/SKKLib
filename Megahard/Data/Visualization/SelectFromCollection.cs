﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	public partial class SelectFromCollection : Megahard.Data.Visualization.DataVisualizer
	{
		public SelectFromCollection()
		{
			comboBox_ = new KryptonComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
			//currentVal_ = new DataObject(comboBox_, "SelectedItem");
			Controls.Add(comboBox_);
		}

		readonly KryptonComboBox comboBox_;
		//readonly DataObject currentVal_;
		/*
		public override DataObject CurrentValue
		{
			get
			{
				return currentVal_;
			}
		}
		 * */

		protected override void OnObjectChanged(ObjectChangedEventArgs args)
		{
			base.OnObjectChanged(args);
			if (AutoUpdate && args.PropertyName.Root == "Data")
				UpdateDisplay();
		}

		public override void UpdateDisplay()
		{
			var ilist = GetDataValueAs<IList>();
			if (ilist == null)
			{
				comboBox_.DataSource = null;
				comboBox_.SelectedIndex = -1;
				comboBox_.Text = "";
				comboBox_.Enabled = false;
			}
			else
			{
				comboBox_.DataSource = ilist;
				comboBox_.Enabled = true;
			}
		}
	}
}
