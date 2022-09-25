﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Megahard.ComponentModel;
using ComponentFactory.Krypton.Toolkit;
using System.Drawing.Drawing2D;

namespace Megahard.Data.Visualization
{
	[ToolboxBitmap(typeof(reslocator), @"Megahard.Icons.Chart.ico")]
	public partial class Chart : UserControl
	{
		public Chart()
		{
			// (1) To remove flicker we use double buffering for drawing
			SetStyle(
				//ControlStyles.Opaque | 
				  ControlStyles.AllPaintingInWmPaint |
				  ControlStyles.OptimizedDoubleBuffer |
				  ControlStyles.ResizeRedraw, true);

			InitializeComponent();
			DataSets = new List<ChartDataSet>();

			this.Padding = new System.Windows.Forms.Padding(10);

			MinimumX = 0;
			MaximumX = 100;
			MinimumY = 0;
			MaximumY = 100;

			DrawTicks = true;
			DrawTickValues = true;
			NumTicks = 10;
			TickLength = 5;


			// (2) Cache the current global palette setting
			palette_ = KryptonManager.CurrentGlobalPalette;

			// (3) Hook into palette events
			if (palette_ != null)
				palette_.PalettePaint += OnPalettePaint;

			// (4) We want to be notified whenever the global palette changes
			KryptonManager.GlobalPaletteChanged += OnGlobalPaletteChanged;

			// (1) Create redirection object to the base palette
			paletteRedirect_ = new PaletteRedirect(palette_);

			// (2) Create accessor objects for the back, border and content
			paletteBack_ = new PaletteBackInheritRedirect(paletteRedirect_);
			paletteBorder_ = new PaletteBorderInheritRedirect(paletteRedirect_);
			paletteContent_ = new PaletteContentInheritRedirect(paletteRedirect_);

			Disposed += new EventHandler(Chart_Disposed);
		}

		void Chart_Disposed(object sender, EventArgs e)
		{
				// (10) Unhook from the palette events
				if (palette_ != null)
				{
					palette_.PalettePaint -= OnPalettePaint;
					palette_ = null;
				}

				// (11) Unhook from the static events, otherwise we cannot be garbage collected
				KryptonManager.GlobalPaletteChanged -= OnGlobalPaletteChanged;
		}

		private void OnPalettePaint(object sender, PaletteLayoutEventArgs e)
		{
			Invalidate();
		}
		private void OnGlobalPaletteChanged(object sender, EventArgs e)
		{
			// (5) Unhook events from old palette
			if (palette_ != null)
				palette_.PalettePaint -= OnPalettePaint;

			// (6) Cache the new IPalette that is the global palette
			palette_ = KryptonManager.CurrentGlobalPalette;

			// (7) Hook into events for the new palette
			if (palette_ != null)
				palette_.PalettePaint += OnPalettePaint;

			// (8) Change of palette means we should repaint to show any changes
			Invalidate();
		}

		public PaletteBackStyle BackStyle { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			if (palette_ != null)
			{
				// (3) Get the renderer associated with this palette
				IRenderer renderer = palette_.GetRenderer();

				// (4) Create the rendering context that is passed into all renderer calls
				using (RenderContext renderContext = new RenderContext(this, e.Graphics, e.ClipRectangle, renderer))
				{
					// (5) Set style required when rendering
					paletteBack_.Style = BackStyle;
					//paletteBack_.Style = PaletteBackStyle.ButtonStandalone;
					//paletteBorder_.Style = PaletteBorderStyle.ButtonStandalone;
					paletteBorder_.Style = PaletteBorderStyle.FormMain;
					//paletteContent_.Style = PaletteContentStyle.LabelNormalPanel;

					// (6) ...perform renderer operations...

					// Do we need to draw the background?
					if (paletteBack_.GetBackDraw(PaletteState.Normal) == InheritBool.True)
					{
						// (12) Get the background path to use for clipping the drawing
						using (GraphicsPath path = renderer.RenderStandardBorder.GetBackPath(renderContext,
																														 ClientRectangle,
																														 paletteBorder_,
																														 VisualOrientation.Top,
																														 PaletteState.Normal))
						{
							// Perform drawing of the background clipped to the path
							mementoBack_ = renderer.RenderStandardBack.DrawBack(renderContext,
																										  ClientRectangle,
																										  path,
																										  paletteBack_,
																										  VisualOrientation.Top,
																										  PaletteState.Normal,
																										  mementoBack_);
						}
					}

					if (BorderStyle != BorderStyle.None)
					{
						// (10) Do we need to draw the border?
						if (paletteBorder_.GetBorderDraw(PaletteState.Normal) == InheritBool.True)
						{
							// (11) Draw the border inside the provided rectangle area
							renderer.RenderStandardBorder.DrawBorder(renderContext,
																					   ClientRectangle,
																					   paletteBorder_,
																					   VisualOrientation.Top,
																					   PaletteState.Normal);
						}
					}

				}


			}

			MegaGraph_Paint(this, e);
		}




