using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
	namespace Megahard
	{
		public class TableLayoutPanelRowControls : Dictionary<int, Control>
		{
			public TableLayoutPanelRowControls(RowStyle rs)
			{
				this.RowStyle = rs;
			}
			public void ForEach(Action<int, Control> a)
			{
				foreach (var kv in this)
				{
					a(kv.Key, kv.Value);
				}
			}

			public RowStyle RowStyle { get; private set; }
		}
	}

	public static class mhTableLayoutPanelExtender
	{
		public static Megahard.TableLayoutPanelRowControls RemoveRow(this TableLayoutPanel panel, int row)
		{
			try
			{
				panel.SuspendLayout();
				Megahard.TableLayoutPanelRowControls rowCtls = new Megahard.TableLayoutPanelRowControls(panel.RowStyles[row]);
				panel.RowStyles.RemoveAt(row);
				foreach (Control ctl in panel.Controls)
				{
					int r = panel.GetRow(ctl);
					if (r == row)
					{
						rowCtls.Add(panel.GetColumn(ctl), ctl);
					}
					if (r > row)
					{
						panel.SetRow(ctl, r - 1);
					}
				}

				foreach (Control ctl in rowCtls.Values)
				{
					panel.Controls.Remove(ctl);
				}

				if (panel.RowCount != 0)
					panel.RowCount -= 1;
				return rowCtls;
			}
			finally
			{
				panel.ResumeLayout();
			}
		}
		public static void InsertRow(this TableLayoutPanel panel, int row, Megahard.TableLayoutPanelRowControls ctls)
		{
			try
			{
				panel.SuspendLayout();

				panel.RowStyles.Insert(row, ctls.RowStyle);
				panel.RowCount += 1;
				foreach (Control ctl in panel.Controls)
				{
					int r = panel.GetRow(ctl);
					if (r >= row)
					{
						panel.SetRow(ctl, r + 1);
					}
				}

				ctls.ForEach((col, ctl) =>
					{
						panel.Controls.Add(ctl);
						panel.SetColumn(ctl, col);
						panel.SetRow(ctl, row);
					}
				);
			}
			finally
			{
				panel.ResumeLayout();
			}
				
		}
	}
}
