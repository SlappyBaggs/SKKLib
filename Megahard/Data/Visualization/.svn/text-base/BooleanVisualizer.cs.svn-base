using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	[ToolboxItem(true)]
	public partial class BooleanVisualizer : Megahard.Data.Visualization.DataVisualizer
	{
		public BooleanVisualizer()
		{
			InitializeComponent();
		}

		public override void UpdateDisplay()
		{
			try
			{
				settingVal_ = true;
				chkBox_.Checked = GetDataValue<bool>();
			}
			finally
			{
				settingVal_ = false;
			}
		}

		protected override void OnCommitted()
		{
			try
			{
				settingVal_ = true;
				SetDataValue(chkBox_.Checked);
			}
			finally
			{
				settingVal_ = false;
			}			
			base.OnCommitted();
		}

		protected override void OnObjectChanged(ObjectChangedEventArgs args)
		{
			base.OnObjectChanged(args);
			if (args.PropertyName.Root != "Data")
				return;
			if (AutoUpdate && !settingVal_)
			{
				UpdateDisplay();
			}
		}
		bool settingVal_;
		private void chkBox__CheckedChanged(object sender, EventArgs e)
		{
			if (AutoCommit && !settingVal_)
			{
				CommitChanges();
			}
		}

	}
}