using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace System.Windows.Forms
{
	public static class mhListViewExtender
	{
		public static void ResizeColumnsIntelligent(this ListView lv)
		{
			ResizeColumnsIntelligent(lv, 2);
		}
		public static void ResizeColumnsIntelligent(this ListView lv, int pad)
		{
			lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			int[] widths = new int[lv.Columns.Count];
			for (int i = 0; i < lv.Columns.Count; ++i)
				widths[i] = lv.Columns[i].Width;
			lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			int totW = 0;
			for (int i = 0; i < lv.Columns.Count; ++i)
			{
				lv.Columns[i].Width = Math.Max(lv.Columns[i].Width, widths[i]) + (i == (lv.Columns.Count - 1) ? 0 : pad);
				totW += lv.Columns[i].Width;
			}

			int maxWidth = lv.ClientRectangle.Width;
			int last = lv.Columns.Count - 1;
			if(totW > maxWidth && (totW - maxWidth) < pad * lv.Columns.Count)
			{
				int diff = totW - maxWidth;

				lv.Columns[last].Width -= diff;
			}

		}
	}
}
