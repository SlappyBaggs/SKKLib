#define FRAMEWORKMENUS
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Megahard.Controls
{
	#region Enumerations

	public enum RulerUnits
	{
		Points = 0,
		Pixels = 1,
		Centimetres = 2,
		Inches = 3,
	}

	public enum RulerAlignment
	{
		TopOrLeft,
		Middle,
		BottomOrRight
	}
	#endregion

	/// <summary>
	/// Summary description for RulerControl.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(reslocator), "Megahard.Icons.Ruler.bmp")]
	public class Ruler : ControlBase, IMessageFilter, INotifyPropertyChanged
	{

		const int WM_MOUSEMOVE    = 0x0200;
		const int WM_MOUSELEAVE   = 0x02A3;
		const int WM_NCMOUSELEAVE = 0x02A2;
		const float DefaultDPI = 96.0f;
		const float AutoValue = 0.0f;

		bool				_DrawLine			= false;
		bool				_InControl			= false;
		int					_mousePosition		= 1;
		int					_OldMousePosition	= -1;
		Bitmap				_Bitmap				= null;

		Orientation	_Orientation;
		RulerUnits _Units;
		RulerAlignment _RulerAlignment = RulerAlignment.BottomOrRight;
		Border3DStyle _3DBorderStyle = Border3DStyle.Etched;
		
		int _StartValue = 0;
		bool _MouseTrackingOn   = false;
		bool _VerticalNumbers	= true;
		float _dpi = DefaultDPI;

		#region Constrcutors/Destructors

		public Ruler()
		{
			this.ContextMenu = new ContextMenu();
			System.Windows.Forms.MenuItem mnuPoints = new System.Windows.Forms.MenuItem("Points", new EventHandler(Popup_Click));
			System.Windows.Forms.MenuItem mnuPixels = new System.Windows.Forms.MenuItem("Pixels", new EventHandler(Popup_Click));
			System.Windows.Forms.MenuItem mnuCentimetres = new System.Windows.Forms.MenuItem("Centimeters", new EventHandler(Popup_Click));
			System.Windows.Forms.MenuItem mnuInches = new System.Windows.Forms.MenuItem("Inches", new EventHandler(Popup_Click));
			System.Windows.Forms.MenuItem mnuMillimetres = new System.Windows.Forms.MenuItem("Millimeters", new EventHandler(Popup_Click));
			ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { mnuPoints, mnuPixels, mnuCentimetres, mnuInches, mnuMillimetres });

			Units = RulerUnits.Inches;
			base.BackColor = System.Drawing.Color.White;
			base.ForeColor = System.Drawing.Color.Black;
			base.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);

			Ticks.ObjectChanged += new EventHandler<Megahard.Data.ObjectChangedEventArgs>(Ticks_ObjectChanged);
		}

		void Ticks_ObjectChanged(object sender, Megahard.Data.ObjectChangedEventArgs e)
		{
			//Invalidate();
		}

		#endregion

		#region Methods

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool PreFilterMessage(ref Message m)
		{
			if (!this._MouseTrackingOn) return false;

			if (m.Msg == WM_MOUSEMOVE)
			{
				int X = 0;
				int Y = 0;

				// The mouse coordinate are measured in screen coordinates because thats what 
				// Control.MousePosition returns.  The Message,LParam value is not used because
				// it returns the mouse position relative to the control the mouse is over. 
				// Chalk and cheese.

				Point pointScreen = Control.MousePosition;

				// Get the origin of this control in screen coordinates so that later we can 
				// compare it against the mouse point to determine it we've hit this control.

				Point pointClientOrigin = new Point(X, Y);
				pointClientOrigin = PointToScreen(pointClientOrigin);

				_DrawLine = false;
				_InControl = false;

				// Work out whether the mouse is within the Y-axis bounds of a vertital ruler or 
				// within the X-axis bounds of a horizontal ruler

				if (this.Orientation == Orientation.Horizontal)
				{
					_DrawLine = (pointScreen.X >= pointClientOrigin.X) && (pointScreen.X <= pointClientOrigin.X + this.Width);
				}
				else
				{
					_DrawLine = (pointScreen.Y >= pointClientOrigin.Y) && (pointScreen.Y <= pointClientOrigin.Y + this.Height);
				}

				// If the mouse is in valid position...
				if (_DrawLine)
				{
					// ...workout the position of the mouse relative to the 
					X = pointScreen.X-pointClientOrigin.X;
					Y = pointScreen.Y-pointClientOrigin.Y;

					// Determine whether the mouse is within the bounds of the control itself
					_InControl = (this.ClientRectangle.Contains(new Point(X, Y)));

					// Make the relative mouse position available in pixel relative to this control's origin
					ChangeMousePosition((this.Orientation == Orientation.Horizontal) ? X : Y);
				} 
				else
				{
					ChangeMousePosition(-1);
				}

				// Paint directly by calling the OnPaint() method.  This way the background is not 
				// hosed by the call to Invalidate() so paining occurs without the hint of a flicker
				PaintEventArgs e = null;
				try
				{
					e = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
					OnPaint(e);
				}
				finally
				{
					e.Graphics.Dispose();
				}
			}

			if ((m.Msg == WM_MOUSELEAVE) || 
				(m.Msg == WM_NCMOUSELEAVE))
			{
				_DrawLine = false;
				PaintEventArgs paintArgs = null;
				try
				{
					paintArgs = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
					this.OnPaint(paintArgs);
				}
				finally
				{
					paintArgs.Graphics.Dispose();
				}
			}
			return false;  // Whether or not the message is filtered
		}

		static void ConfigureDefaultTicks(Data.ObservableCollection<RulerTick> ticks, RulerUnits unit)
		{
			ticks.Clear();
			switch (unit)
			{
				case RulerUnits.Pixels:
					ticks.Add(new RulerTick(100, Measurement.AsPercent(90)) { ShowValue = true });
					ticks.Add(new RulerTick(50, Measurement.AsPercent(75)));
					ticks.Add(new RulerTick(25, Measurement.AsPercent(50)));
					ticks.Add(new RulerTick(5, Measurement.AsPercent(25)));
					break;
				case RulerUnits.Inches:
					ticks.Add(new RulerTick(1, Measurement.AsPercent(90)) { ShowValue = true });
					ticks.Add(new RulerTick(0.5f, Measurement.AsPercent(75)));
					ticks.Add(new RulerTick(0.25f, Measurement.AsPercent(50)));
					ticks.Add(new RulerTick(0.125f, Measurement.AsPercent(25)));
					ticks.Add(new RulerTick(0.0625f, Measurement.AsPercent(12)));
					break;
				case RulerUnits.Centimetres:
					ticks.Add(new RulerTick(1, Measurement.AsPercent(90)) { ShowValue = true });
					ticks.Add(new RulerTick(0.1f, Measurement.AsPercent(50)));
					break;
				case RulerUnits.Points:
					ticks.Add(new RulerTick(72, Measurement.AsPercent(90)) { ShowValue = true });
					ticks.Add(new RulerTick(36, Measurement.AsPercent(50)));
					ticks.Add(new RulerTick(18, Measurement.AsPercent(25)));
					break;
			}
		}

		public double PixelToScaleValue(int offset)
		{
			return this.CalculateValue(offset);
		}

		public int ScaleValueToPixel(double scaleValue)
		{
			return CalculatePixel(scaleValue);
		}

		#endregion

		#region Overrides

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			if(e.InvalidRect == ClientRectangle)
				_Bitmap = null;
			base.OnInvalidated(e);
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (_Bitmap != null)
				{
					_Bitmap.Dispose();
					_Bitmap = null;
				}
			}
			base.Dispose( disposing );
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			this.Invalidate();
		}

		[Description("Draws the ruler marks in the scale requested.")]
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			DrawControl(e.Graphics);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged (e);

			if (this.Visible)
			{
				if (_MouseTrackingOn) Application.AddMessageFilter(this);
			}
			else
			{
				// DOn't change the tracking state so that the filter will be added again when the control is made visible again; 
				if (_MouseTrackingOn) RemoveMessageFilter(); 
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((Control.MouseButtons & MouseButtons.Right) != 0)
			{
				this.ContextMenu.Show(this, PointToClient(Control.MousePosition));
			}
			else
			{
				EventArgs eClick = new EventArgs();
				this.OnClick(eClick);
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed (e);
			RemoveMessageFilter();
			_MouseTrackingOn = false;
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved (e);
			RemoveMessageFilter();
			_MouseTrackingOn = false;
		}

		private void RemoveMessageFilter()
		{
			try
			{
				if (_MouseTrackingOn) 
				{
					Application.RemoveMessageFilter(this);
				}
			} 
			catch {}
		}

		#endregion

		#region Event Handlers

		private void Popup_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
			Units = (RulerUnits)item.Index;
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			_DrawLine = false;
			Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Invalidate();
		}
		#endregion

		#region Properties

		[DefaultValue(typeof(Border3DStyle),"Etched"), Description("The border style use the Windows.Forms.Border3DStyle type"), Category("Ruler"), Design.ShowInSmartPanel]
		public Border3DStyle BorderStyle
		{
			get
			{
				return _3DBorderStyle;
			}
			set
			{
				_3DBorderStyle = value;
				Invalidate();
			}
		}

		[Description("Horizontal or vertical layout")]
		[Category("Ruler")]
		[DefaultValue(Orientation.Horizontal), Design.ShowInSmartPanel]
		public Orientation Orientation
		{ 
			get { return _Orientation; }
			set 
			{
				_Orientation = value;
				Invalidate();
			}
		}

		[Description("A value from which the ruler marking should be shown.  Default is zero.")]
		[Category("Ruler")]
		[DefaultValue(0)]
		public int StartValue
		{
			get { return _StartValue; }
			set 
			{
				if (StartValue == value)
					return;
				_StartValue = value;
				Invalidate();
			}
		}

		[Description("The scale to use")]
		[Category("Ruler")]
		[DefaultValue(RulerUnits.Inches)]
		public RulerUnits Units
		{
			get { return _Units; }
			set 
			{
				if (_Units == value)
					return;
				if (_dpi == AutoValue)
				{
				}
				_Units = value;
				ConfigureDefaultTicks(Ticks, Units);
				// Setup the menu
				for(int i = 0; i <= 4; i++)
					ContextMenu.MenuItems[i].Checked = false;
				ContextMenu.MenuItems[(int)value].Checked = true;
				Invalidate();
			}
		}

		[Description("The value of the current mouse position expressed in unit of the scale set (centimetres, inches, etc.")]
		[Category("Ruler")]
		[Browsable(false)]
		public double ScaleValue
		{
			get {return CalculateValue(_mousePosition); }
		}

		[Description("TRUE if a line is displayed to track the current position of the mouse and events are generated as the mouse moves.")]
		[Category("Ruler")]
		[DefaultValue(false)]
		public bool MouseTrackingOn
		{
			get { return _MouseTrackingOn; }
			set 
			{ 
				if (value == _MouseTrackingOn) return;
				
				if (value)
				{
					// Tracking is being enabled so add the message filter hook
					if (this.Visible) Application.AddMessageFilter(this);
				}
				else
				{
					// Tracking is being disabled so remove the message filter hook
					Application.RemoveMessageFilter(this);
					ChangeMousePosition(-1);
				}

				_MouseTrackingOn = value;
				Invalidate();
			}
		}

		[Description("The font used to display the division number")]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				Invalidate();
			}
		}

		[Description("Return the mouse position as number of pixels from the top or left of the control.  -1 means that the mouse is positioned before or after the control.")]
		[Browsable(false)]
		public int MouseLocation
		{
			get { return _mousePosition; }
		}

		[Description("")]
		[Category("Ruler")]
		[DefaultValue(true)]
		public bool VerticalNumbers
		{
			get { return _VerticalNumbers; }
			set
			{
				_VerticalNumbers = value;
				Invalidate();
			}
		}

		[Category("Ruler")]
		[AmbientValue(0.0f)]
		[TypeConverter(typeof(AutoFloatConverter))]
		public float DPI
		{
			get 
			{ 
				if(_dpi != AutoValue)
					return _dpi;
				return RulerEdgePixelLength / RulerUnitToInch(Length);
			}
			set
			{
				if (DPI == value)
					return;
				value = Math.Max(value, AutoValue);
				if (value == AutoValue)
					_Length = Length;
				else
					_Length = AutoValue;
				_dpi = value;
				
				OnPropertyChanged("DPI");
				OnPropertyChanged("Length");
				Invalidate();
			}
		}

		int RulerEdgePixelLength
		{
			get { return Orientation == Orientation.Horizontal ? Width : Height; }
		}

		bool ShouldSerializeDPI()
		{
			return _dpi != AutoValue && _dpi != DefaultDPI;
		}

		void ResetDPI()
		{
			DPI = DefaultDPI;
		}
		
		private float _Length;

		[Category("Ruler")]
		[AmbientValue(AutoValue)]
		[TypeConverter(typeof(AutoFloatConverter))]
		public float Length
		{
			get
			{
				if (_Length != AutoValue)
					return _Length;
				return RulerUnitLength;
			}
			set
			{
				if (value == _Length)
					return;
				value = Math.Max(value, AutoValue);
				if (value == AutoValue)
					_dpi = DPI;
				else
					_dpi = AutoValue;
				
				_Length = value;
				OnPropertyChanged("Length");
				OnPropertyChanged("DPI");
				Invalidate();
			}
		}

		void ResetLength()
		{
			_Length = AutoValue;
		}
		bool ShouldSerializeLength()
		{
			return _Length != AutoValue;
		}

		/// <summary>
		/// Size of the ruler in Ruler Units
		/// </summary>
		SizeF RulerUnitSize
		{
			get
			{
				SizeF pixels = Orientation == Orientation.Horizontal ? new SizeF(Width, Height) : new SizeF(Width, Height);
				switch (Units)
				{
					case RulerUnits.Centimetres:
						return new SizeF(Science.UnitConverter.InchToCentimeter(pixels.Width / DPI), Science.UnitConverter.InchToCentimeter(pixels.Height / DPI));
					case RulerUnits.Inches:
						return new SizeF(pixels.Width / DPI, pixels.Height / DPI);
					case RulerUnits.Pixels:
						return pixels;
					case RulerUnits.Points:
						return new SizeF(Science.UnitConverter.InchToPoints(pixels.Width / DPI), Science.UnitConverter.InchToPoints(pixels.Height / DPI));
					default:
						throw new InvalidEnumArgumentException("Units of ruler has invalid enum value");
				}
			}
		}

		RectangleF RulerUnitRect
		{
			get
			{
				return new RectangleF(new PointF(0, 0), RulerUnitSize);
			}
		}

		/// <summary>
		/// Length of the ruler in RulerUnits
		/// </summary>
		float RulerUnitLength
		{
			get
			{
				float pixels = Orientation == Orientation.Horizontal ? Width : Height;
				switch (Units)
				{
					case RulerUnits.Centimetres:
						return Science.UnitConverter.InchToCentimeter(pixels / DPI);
					case RulerUnits.Inches:
						return pixels / DPI;
					case RulerUnits.Pixels:
						return pixels;
					case RulerUnits.Points:
						return Science.UnitConverter.InchToPoints(pixels / DPI);
					default:
						throw new InvalidEnumArgumentException("Units of ruler has invalid enum value");
				}
			}
		}

		Font CreateScaledFont(Font f)
		{
			float pts = f.SizeInPoints;

			float scale = Math.Min(96.0f / DPI, 2.0f);
			
			return new Font(f.FontFamily, pts * scale, GraphicsUnit.Point);
		}

		/// <summary>
		/// Height of the ruler in RulerUnits
		/// </summary>
		float RulerUnitHeight
		{
			get
			{
				float pixels = Orientation == Orientation.Horizontal ? Height : Width;

				switch (Units)
				{
					case RulerUnits.Centimetres:
						return Science.UnitConverter.InchToCentimeter(pixels / DPI);
					case RulerUnits.Inches:
						return pixels / DPI;
					case RulerUnits.Pixels:
						return pixels;
					case RulerUnits.Points:
						return Science.UnitConverter.InchToPoints(pixels / DPI);
					default:
						throw new InvalidEnumArgumentException("Units of ruler has invalid enum value");
				}
			}
		}

		float RulerUnitToPixel(float val)
		{
			switch (Units)
			{
				case RulerUnits.Centimetres:
					return Science.UnitConverter.CentimeterToInch(val) * DPI;
				case RulerUnits.Inches:
					return val * DPI;
				case RulerUnits.Pixels:
					return val;
				case RulerUnits.Points:
					return Science.UnitConverter.PointsToInch(val) * DPI;
				default:
					throw new InvalidEnumArgumentException("Units of ruler has invalid enum value");
			}
		}

		float RulerUnitToInch(float val)
		{
			switch (Units)
			{
				case RulerUnits.Centimetres:
					return Science.UnitConverter.CentimeterToInch(val);
				case RulerUnits.Inches:
					return val;
				case RulerUnits.Pixels:
					return val * DPI;
				case RulerUnits.Points:
					return Science.UnitConverter.PointsToInch(val);
				default:
					throw new InvalidEnumArgumentException("Units of ruler has invalid enum value");
			}
		}

		[Description("Determines how the ruler markings are displayed")]
		[Category("Ruler")]
		[DefaultValue(RulerAlignment.BottomOrRight)]
		public RulerAlignment RulerAlignment
		{
			get { return _RulerAlignment; }
			set 
			{
				if (_RulerAlignment == value) return;
				_RulerAlignment = value;
				Invalidate();
			}
		}


		#endregion

		private double CalculateValue(int offset)
		{
			if (offset < 0) return 0;
			return 0;
		}

		[Description("May not return zero even when a -ve scale number is given as the returned value needs to allow for the border thickness")]
		private int CalculatePixel(double scaleValue)
		{
			return 0;
		}

		public void RenderTrackLine(Graphics g)
		{
			if (_MouseTrackingOn & _DrawLine)
			{
				int offset = Offset();

				// Optionally render Mouse tracking line
				switch(Orientation)
				{
					case Orientation.Horizontal:
						Line(g, _mousePosition, offset, _mousePosition, Height - offset);
						break;
					case Orientation.Vertical:
						Line (g, offset, _mousePosition, Width - offset, _mousePosition);
						break;
				}
			}
		}

		private void DrawControl(Graphics graphics)
		{
			if (!this.Visible) return;

			// Bug reported by Kristoffer F 
			if (this.Width < 1 || this.Height < 1) 
			{
				return;
			}

			if (_Bitmap == null)
			{
				// Create a bitmap
				_Bitmap = new Bitmap(this.Width, this.Height);
				if(Units != RulerUnits.Pixels)
					_Bitmap.SetResolution(DPI, DPI);
				var g = Graphics.FromImage(_Bitmap);

				try
				{
					// Wash the background with BackColor
					g.FillRectangle(new SolidBrush(this.BackColor), RulerUnitRect);

					foreach (var tick in Ticks)
					{
						Brush brush = new SolidBrush(tick.Color.IsEmpty ? this.ForeColor : tick.Color);
						Font realFont = CreateScaledFont(tick.Font ?? Font);
						try
						{
							for (float x = 0; x <= RulerUnitLength; x += tick.Interval)
							{
								PointF start, end;
								CreateDivisionMark(out start, out end, x, tick.Height);
								Line(g, start.X, start.Y, end.X, end.Y);
								if (tick.ShowValue)
								{
									g.DrawString(tick.FormatValue(x + StartValue), 
										realFont, brush, RulerUnitToPixel(start.X), RulerUnitToPixel(start.Y));
									//g.DrawString(tick.FormatValue(x + StartValue), tick.Font ?? Font, brush, start.X, start.Y);
								}
							}
						}
						finally
						{
							brush.Dispose();
							realFont.Dispose();
						}
					}
					if (_3DBorderStyle != Border3DStyle.Flat)
						ControlPaint.DrawBorder3D(g, this.ClientRectangle, this._3DBorderStyle );
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
				finally 
				{
					g.Dispose();
				}
			}

			try
			{
				// Always draw the bitmap
				graphics.DrawImage(_Bitmap, this.ClientRectangle);

				//RenderTrackLine(graphics);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		void CreateDivisionMark(out PointF start, out PointF end, float position, Measurement height)
		{
			// This function is affected by the RulerAlignment setting
			float markStart = 0;
			float markLen = height.Evaluate(RulerUnitHeight);
			float h = RulerUnitHeight;
			switch (RulerAlignment)
			{
				case RulerAlignment.BottomOrRight:
					{
						markStart = h - markLen;
						break;
					}
				case RulerAlignment.Middle:
					{
						markStart = (h - markLen) / 2;
						break;
					}
				default:
					break;
			}

			if (Orientation == Orientation.Horizontal)
			{
				start = new PointF(position, markStart);
				end = new PointF(position, markStart + markLen);
			}
			else
			{
				start = new PointF(markStart, position);
				end = new PointF(markStart + markLen, position);
			}
		}


		/// <summary>
		/// params should be ruler units, we dont go back to pixels until the very end
		/// </summary>
		private void Line(Graphics g, float x1, float y1, float x2, float y2)
		{
			using(SolidBrush brush = new SolidBrush(this.ForeColor))
			{
				using(Pen pen = new Pen(brush, 1.0f))
				{
					g.DrawLine(pen, RulerUnitToPixel(x1), RulerUnitToPixel(y1), RulerUnitToPixel(x2), RulerUnitToPixel(y2));
					//g.DrawLine(pen, x1, y1, x2, y2);
					pen.Dispose();
					brush.Dispose();
				}
			}
		}

		private int Offset()
		{
			switch(this._3DBorderStyle)
			{
				case Border3DStyle.Flat: return 0;
				case Border3DStyle.Adjust: return 0;
				case Border3DStyle.Sunken: return 2;
				case Border3DStyle.Bump: return 2;
				case Border3DStyle.Etched: return 2;
				case Border3DStyle.Raised: return 2;
				case Border3DStyle.RaisedInner: return 1;
				case Border3DStyle.RaisedOuter: return 1;
				case Border3DStyle.SunkenInner: return 1;
				case Border3DStyle.SunkenOuter: return 1;
				default: return 0;
			}
		}

		private int Start()
		{
			switch(this._3DBorderStyle)
			{
				case Border3DStyle.Flat: return 0;
				case Border3DStyle.Adjust: return 0;
				case Border3DStyle.Sunken: return 1;
				case Border3DStyle.Bump: return 1;
				case Border3DStyle.Etched: return 1;
				case Border3DStyle.Raised: return 1;
				case Border3DStyle.RaisedInner: return 0;
				case Border3DStyle.RaisedOuter: return 0;
				case Border3DStyle.SunkenInner: return 0;
				case Border3DStyle.SunkenOuter: return 0;
				default: return 0;
			}
		}

		private void ChangeMousePosition(int newPosition)
		{
			this._OldMousePosition = this._mousePosition;
			this._mousePosition = newPosition;
		}

		class AutoFloatConverter : TypeConverter
		{
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					var strValue = ((string)value).Trim().ToLower();
					if (strValue == "auto")
						return AutoValue;
					return float.Parse(strValue);
				}
				if (value is float)
					return value;
				return base.ConvertFrom(context, culture, value);
			}


			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					float dpi = (float)value;
					if (dpi == AutoValue)
						return "auto";
					return dpi.ToString();
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if(context != null && context.PropertyDescriptor != null && context.PropertyDescriptor.Name == "DPI")
					return new StandardValuesCollection(new[] { AutoValue, 72.0f, 96.0f });
				return new StandardValuesCollection(new[] { AutoValue });
			}
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string prop)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(prop));
		}
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			var copy = PropertyChanged;
			if (copy != null)
				copy(this, args);
		}
		#endregion

		readonly Megahard.Data.ObservableCollection<RulerTick> _ticks = new Megahard.Data.ObservableCollection<RulerTick>();
		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Category("Ruler")]
		public Megahard.Data.ObservableCollection<RulerTick> Ticks
		{
			get { return _ticks; }
		}
	}

	public class RulerTick
	{
		public RulerTick(float interval, Measurement height)
		{
			Interval = interval;
			Height = height;
			Color = Color.Empty;
		}
		public RulerTick()
		{
			Height = Measurement.AsPercent(50);
			Interval = 1.0f;
			Color = Color.Empty;
		}
		public override string ToString()
		{
			return string.Format("Every {0}", Interval);
		}
		[DefaultValue(false)]
		public bool ShowValue { get; set; }

		[DefaultValue(null)]
		public string ValueFormat
		{
			get;
			set;
		}
		
		[DefaultValue(typeof(Measurement), "50%")]
		public Measurement Height { get; set; }

		[DefaultValue(1.0f)]
		public float Interval { get; set; }

		[DefaultValue(null)]
		public Font Font { get; set; }

		[DefaultValue(ContentAlignment.Near)]
		public ContentAlignment Alignment { get; set; }

		[DefaultValue(typeof(Color), "")]
		public Color Color { get; set; }

		public string FormatValue(float val)
		{
			return string.Format(ValueFormat ?? "{0}", val);
		}

	}
}