		private PaletteRedirect paletteRedirect_;
		private PaletteBackInheritRedirect paletteBack_;
		private PaletteBorderInheritRedirect paletteBorder_;
		private PaletteContentInheritRedirect paletteContent_;
		private IDisposable mementoBack_;
		private IPalette palette_;



		public event EventHandler MegaGraphUpdated;
		protected virtual void OnMegaGraphUpdated() 
		{ 
			if (MegaGraphUpdated != null) 
				MegaGraphUpdated(this, new EventArgs()); 
		}

		public override string ToString() { return Name; }


		#region DATA SET
		private int cIndex = 0;
		private Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
		private Color GetColor()
		{
			if (cIndex >= colors.Length) cIndex = 0;
			return colors[cIndex++];
		}

		public ChartDataSet GetNewDataSet(string n) { return GetNewDataSet(n, GetColor()); }
		public ChartDataSet GetNewDataSet(string n, Color c)
		{
			ChartDataSet ds = new ChartDataSet(n, c);
			ds.PropertyChanged += new PropertyChangedEventHandler(DataSetChanged);
			DataSets.Add(ds);
			return ds;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Category("Mega Graph"), Description("Collection of Data Sets"), PropertyOrder(50)]
		public List<ChartDataSet> DataSets { get; set; }

		public ChartDataSet GetDataSet(uint i)
		{
			if (i >= DataSets.Count) throw new ChartException("No DataSet at index " + i.ToString());
			return DataSets[(int)i];
		}
		//private List<MGDataSet> dataSets = new List<MGDataSet>();
		private void DataSetChanged(object o, PropertyChangedEventArgs e) { UpdateGraph(); }
		#endregion

		#region RANGE
		[Category("Mega Graph"), Description("Lowest displayed X Value"), DefaultValue(0.0), PropertyOrder(1)]
		public double MinimumX
		{
			get { return minX_; }
			set { minX_ = value; UpdateGraph(); }
		}
		private double minX_;

		[Category("Mega Graph"), Description("Highest displayed Y Value"), DefaultValue(100.0), PropertyOrder(2)]
		public double MaximumX
		{
			get { return maxX_; }
			set { maxX_ = value; UpdateGraph(); }
		}
		private double maxX_;

		[Category("Mega Graph"), Description("Lowest displayed X Value"), DefaultValue(0.0), PropertyOrder(3)]
		public double MinimumY
		{
			get { return minY_; }
			set { minY_ = value; UpdateGraph(); }
		}
		private double minY_;

		[Category("Mega Graph"), Description("Highest displayed Y Value"), DefaultValue(100.0), PropertyOrder(4)]
		public double MaximumY
		{
			get { return maxY_; }
			set { maxY_ = value; UpdateGraph(); }
		}
		private double maxY_;
		#endregion

		#region TICKS
		[Category("Mega Graph"), Description("Draw ticks marks or not"), DefaultValue(true), PropertyOrder(5)]
		[RefreshProperties(RefreshProperties.All)]
		public bool DrawTicks
		{
			get { return drawTicks_; }
			set { drawTicks_ = value; UpdateGraph(); }
		}
		private bool drawTicks_;

		[Category("Mega Graph"), Description("Draw ticks values"), DefaultValue(true), PropertyOrder(6)]
		[RefreshProperties(RefreshProperties.All)]
		public bool DrawTickValues
		{
			get { return drawTickValues_; }
			set { drawTickValues_ = value; UpdateGraph(); }
		}
		private bool drawTickValues_;

		[Category("Mega Graph"), Description("Number of tick marks to display on the X & Y axis"), DefaultValue((uint)10), PropertyOrder(7)]
		public uint NumTicks
		{
			get { return numTicks_; }
			set { if (value <= 0) throw new ChartException("Number of tick marks must be greater than 0"); numTicks_ = value; UpdateGraph(); }
		}
		private uint numTicks_;

		[Category("Mega Graph"), Description("Length of tick marks"), DefaultValue((uint)5), PropertyOrder(8)]
		public uint TickLength
		{
			get { return tickLength_; }
			set { tickLength_ = value; UpdateGraph(); }
		}
		private uint tickLength_;
		#endregion

		#region PAINTING
		private Pen CurPen;
		private Pen bPen = new Pen(Color.Black);
		private Brush bBrush = new SolidBrush(Color.Black);

		private int hX { get { return ClientSize.Width - Padding.Right - 1; } }
		private int hY { get { return Padding.Top; } }
		private int lX { get { return Padding.Left; } }
		private int lY { get { return ClientSize.Height - Padding.Bottom - 1; } }

		private int XAvail { get { return hX - lX; } }
		private int YAvail { get { return lY - hY; } }

		private double XRange { get { return MaximumX - MinimumX; } }
		private double YRange { get { return MaximumY - MinimumY; } }

		private double facX(double x) { return Padding.Left + XAvail * (x - MinimumX) / XRange; }
		private double facY(double y) { return ClientSize.Height - Padding.Bottom - YAvail * (y - MinimumY) / YRange; }

		public void UpdateGraph() { Invalidate(); OnMegaGraphUpdated(); }

