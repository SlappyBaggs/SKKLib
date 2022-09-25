using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Megahard.ComponentModel;

namespace Megahard.Data.Visualization
{
	[Serializable()]
	[TypeConverter(typeof(PropertySorter))]
	[DefaultEvent("PointChanged")]
	[DefaultProperty("X")]
	public class ChartDataPoint
	{
		public event PropertyChangedEventHandler PointChanged;
		private void OnPointChanged(string s) { if (PointChanged != null) PointChanged(this, new PropertyChangedEventArgs(s)); }

		public ChartDataPoint() { X = Y = 0.0; }
		public ChartDataPoint(int x, int y) { X = (double)x; Y = (double)y; }
		public ChartDataPoint(int x, double y) { X = (double)x; Y = y; }
		public ChartDataPoint(double x, int y) { X = x; Y = (double)y; }
		public ChartDataPoint(double x, double y) { X = x; Y = y; }

		[Category("Mega Graph Data Point"), Description("X value"), DefaultValue(0.0), PropertyOrder()]
		public double X
		{
			get { return x_; }
			set { if (x_ == value) return; x_ = value; OnPointChanged("X Value Changed"); }
		}
		private double x_;

		[Category("Mega Graph Data Point"), Description("Y value"), DefaultValue(0.0), PropertyOrder()]
		public double Y
		{
			get { return y_; }
			set { if (y_ == value) return; y_ = value; OnPointChanged("Y Value Changed"); }
		}
		private double y_;
	}
}
