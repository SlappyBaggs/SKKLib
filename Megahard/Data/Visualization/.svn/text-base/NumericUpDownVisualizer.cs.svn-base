using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public partial class NumericUpDownVisualizer : Megahard.Data.Visualization.DataVisualizer
	{
		public NumericUpDownVisualizer()
		{
			InitializeComponent();
		}

        static int NumberofDecimalPlaces(string val)
        {
            if (val.IsNullOrEmpty())
                return 0;
            int pos = val.IndexOf('.');
            if (pos == -1)
                return 0;
            return val.Length - (pos + 1);
        }

		int _preferredWidth;
		protected override void OnObjectChanged(ObjectChangedEventArgs args)
		{
			if (args.PropertyName == "Data" && !settingVal_)
			{
				try
				{
					settingVal_ = true;
				
					var newVal = Data != null ? Data.GetValueAs<IConvertible>() : null;
					if (Mathematics.NumericInfo.IsRealNumber(newVal))
						newVal = newVal.ToDecimal(null);
					if (newVal != null)
					{
						numericUpDown_.Enabled = true;
						numericUpDown_.ValueChanged -= numericUpDown__ValueChanged;

						if (Mathematics.NumericInfo.IsRealNumber(newVal))
						{
							numericUpDown_.DecimalPlaces = Math.Max(2, NumberofDecimalPlaces(newVal.ToString()));
							numericUpDown_.Increment = 0.01m;
						}
						else
						{
							numericUpDown_.DecimalPlaces = 0;
						}

						var max = Mathematics.NumericInfo.MaxValue(newVal.GetType()).ToDecimal(null);
                        var min = Mathematics.NumericInfo.MinValue(newVal.GetType()).ToDecimal(null);;
						numericUpDown_.Maximum = Math.Min(max, decimal.MaxValue / 10);
						numericUpDown_.Minimum = Math.Max(min, decimal.MinValue / 10);

						numericUpDown_.Value = newVal.ToDecimal(null);
						
						
						numericUpDown_.ValueChanged += numericUpDown__ValueChanged;
						_preferredWidth = 100;
					}
					else
					{
						numericUpDown_.Value = 0;
						numericUpDown_.Enabled = false;
						_preferredWidth = 0;
					}
				}
				finally
				{
					settingVal_ = false;
				}
			}
			base.OnObjectChanged(args);
		}
		bool settingVal_;
		private void numericUpDown__ValueChanged(object sender, EventArgs e)
		{
			if (UpdateMode == UpdateMode.Automatic && !settingVal_)
			{
				try
				{
					settingVal_ = true;
					Data.SetValue(numericUpDown_.Value);
				}
				finally
				{
					settingVal_ = false;
				}
			}
		}

		protected override void OnCommitted()
		{
			
			Data.SetValue(Convert.ToSingle(numericUpDown_.Value));
			base.OnCommitted();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			return new Size(_preferredWidth, MinimumSize.Height);
		}
	}
}