		private void MegaGraph_Paint(object sender, PaintEventArgs e)
		{
			if (MinimumX >= MaximumX) throw new ChartException("The minimum value for X must be less than the maximum value for X");
			if (MinimumY >= MaximumY) throw new ChartException("The minimum value for Y must be less than the maximum value for Y");
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


			if (DrawTicks) DrawTickMarks(e.Graphics);

			// Clip region AFTER we draw the tick marks...
			e.Graphics.Clip = new Region(new Rectangle(Padding.Left, Padding.Top, XAvail + 1, YAvail + 1));

			// Draw Points
			double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
			foreach (ChartDataSet ds in DataSets)
			{
				// Need at least 2 pts...
				if (ds.DataPoints.Count < 2 || !ds.DataSetVisible) continue;
				CurPen = ds.DataSetPen;
				
				x1 = facX(ds.DataPoints[0].X);
				y1 = facY(ds.DataPoints[0].Y);
				if (ds.DrawPoints) DrawPoint(e.Graphics, x1, y1, ds.PointSize, ds.PointShape);

				Point[] pts = new Point[ds.DataPoints.Count + 1];
				
				for (int i = 1; i < ds.DataPoints.Count; i++)
				{
					
					x2 = facX(ds.DataPoints[i].X);
					y2 = facY(ds.DataPoints[i].Y);

					pts[i] = new Point((int)x1, (int)y1);

					// Draw the line!
					//e.Graphics.DrawLine(CurPen, (float)x1, (float)y1, (float)x2, (float)y2);

					if (ds.DrawPoints) DrawPoint(e.Graphics, x2, y2, ds.PointSize, ds.PointShape);

					x1 = x2;
					y1 = y2;
				}
				pts[ds.DataPoints.Count] = new Point((int)x2, (int)y2);
				e.Graphics.DrawCurve(CurPen, pts);
			}
		}

		private double s30 = Math.Sin(30.0 * Math.PI / 180.0);
		private double c30 = Math.Cos(30.0 * Math.PI / 180.0);
		private void DrawPoint(Graphics g, double x, double y, float PointSize, PointShape PointShape)
		{
			switch (PointShape)
			{
				case PointShape.SQUARE:
					g.DrawRectangle(CurPen, (float)(x - PointSize / 2.0), (float)(y - PointSize / 2.0), PointSize, PointSize);
					break;
				case PointShape.TRIANGLE:
					g.DrawLine(CurPen, (float)(x - c30 * PointSize / 2.0), (float)(y + s30 * PointSize / 2.0), (float)(x + c30 * PointSize / 2.0), (float)(y + s30 * PointSize / 2.0));
					g.DrawLine(CurPen, (float)(x + c30 * PointSize / 2.0), (float)(y + s30 * PointSize / 2.0), (float)x, (float)(y - PointSize / 2.0));
					g.DrawLine(CurPen, (float)x, (float)(y - PointSize / 2.0), (float)(x - c30 * PointSize / 2.0), (float)(y + s30 * PointSize / 2.0));
					break;
				case PointShape.CIRCLE:
					g.DrawEllipse(CurPen, (float)(x - PointSize / 2.0), (float)(y - PointSize / 2.0), PointSize, PointSize);
					break;
			}
		}

		private void DrawTickMarks(Graphics g)
		{
			Pen bp = new Pen(ForeColor);
			Brush br = new SolidBrush(ForeColor);

			g.DrawLine(bp, lX, lY, lX, hY);
			g.DrawLine(bp, lX, lY, hX, lY);

			double jX = (double)XAvail / NumTicks;
			double jY = (double)YAvail / NumTicks;
			float x, y;
			string v;
			for (int i = 1; i <= NumTicks; i++)
			{
				// X
				x = (float)(lX + jX * i);
				g.DrawLine(bp, x, lY, x, lY - TickLength);
				if (DrawTickValues)
				{
					v = (MinimumX + i * (MaximumX - MinimumX) / NumTicks).ToString();
					g.DrawString(v, Font, br, x - g.MeasureString(v, Font).Width / 2.0f, lY - TickLength - Font.Height);
				}
				
				// Y
				y = (float)(lY - jY * i);
				g.DrawLine(bp, lX, y, lX + TickLength, y);
				if(DrawTickValues) 
					g.DrawString((MinimumY + i * (MaximumY - MinimumY) / NumTicks).ToString(), Font, br, lX + TickLength, (float)(y - Font.Height / 2.0));
			}
		}
		#endregion

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
				if (pd.Category == "Mega Graph")
				{
					// Handle properties with specific needs first
					if ((pd.Name == "NumTicks") && !DrawTicks) continue;
					if ((pd.Name == "TickLength") && !DrawTicks) continue;
				}
				pdsRet.Add(pd);
			}
			return pdsRet;
		}
		#endregion

	}

	public enum PointShape { SQUARE, TRIANGLE, CIRCLE }

	public class ChartException : System.Exception
	{
		internal ChartException(string m) : base(m) { }
	}

}
