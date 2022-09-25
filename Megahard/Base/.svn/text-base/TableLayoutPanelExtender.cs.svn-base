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
		/// <summary>
		/// Remove a row from the TableLayout panel
		/// </summary>
		/// <returns>Returns a Dictionary of controls that were removed, key is the column the control was in</returns>
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

		/// <summary>
		/// Insert a row in front of the specified row
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="row"></param>
		/// <param name="ctls"></param>
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
