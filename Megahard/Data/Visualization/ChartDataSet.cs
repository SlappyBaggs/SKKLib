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
	[TypeConverter(typeof(PropertySorter))]
	[DefaultEvent("DataSetChanged")]
	[DefaultProperty("Name")]
	public class ChartDataSet : ICustomTypeDescriptor, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string s)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(s));
		}

		public ChartDataSet() : this("DataSet", Color.Red) { }
		public ChartDataSet(string n, Color c)
		{
			dataPoints_ = new List<ChartDataPoint>();

			Name = n;

			DataSetVisible = true;
			Color = c;
			DashStyle = DashStyle.Solid;

			PointShape = PointShape.SQUARE;
		}

		[Category("Mega Graph Data Set"), Description("Name of this DataSet"), DefaultValue("DataSet"), PropertyOrder()]
		public string Name
		{
			get { return name_; }
			set
			{
				if (name_ == value)
					return;
				name_ = value;
				OnPropertyChanged("Name");
			}
		}
		private string name_;

		[Category("Mega Graph Data Set"), Description("Draw data points or not"), DefaultValue(true), PropertyOrder()]
		public bool DataSetVisible
		{
			get { return dataSetVisible_; }
			set
			{
				if (dataSetVisible_ == value)
					return;
				dataSetVisible_ = value;
				OnPropertyChanged("DataSetVisible");
			}
		}
		private bool dataSetVisible_;

		[Category("Mega Graph Data Set"), Description("Color this dataset will be rendered on the MegaGraph"), DefaultValue(typeof(Color), "Red"), PropertyOrder()]
		public Color Color
		{
			get { return color_; }
			set
			{
				if (color_ == value)
					return;
				color_ = value;
				ClearPen();
				OnPropertyChanged("Color");
			}
		}
		private Color color_;

		[Category("Mega Graph Data Set"), Description("Width of line drawn between data points"), DefaultValue(1), PropertyOrder()]
		public float LineWidth
		{
			get { return lineWidth_; }
			set
			{
				if (lineWidth_ == value)
					return;
				lineWidth_ = value;
				ClearPen();
				OnPropertyChanged("LineWidth");
			}
		}
		private float lineWidth_ = 1;

		[Category("Mega Graph Data Set"), Description("Style of lines to be drawn between data points"), DefaultValue(DashStyle.Solid), PropertyOrder()]
		public DashStyle DashStyle
		{
			get { return dashStyle_; }
			set
			{
				if (dashStyle_ == value)
					return;
				dashStyle_ = value;
				ClearPen();
				OnPropertyChanged("DashStyle");
			}
		}
		private DashStyle dashStyle_;

		[Category("Mega Graph Data Set"), Description("Draw data points or not"), DefaultValue(true), PropertyOrder()]
		public bool DrawPoints
		{
			get { return drawPoints_; }
			set
			{
				if (drawPoints_ == value)
					return;
				drawPoints_ = value;
				OnPropertyChanged("DrawPoints");
			}
		}
		private bool drawPoints_ = true;

		[Category("Mega Graph Data Set"), Description("Size to draw data points"), DefaultValue(4), PropertyOrder()]
		public float PointSize
		{
			get { return pointSize_; }
			set
			{
				if (pointSize_ == value)
					return;
				pointSize_ = value;
				OnPropertyChanged("PointSize");
			}
		}
		private float pointSize_ = 4;

		[Category("Mega Graph Data Set"), Description("Shape of data points"), DefaultValue(PointShape.SQUARE), PropertyOrder()]
		public PointShape PointShape
		{
			get { return pointShape_; }
			set
			{
				if (pointShape_ == value)
					return;
				pointShape_ = value;
				OnPropertyChanged("PointShape");
			}
		}
		private PointShape pointShape_;

		private List<ChartDataPoint> dataPoints_;
		[Category("Mega Graph Data Set"), Description("List of Data Points"), PropertyOrder(1000)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<ChartDataPoint> DataPoints
		{
			get
			{
				return dataPoints_;
			}
		}

		[Browsable(false)]
		public Pen DataSetPen
		{
			get
			{
				if (dataSetPen_ == null)
				{
					dataSetPen_ = new Pen(Color, (float)LineWidth);
					dataSetPen_.DashStyle = DashStyle;
				}
				return dataSetPen_;
			}
		}
		[NonSerialized]
		private Pen dataSetPen_ = null;

		private void ClearPen() { dataSetPen_ = null; }

		public void ClearPoints()
		{
			DataPoints.Clear();
		}

		public void AddPoint(ChartDataPoint dp)
		{
			dp.PointChanged += new PropertyChangedEventHandler(this.PointChanged);
			DataPoints.Add(dp);
			OnPropertyChanged("Point Added");
		}

		public void AddPoint(double x, double y) { AddPoint(new ChartDataPoint(x, y)); }

		public void PopPointAndShift(double x, double y)
		{
			if (DataPoints.Count > 0)
			{
				DataPoints.RemoveAt(0);
				for (int i = 0; i < DataPoints.Count; ++i)
				{
					DataPoints[i].X -= x;
					DataPoints[i].Y -= y;
				}
			}
		}
		public void PopPoint()
		{
			if (DataPoints.Count > 0)
				DataPoints.RemoveAt(0);
		}

		private void PointChanged(object o, PropertyChangedEventArgs e) 
		{ 
			OnPropertyChanged(e.PropertyName); 
		}

		#region ICustomTypeDescriptor Implementation
		public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }
		public String GetClassName() { return TypeDescriptor.GetClassName(this, true); }
		public String GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }
		public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(this, true); }
		public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }
		public PropertyDescriptor GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }
		public object GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(this, editorBaseType, true); }
		public EventDescriptorCollection GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }
		public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(this, true); }
		public object GetPropertyOwner(PropertyDescriptor pd) { return this; }
		public PropertyDescriptorCollection GetProperties(Attribute[] attr) { return GetProperties(); }
		public PropertyDescriptorCollection GetProperties()
		{
			PropertyDescriptorCollection pdsRet = new PropertyDescriptorCollection(null);
			PropertyDescriptorCollection pdsOld = TypeDescriptor.GetProperties(this, true);
			foreach (PropertyDescriptor pd in pdsOld)
			{
				if (pd.Category == "Mega Graph Data Set")
				{
					if ((pd.Name == "Color") && !DataSetVisible) continue;
					if ((pd.Name == "LineWidth") && !DataSetVisible) continue;
					if ((pd.Name == "DashStyle") && !DataSetVisible) continue;
					if ((pd.Name == "DrawPoints") && !DataSetVisible) continue;
					if ((pd.Name == "PointSize") && !(DataSetVisible && DrawPoints)) continue;
					if ((pd.Name == "PointShape") && !(DataSetVisible && DrawPoints)) continue;
				}
				pdsRet.Add(pd);
			}
			return pdsRet;
		}
		#endregion
	}
}
