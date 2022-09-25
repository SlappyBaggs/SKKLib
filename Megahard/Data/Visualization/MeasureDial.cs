using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Megahard.ComponentModel;
using Megahard.ExtensionMethods;

namespace Megahard.Data.Visualization
{
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(reslocator), @"Megahard.Icons.Dial.ico")]
	[TypeConverter(typeof(PropertySorter))]
	[DefaultEvent("MeasureDialChanged")]
	public partial class MeasureDial : Control, ICustomTypeDescriptor//, INotifyPropertyChanged
	{
		private Pen borderPen;
		private Pen linePen;
		private Pen tickPen;
		private SolidBrush backBrush;
		private SolidBrush textBrush;
		private SolidBrush labelBrush;
		private SolidBrush valueBrush;

		private double _minVal = 0.0;
		[Category("Measure Dial Values")]
		[Description("The minimum value the dial represents")]
		[DefaultValue(0.0)]
		[PropertyOrder(1)]
		public double MinimumValue
		{
			get { return _minVal; }
			set { _minVal = value; Invalidate(); }
		}
		
		private double _maxVal = 100.0;
		[Category("Measure Dial Values")]
		[Description("The maximum value the dial represents")]
		[DefaultValue(100.0)]
		[PropertyOrder(2)]
		public double MaximumValue
		{
			get { return _maxVal; }
			set { _maxVal = value; Invalidate(); }
		}

		public MeasureDial()
		{
			InitializeComponent();

            SmoothingMode = SmoothingMode.AntiAlias;
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			// Default Values
			Spinnable = false;
			CurrentValue = 0.0;
			ValuePerDial = 100.0;
    	}

        [Category("Appearance")]
        [DefaultValue(SmoothingMode.AntiAlias)]
        public SmoothingMode SmoothingMode
        {
            get;
            set;
        }

		//public event PropertyChangedEventHandler PropertyChanged;
		public event MeasureDialChangedEventHandler MeasureDialChanged;
	
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//  COMMON PROPERTIES

		//////////////
		//  VALUES
		[PropertyOrder(-1)]
		[Category("Measure Dial Values")]
		[Description("The current value the dial displays")]
		[DefaultValue(0.0)]
		public double CurrentValue
		{
			get
			{
				if (DialImage != null)
				{
					if (DialMode == SpinDialMode.Clamped)
						return (realDialAngle - MinimumAngle) / (MaximumAngle - MinimumAngle) * (MaximumValue - MinimumValue) + MinimumValue;
					else
						return realDialAngle * ValuePerDial / 360.0;
				}
				else
				{
					return (realDialAngle - MinimumAngle) / (MaximumAngle - MinimumAngle) * (MaximumValue - MinimumValue) + MinimumValue;
				}
			}

			set
			{
				double ov_ = CurrentValue;
				if (DialImage != null)
				{
					if (DialMode == SpinDialMode.Clamped)
					{
						double d = value;
						if (d > MaximumValue) d = MaximumValue;
						else if (d < MinimumValue) d = MinimumValue;
						DialAngle = (d - MinimumValue) / (MaximumValue - MinimumValue) * (MaximumAngle - MinimumAngle) + MinimumAngle;
					}
					else
						DialAngle = value * 360.0 / ValuePerDial;
				}
				else
				{
					double d = value;
					if (d > MaximumValue) d = MaximumValue;
					else if (d < MinimumValue) d = MinimumValue;
					DialAngle = (d - MinimumValue) / (MaximumValue - MinimumValue) * (MaximumAngle - MinimumAngle) + MinimumAngle;
				}

				if (MeasureDialChanged != null)
					MeasureDialChanged(this, new MeasureDialChangedEventArgs(value, value - ov_));
	
				Invalidate();
			}
		}



		private double _minAngle = -180.0;
		[Category("Measure Dial Values")]
		[Description("The minimum angle this dial can be set to")]
		[DefaultValue(-180.0)]
		[PropertyOrder(3)]
		public double MinimumAngle
		{
			get { return _minAngle; }
			set { _minAngle = value; if (DialAngle < _minAngle) DialAngle = _minAngle; Invalidate(); }
		}

		private double _maxAngle = 180.0;
		[Category("Measure Dial Values")]
		[Description("The maximum angle this dial can be set to")]
		[DefaultValue(180.0)]
		[PropertyOrder(4)]
		public double MaximumAngle
		{
			get { return _maxAngle; }
			set { _maxAngle = value; if (DialAngle > _maxAngle) DialAngle = _maxAngle; Invalidate(); }
		}

		private Image _dialImage;
		[Category("Measure Dial Image")]
		[Description("The image to use for the background and dial")]
		[RefreshProperties(RefreshProperties.All)]
		[PropertyOrder(-10)]
		public Image DialImage
		{
			get { return _dialImage; }
			set { _dialImage = value; ResizeDialImage(); }
		}

		[Category("Measure Dial Behavior")]
		[Description("Is this dial spinnable, or display only")]
		[DefaultValue(false)]
		public bool Spinnable { get; set; }

		private bool snapped_ = false;
		[Category("Measure Dial Behavior")]
		[Description("Is the dial 'snapped'")]
		[DefaultValue(false)]
		public bool Snapped
		{
			get { return snapped_; }
			set { snapped_ = value; Invalidate(); }
		}

		[Category("Measure Dial Values")]
		[Description("Value one revolution of dial represents")]
		[DefaultValue(100.0)]
		public double ValuePerDial { get; set; }

		private SpinDialMode _dialMode = SpinDialMode.Clamped;
		[Category("Measure Dial Behavior")]
		[Description("Clamped - Has limits to turning  FreeForm - No turn limits")]
		[DefaultValue(SpinDialMode.Clamped)]
		[RefreshProperties(RefreshProperties.All)]
		public SpinDialMode DialMode
		{
			get { return _dialMode; }
			set { _dialMode = value; Invalidate(); }
		}

		private bool _drawDesignOutline = true;
		[Category("Measure Dial Behavior")]
		[Description("Draws outlines of dial radius and angle limits in Design Mode")]
		[DefaultValue(true)]
		public bool DrawDesignOutline
		{
			get { return _drawDesignOutline; }
			set { _drawDesignOutline = value; Invalidate(); }
		}

		private double _minSpinDistance = 1.0;
		[Category("Measure Dial Behavior")]
		[Description("Minimum distance mouse must be from center of dial to spin")]
		[DefaultValue(1.0)]
		public double MinimumSpinDistance
		{
			get { return _minSpinDistance; }
			set { _minSpinDistance = value; Invalidate(); }
		}
		private double MinimumSpinDistanceSquared { get { return MinimumSpinDistance * MinimumSpinDistance; } }
		
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//  MEASURE DIAL PROPERTIES
		
		//////////////
		//  COLORS
		private Color _colorBack = Color.White;
		[Category("Measure Dial Colors")]
		[Description("The background color of the dial")]
		[DefaultValue(typeof(Color), "White")]
		public Color ColorBack
		{
			get { return _colorBack; }
			set { _colorBack = value; backBrush = null; Invalidate(); }
		}

		private Color _colorBorder = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The border color of the dial")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorBorder
		{
			get { return _colorBorder; }
			set { _colorBorder = value; borderPen = null; Invalidate(); }
		}

		private Color _colorLine = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the dial line")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorLine
		{
			get { return _colorLine; }
			set { _colorLine = value; linePen = null; Invalidate(); }
		}

		private Color _colorTick = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the value ticks")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorTick
		{
			get { return _colorTick; }
			set { _colorTick = value; tickPen = null; Invalidate(); }
		}

		private Color _colorLabel = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the label, if set")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorLabel
		{
			get { return _colorLabel; }
			set { _colorLabel = value; labelBrush = null; Invalidate(); }
		}

		private Color _colorValue = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the current value, if shown")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorValue
		{
			get { return _colorValue; }
			set { _colorValue = value; valueBrush = null; Invalidate(); }
		}

		public override Color BackColor
        { 
            get { return SystemColors.Control; }
            set { base.BackColor = value; Invalidate(); }
        }
		
        public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; textBrush = null; Invalidate(); } 
		}


		//////////////////
		//  APPEARANCE
		public double _padding = 5.0;
		[Category("Measure Dial Appearance")]
		[Description("The space between the control border and the dial border")]
		[DefaultValue(5.0)]
		public double DialPadding
		{
			get { return _padding; }
			set { _padding = value; Invalidate(); }
		}

		private string _text = string.Empty;
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
		public override string Text
		{ 
			get { return _text; } 
			set { _text = value; Invalidate(); } 
		}

		private double _borderThickness = 5.0;
		[Category("Measure Dial Appearance")]
		[Description("The thickness of the border")]
		[DefaultValue(5.0)]
		public double BorderThickness
		{
			get { return _borderThickness; }
			set { _borderThickness = value; borderPen = null; Invalidate(); }
		}

		private double _lineThickness = 1.0;
		[Category("Measure Dial Appearance")]
		[Description("The thickness of the dial line")]
		[DefaultValue(1.0)]
		public double LineThickness
		{
			get { return _lineThickness; }
			set { _lineThickness = value; linePen = null; Invalidate(); }
		}

		private bool _drawTicks = true;
		[Category("Measure Dial Appearance")]
		[Description("Draw tick lines")]
		[DefaultValue(true)]
		[RefreshProperties(RefreshProperties.All)]
		[PropertyOrder(20)]
		public bool DrawTicks
		{
			get { return _drawTicks; }
			set { _drawTicks = value; Invalidate(); }
		}

		private bool _drawTickValues = true;
		[Category("Measure Dial Appearance")]
		[Description("Draw tick values")]
		[DefaultValue(true)]
		[RefreshProperties(RefreshProperties.All)]
		[PropertyOrder(21)]
		public bool DrawTickValues
		{
			get { return _drawTickValues; }
			set { _drawTickValues = value; Invalidate(); }
		}

		private int _numTicks = 12;
		[Category("Measure Dial Appearance")]
		[Description("The number of divisions shown on the dial")]
		[DefaultValue(12)]
		[PropertyOrder(22)]
		public int NumberTicks
		{
			get { return _numTicks; }
			set { _numTicks = value; Invalidate(); }
		}

		private double _tickThickness = 1.0;
		[Category("Measure Dial Appearance")]
		[Description("The thickness of the tick lines")]
		[DefaultValue(1.0)]
		[PropertyOrder(23)]
		public double TickThickness
		{
			get { return _tickThickness; }
			set { _tickThickness = value; tickPen = null; Invalidate(); }
		}

		private bool _drawNub = true;
		[Category("Measure Dial Appearance")]
		[Description("Draw the center nub")]
		[DefaultValue(true)]
		public bool DrawNub
		{
			get { return _drawNub; }
			set { _drawNub = value; Invalidate(); }
		}

		private bool _showValue = true;
		[Category("Measure Dial Appearance")]
		[Description("Show the current value on the dial")]
		[DefaultValue(true)]
		[RefreshProperties(RefreshProperties.All)]
		[PropertyOrder(10)]
		public bool ShowValue
		{
			get { return _showValue; }
			set { _showValue = value; Invalidate(); }
		}

		private int _numDecimals = 3;
		[Category("Measure Dial Appearance")]
		[Description("Number of decimals to show for current value")]
		[DefaultValue(3)]
		[PropertyOrder(11)]
		public int NumDecimals
		{
			get { return _numDecimals; }
			set { _numDecimals = value; Invalidate(); }
		}

		private string _valueUnit = string.Empty;
		[Category("Measure Dial Appearance")]
		[Description("Units of the current value")]
		[DefaultValue("")]
		[PropertyOrder(12)]
		public string ValueUnit
		{
			get { return _valueUnit; }
			set { _valueUnit = value; Invalidate(); }
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//  SPIN DIAL PROPERTIES

		bool _clipImage = true;
		[Category("Measure Dial Image")]
		[Description("Whether to clip the DialImage to be a circle, or show the whole image")]
		[DefaultValue(true)]
		[RefreshProperties(RefreshProperties.All)]
		public bool ClipDialImage
		{
			get { return _clipImage; }
			set { _clipImage = value; Invalidate(); }
		}

		private int _dialRadius = 50;
		private int DialRadiusSquared { get { return DialRadius * DialRadius; } }
		[Category("Measure Dial Image")]
		[Description("The radius of the dial")]
		[DefaultValue(50)]
		public int DialRadius
		{
			get { return (DialImage == null) ? (ClientSize.Width / 2 - (int)DialPadding - (int)BorderThickness) : _dialRadius; }
			set { _dialRadius = value; ResizeDialImage(); }
		}










		///////////////////////
		//  LOCAL VARIABLES
		private double dialAngle_;
		private double DialAngle
		{
			get { return dialAngle_; }
			set { dialAngle_ = value; RecalcRealAngle(); }
		}
		private double realDialAngle;
		private Point PointCenter { get { return new Point(ClientSize.Width / 2, ClientSize.Height / 2); } }
		private Point RefPoint;
		private bool capturing = false;
		private TextureBrush _texBrush;
		private TextureBrush _clipBrush;

		
		
		
		/////////////////
		//  FUNCTIONS
		private void RecalcRealAngle()
		{
			if (Snapped)
			{
				double d = (MaximumAngle - MinimumAngle) / NumberTicks;
				realDialAngle = Math.Floor(DialAngle / d + 0.5) * d;
			}
			else
			{
				realDialAngle = DialAngle;
			}
		}

		private void ResizeDialImage()
		{
			Invalidate();
			if (_dialImage == null) return;
			Image i = new Bitmap(ClientSize.Width, ClientSize.Height);
			using (Graphics g = Graphics.FromImage((Image)i)) g.DrawImage(_dialImage, 0, 0, ClientSize.Width, ClientSize.Height);
			_dialImage = i;
			_texBrush = new TextureBrush(_dialImage);
			_texBrush.TranslateTransform(DialRadius - ClientSize.Width / 2, DialRadius - ClientSize.Height / 2);
			if (ClipDialImage)
				_clipBrush = new TextureBrush(_dialImage);
			else
				_clipBrush = null;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (MinimumValue >= MaximumValue) throw new ControlArgumentException("Minimum Value cannot be greater than or equal to Maximum Value");
			if (MaximumValue <= MinimumValue) throw new ControlArgumentException("Maximum Value cannot be less than or equal to Minimum Value");

			if (DialMode == SpinDialMode.Clamped)
			{
				if (_minAngle >= _maxAngle) throw new ControlArgumentException("Minimum Angle cannot be greater than or equal to Maximum Angle");
				if (_maxAngle <= _minAngle) throw new ControlArgumentException("Maximum Angle cannot be less than or equal to Minimum Angle");
			}

			if (DialImage == null)
				Paint_NoImage(e.Graphics);
			else
				Paint_Image(e.Graphics);

			base.OnPaint(e);
		}

		private void Paint_NoImage(Graphics g)
		{
			if (_numTicks == 0) throw new ControlArgumentException("Number of Divisions cannot be zero");

			if (borderPen == null)  { borderPen = new Pen(_colorBorder, (float)_borderThickness); }
			if (linePen == null)    { linePen = new Pen(_colorLine, (float)_lineThickness); }
			if (tickPen == null)    { tickPen = new Pen(_colorTick, (float)_tickThickness); }
			if (backBrush == null)  { backBrush = new SolidBrush(_colorBack); }
			if (textBrush == null)  { textBrush = new SolidBrush(ForeColor); }
			if (labelBrush == null) { labelBrush = new SolidBrush(_colorLabel); }
			if (valueBrush == null) { valueBrush = new SolidBrush(_colorValue); }

			int dim = ((ClientSize.Width < ClientSize.Height) ? ClientSize.Width : ClientSize.Height) -  (int)_padding * 2 - (int)_borderThickness * 2;
			int radius = dim / 2;
            g.SmoothingMode = SmoothingMode;
			g.DrawEllipse(borderPen, (float)_padding + (float)_borderThickness, (float)_padding + (float)_borderThickness, dim, dim);
			g.FillEllipse(backBrush, (float)_padding + (float)_borderThickness, (float)_padding + (float)_borderThickness, dim, dim);

			double angle = -90.0 + realDialAngle; //  MinimumAngle + MaximumAngle * (_curVal - MinimumValue) / (MaximumValue - MinimumValue);
			angle *= Math.PI / 180.0;
			g.DrawLine(linePen, ClientSize.Width / 2, ClientSize.Height / 2, ClientSize.Width / 2 + (int)(Math.Cos(angle) * radius * 0.8), ClientSize.Height / 2 + (int)(Math.Sin(angle) * radius * 0.8));

			if (DrawNub)
			{
				int cs = ClientSize.Height;
				int ns = cs / 30;
				g.DrawEllipse(tickPen, cs / 2 - ns / 2, cs / 2 - ns / 2, ns, ns);
				g.FillEllipse(textBrush, cs / 2 - ns / 2, cs / 2 - ns / 2, ns, ns);
			}

			if (DrawTicks)
			{
				double a;
				double tick = (MaximumAngle - MinimumAngle) / (double)_numTicks;
				for (double i = MinimumAngle; i <= (MaximumAngle + tick / 2); i += tick)
				{
					a = (i + 270.0) * Math.PI / 180.0;
					double x1 = Math.Cos(a) * radius;
					double y1 = Math.Sin(a) * radius;
					double x2 = Math.Cos(a) * radius * 0.9;
					double y2 = Math.Sin(a) * radius * 0.9;
					g.DrawLine(tickPen, ClientSize.Width / 2 + (int)x1, ClientSize.Height / 2 + (int)y1, ClientSize.Width / 2 + (int)x2, ClientSize.Height / 2 + (int)y2);

					if (DrawTickValues)
					{
						double x3 = Math.Cos(a) * radius * 0.8;
						double y3 = Math.Sin(a) * radius * 0.8;
						int n = (int)(((double)i - MinimumAngle) / (MaximumAngle - MinimumAngle) * (MaximumValue - MinimumValue)) + (int)MinimumValue;
						string s = ((i + tick) <= (MaximumAngle + tick / 2)) ? n.ToString() : ((int)MaximumValue).ToString();
						Size tSize = TextRenderer.MeasureText(n.ToString(), Font);
						g.DrawString(s, Font, textBrush, ClientSize.Width / 2 + (float)x3 - tSize.Width / 2, ClientSize.Height / 2 + (float)y3 - tSize.Height / 2);
					}
				}
			}
			if (Text != string.Empty)
			{
				Size tSize = TextRenderer.MeasureText(Text, Font);
				g.DrawString(Text, Font, labelBrush, ClientSize.Width / 2 - tSize.Width / 2, ClientSize.Height / 2 - tSize.Height / 2 - ClientSize.Height / 7.5f);
			}

			if (_showValue)
			{
				string cv = CurrentValue.ToString("F" + _numDecimals.ToString()) + ((_valueUnit != string.Empty) ? " " + _valueUnit : "");
				Size tSize = TextRenderer.MeasureText(cv, Font);
				g.DrawString(cv, Font, valueBrush, ClientSize.Width / 2 - tSize.Width / 2, ClientSize.Height / 2 + tSize.Height / 2 + ClientSize.Height / 20);
			}
			
			if (DesignMode && DrawDesignOutline )
			{
				Pen dPen = new Pen(Color.Black, 2.0f);
				if (DialMode == SpinDialMode.Clamped)
				{
					double pX, pY;
					pX = Math.Sin((-MinimumAngle + 180.0) * Math.PI / 180.0);
					pY = Math.Cos((-MinimumAngle + 180.0) * Math.PI / 180.0);
					g.DrawLine(dPen, ClientSize.Width / 2 + (float)(pX * (radius + 10)), ClientSize.Height / 2 + (float)(pY * (radius + 10)), ClientSize.Width / 2 + (float)(pX * (radius - 10)), ClientSize.Height / 2 + (float)(pY * (radius - 10)));

					pX = Math.Sin((-MaximumAngle + 180.0) * Math.PI / 180.0);
					pY = Math.Cos((-MaximumAngle + 180.0) * Math.PI / 180.0);
					g.DrawLine(dPen, ClientSize.Width / 2 + (float)(pX * (radius + 10)), ClientSize.Height / 2 + (float)(pY * (radius + 10)), ClientSize.Width / 2 + (float)(pX * (radius - 10)), ClientSize.Height / 2 + (float)(pY * (radius - 10)));
				}
				g.DrawEllipse(dPen, (float)(ClientSize.Width / 2 - MinimumSpinDistance), (float)(ClientSize.Height / 2 - MinimumSpinDistance), (float)MinimumSpinDistance * 2.0f, (float)MinimumSpinDistance * 2.0f);
			}
		}

		private void Paint_Image(Graphics g)
		{
			//e.Graphics.DrawImage(_dialImage, dstRect, 0, 0, ClientSize.Width, ClientSize.Height, GraphicsUnit.Pixel, _imageAttr);
			if (ClipDialImage)
				g.FillEllipse(_clipBrush, 0, 0, ClientSize.Width, ClientSize.Height);
			else
				g.DrawImage(_dialImage, 0, 0, ClientSize.Width, ClientSize.Height);
			System.Drawing.Drawing2D.Matrix X = new Matrix();
			X.Translate(ClientSize.Width / 2, ClientSize.Height / 2);
			X.Rotate((float)realDialAngle);
			X.Translate(-DialRadius, -DialRadius);
			g.Transform = X;
			g.FillEllipse(_texBrush, 0, 0, DialRadius * 2, DialRadius * 2);

			if (DesignMode && DrawDesignOutline)
			{
				System.Drawing.Drawing2D.Matrix dX = new Matrix();
				g.Transform = dX;
				Pen dPen = new Pen(Color.Black, 2.0f);
				g.DrawEllipse(dPen, ClientSize.Width / 2 - DialRadius, ClientSize.Height / 2 - DialRadius, DialRadius * 2, DialRadius * 2);
				g.DrawEllipse(dPen, (float)(ClientSize.Width / 2 - MinimumSpinDistance), (float)(ClientSize.Height / 2 - MinimumSpinDistance), (float)MinimumSpinDistance * 2.0f, (float)MinimumSpinDistance * 2.0f);

				if (DialMode == SpinDialMode.Clamped)
				{
					double pX, pY;
					pX = Math.Sin((-MinimumAngle + 180.0) * Math.PI / 180.0);
					pY = Math.Cos((-MinimumAngle + 180.0) * Math.PI / 180.0);
					g.DrawLine(dPen, ClientSize.Width / 2 + (float)(pX * DialRadius), ClientSize.Height / 2 + (float)(pY * DialRadius), ClientSize.Width / 2 + (float)(pX * (DialRadius - 10)), ClientSize.Height / 2 + (float)(pY * (DialRadius - 10)));

					pX = Math.Sin((-MaximumAngle + 180.0) * Math.PI / 180.0);
					pY = Math.Cos((-MaximumAngle + 180.0) * Math.PI / 180.0);
					g.DrawLine(dPen, ClientSize.Width / 2 + (float)(pX * DialRadius), ClientSize.Height / 2 + (float)(pY * DialRadius), ClientSize.Width / 2 + (float)(pX * (DialRadius - 10)), ClientSize.Height / 2 + (float)(pY * (DialRadius - 10)));
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			int x = ClientSize.Width;
			int y = ClientSize.Height;
			if (x > y)
				ClientSize = new Size(x, x);
			else
				ClientSize = new Size(y, y);
			base.OnResize(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (capturing && (e.Location.Diff(PointCenter).MagnitudeSquared() >= MinimumSpinDistanceSquared))
			{
	
				double ov_ = CurrentValue;
	
				SpinDialPoint p3 = new SpinDialPoint(e.Location.Diff(PointCenter));
				SpinDialPoint p4 = new SpinDialPoint(RefPoint.Diff(PointCenter));
				double angle = p3.AngleBetween(p4);
				if (!angle.IsNaN())
				{
					DialAngle += angle;
					if (DialMode == SpinDialMode.Clamped)
					{
						if (DialAngle < MinimumAngle) DialAngle = MinimumAngle;
						if (DialAngle > MaximumAngle) DialAngle = MaximumAngle;
					}

					double cv_ = CurrentValue;

					if (MeasureDialChanged != null)
						MeasureDialChanged(this, new MeasureDialChangedEventArgs(cv_, cv_ - ov_));

					//if (PropertyChanged != null)
					//	PropertyChanged(this, new PropertyChangedEventArgs("CurrentValue"));

					RefPoint = e.Location;

					Invalidate();
				}
			}
			
			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Add a "too close to center of dial" check...
			capturing = Spinnable && (e.Location.Diff(PointCenter).MagnitudeSquared() < DialRadiusSquared);
			if (capturing)
				RefPoint = e.Location;
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			capturing = false;
			base.OnMouseUp(e);
		}
		
		////////////////////////////////////////////
		//  ICustomTypeDescriptor Implementation	
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
				if ((pd.Category.Length >= 12) && (pd.Category.Substring(0, 12) == "Measure Dial"))
				{
					// Handle properties with specific needs first
					if ((pd.Name == "MaximumAngle") || (pd.Name == "MinimumAngle"))
					{
						if (DialMode == SpinDialMode.Clamped) pdsRet.Add(pd);
					}
					else if (pd.Name == "ValuePerDial")
					{
						if (DialMode == SpinDialMode.Freeform) pdsRet.Add(pd);
					}
					else if (DialImage == null)
					{
						// We have no image
						if ((pd.Category == "Measure Dial Image") && (pd.Name != "DialImage")) continue;
						if (!DrawTicks && ((pd.Name == "DrawTickValues") || (pd.Name == "NumberTicks") || (pd.Name == "ColorTick") || (pd.Name == "TickThickness"))) continue;
						if (!ShowValue && ((pd.Name == "ValueUnit") || (pd.Name == "NumDecimals") || (pd.Name == "ColorValue"))) continue;
						pdsRet.Add(pd);
					}
					else
					{
						// We have an image
						if (pd.Category == "Measure Dial Appearance") continue;
						if (pd.Category == "Measure Dial Colors") continue;
						
						// These don't seem to be affected...
						if (pd.Name == "Font") continue;
						if (pd.Name == "Text") continue;
						if (pd.Name == "ForeColor") continue;
						if (pd.Name == "BackColor" && !ClipDialImage) continue;
						pdsRet.Add(pd);
					}
				}
				else
				{
					pdsRet.Add(pd);
				}
			}
			return pdsRet;
		}
	}

	public class ControlArgumentException : Exception
	{
		public ControlArgumentException() { }
		public ControlArgumentException(string s) : base(s) { }
	}

	public enum SpinDialMode
	{
		Clamped,
		Freeform
	}

	public class MeasureDialChangedEventArgs : EventArgs
	{
		public MeasureDialChangedEventArgs(double nv, double vd)
		{
			newValue_ = nv;
			valueDiff_ = vd;
		}

		private double newValue_;
		public double NewValue { get { return newValue_; } }

		private double valueDiff_;
		public double ValueDiff { get { return valueDiff_; } }
	}

	public delegate void MeasureDialChangedEventHandler(object o, MeasureDialChangedEventArgs e);

	internal class SpinDialPoint
	{
		internal SpinDialPoint(Point p)
		{
			double m = p.Magnitude();
			_nX = (double)p.X / m;
			_nY = (double)p.Y / m;
		}
		internal double _nX;
		internal double _nY;

		internal double DotProduct(SpinDialPoint p)
		{
			return _nX * p._nX + _nY * p._nY;
		}

		internal double AngleBetween(SpinDialPoint p)
		{
			//double d = Math.Atan2(p._nY, p._nX) - Math.Atan2(_nY, _nX);
			return Math.Acos(DotProduct(p)) * 180.0 / Math.PI * (AngleDir(p) ? 1 : -1);
		}

		internal bool AngleDir(SpinDialPoint p)
		{
			return (Math.Atan2(p._nY, p._nX) - Math.Atan2(_nY, _nX)) < 0;
		}
	}

	public class MeasureDialColors : INotifyPropertyChanged
	{

		//////////////
		//  COLORS
		private Color colorBlack_ = Color.White;
		[Category("Measure Dial Colors")]
		[Description("The background color of the dial")]
		[DefaultValue(typeof(Color), "White")]
		public Color ColorBack
		{
			get { return colorBlack_; }
			set
			{
				if (colorBlack_ == value)
					return;
				colorBlack_ = value;
				OnPropertyChanged("ColorBack");
			}
		}
		
		private Color colorBorder_ = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The border color of the dial")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorBorder
		{
			get { return colorBorder_; }
			set
			{
				if (colorBorder_ == value)
					return;
				colorBorder_ = value;
				OnPropertyChanged("ColorBorder");
			}
		}

		private Color colorLine_ = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the dial line")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorLine
		{
			get { return colorLine_; }
			set
			{
				if (colorLine_ == value)
					return;
				colorLine_ = value;
				OnPropertyChanged("ColorLine");
			}
		}

		private Color colorTick_ = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the value ticks")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorTick
		{
			get { return colorTick_; }
			set
			{
				if (colorTick_ == value)
					return;
				colorTick_ = value;
				OnPropertyChanged("ColorTick");
			}
		}

		private Color colorLabel_ = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the label, if set")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorLabel
		{
			get { return colorLabel_; }
			set
			{
				if (colorLabel_ == value)
					return;
				colorLabel_ = value;
				OnPropertyChanged("ColorLabel");
			}
		}

		private Color colorValue_ = Color.Black;
		[Category("Measure Dial Colors")]
		[Description("The color of the current value, if shown")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ColorValue
		{
			get { return colorValue_; }
			set
			{
				if (colorValue_ == value)
					return;
				colorValue_ = value;
				OnPropertyChanged("ColorValue");
			}
		}

		private Color backColor_;
		public Color BackColor
		{
			get
			{
				return backColor_;
			}
			set
			{
				if (backColor_ == value)
					return;
				backColor_ = value;
				OnPropertyChanged("BackColor");
			}
		}
		private Color foreColor_;
		public Color ForeColor
		{
			get
			{
				return foreColor_;
			}
			set
			{
				if (foreColor_ == value)
					return;
				foreColor_ = value;
				OnPropertyChanged("ForeColor");
			}
		}

        protected virtual void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}

}

