using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Megahard.ExtensionMethods;

namespace Megahard.Data.Visualization
{
	[ToolboxBitmap(typeof(reslocator), "Megahard.Icons.Legend.ico")]
	public partial class ChartLegend : UserControl
	{
		public ChartLegend()
		{
			InitializeComponent();
		}

		[Category("Mega Graph Legend")]
		[Description("The Mega Graph that this legend will display information about")]
		//[Editor(typeof(Design.ListCompatibleTypesInContainer), typeof(System.Drawing.Design.UITypeEditor))]
		public Chart Chart
		{ 
			get { return chart_; }
			set
			{
				if (chart_ == value)
					return;
				chart_ = value;
				if (chart_ != null)
					chart_.MegaGraphUpdated += new EventHandler(MegaGraphUpdated);
				RebuildLegend();
			}
		}
		Chart chart_ = null;


		private void DrawLegendItem(object sender, DrawItemEventArgs e)
		{
			if (Chart == null || Chart.DataSets == null || e.Index < 0) return;
			ChartDataSet ds;
			try
			{
				ds = Chart.DataSets[e.Index];
			}
			catch (System.ArgumentOutOfRangeException)
			{
				RebuildLegend();
				return;
			}
			if (!ds.DataSetVisible) return;
			SizeF sz = e.Graphics.MeasureString(ds.Name, Font);
			e.Graphics.DrawString(ds.Name, Font, new SolidBrush(ds.Color), e.Bounds);
			e.Graphics.DrawLine(ds.DataSetPen, sz.Width, (float)(e.Bounds.Top + e.Bounds.Height / 2.0), e.Bounds.Width, (float)(e.Bounds.Top + e.Bounds.Height / 2.0));
		}

		private void MeasureLegendItem(object sender, MeasureItemEventArgs e)
		{
			try
			{
				ChartDataSet ds = Chart.DataSets[e.Index];
				e.ItemHeight = ds.DataSetVisible ? (int)Font.GetHeight() : 0;
			}
			catch (System.Exception)
			{
				e.ItemHeight = 0;
			}
		}

		public void MegaGraphUpdated(object o, EventArgs e)
		{
			this.AutoBeginInvoke(()=> RebuildLegend());
		}

		private void RebuildLegend()
		{
			this.listBox1.Items.Clear();
			if(Chart != null) foreach (ChartDataSet ds in Chart.DataSets) listBox1.Items.Add(1);
			Invalidate();
		}


	}
}
