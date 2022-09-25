using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Megahard.ComponentModel;
using System.ComponentModel.Design;

namespace Megahard.Controls
{
	public enum LEDState
	{
		ON = 0,
		OFF,
		Disabled,
		Invalid
	};
	public enum LEDShape
	{
		Round  = 0,
		Square
	}
	
	public enum LEDColor
	{
		Red = 0,
		Green,
		Yellow,
		Bluenot
	}

	[TypeConverter(typeof(PropertySorter))]
	[DefaultEvent("LEDControlBlinked")]
	[ToolboxBitmap(typeof(reslocator), "Megahard.Icons.LED.ico")]
	public partial class LEDControl : ControlBase
	{
		public event LEDControlBlinkedEventHandler LEDControlBlinked;

		public LEDControl()
		{
			InitializeComponent();

			TimerInterval = 1000;
			timer.Tick += new EventHandler(TimerOnTick);
			timer.Start();

			LEDState = lastLEDState = LEDState.ON;
			LEDShape = LEDShape.Round;
			LEDColor = LEDColor.Red;
		}

		private Timer timer = new Timer();
		private const int LEDSize = 16;
		private Rectangle? LEDRect;
		private LEDState lastLEDState;

		[Browsable(false)]
		public bool LEDOn { get { return ledState_ == LEDState.ON; } }

		[Category("LED Control")]
		[Description("Current state of LED")]
		[PropertyOrder(1)]
		[DefaultValue(LEDState.ON)]
		public LEDState LEDState
		{
			get { return ledState_; }
			set 
			{
				if (ledState_ == value)
					return; // no need to redraw
				ledState_ = value; 
				LEDRect = null; 
				Invalidate(); 
			}
		}
		private LEDState ledState_;

		[Category("LED Control")]
		[Description("Shape of LED")]
		[PropertyOrder(2)]
		[DefaultValue(LEDShape.Round)]
		[Design.ShowInSmartPanel]
		public LEDShape LEDShape
		{
			get { return ledShape_; }
			set 
			{
				if (ledShape_ == value)
					return; // no need to redraw
				ledShape_ = value; 
				LEDRect = null; 
				Invalidate(); 
			}
		}
		private LEDShape ledShape_;

		[Category("LED Control")]
		[Description("Color of LED")]
		[PropertyOrder(3)]
		[DefaultValue(LEDColor.Red)]
		[Design.ShowInSmartPanel]
		public LEDColor LEDColor
		{
			get { return ledColor_; }
			set 
			{
				if (ledColor_ == value)
					return; // no need to redraw
				ledColor_ = value; 
				LEDRect = null; 
				Invalidate(); 
			}
		}
		private LEDColor ledColor_;

		[Category("LED Control")]
		[Description("Interval between blinks")]
		[PropertyOrder(4)]
		[DefaultValue(1000)]
		[Design.ShowInSmartPanel]
		public int TimerInterval
		{
			get { return interval_; }
			set
			{
				timer.Stop();
				interval_ = value;
				if (interval_ > 0)
				{
					timer.Interval = interval_;
					timer.Start();
				}
			}
		}
		private int interval_;

		private void TimerOnTick(object sender, EventArgs ea)
		{
			if (LEDState == LEDState.Disabled)
				return;
			
			if (LEDState == LEDState.ON)
				LEDState = LEDState.OFF;
			else if (LEDState == LEDState.OFF)
				LEDState = LEDState.ON;
			
			if (LEDControlBlinked != null)
				LEDControlBlinked(this, new LEDControlBlinkEventArgs(LEDState));
		}

		private void LEDControl_Paint(object sender, PaintEventArgs e)
		{
			if (LEDRect == null)
				LEDRect = new Rectangle(Convert.ToInt32(ledState_) * LEDSize,
										Convert.ToInt32(ledColor_) * LEDSize + Convert.ToInt32(ledShape_) * System.Enum.GetValues(typeof(LEDColor)).Length * LEDSize,
										LEDSize,
										LEDSize);
			e.Graphics.DrawImage(imageList_.Images[0], this.ClientRectangle, LEDRect.Value, GraphicsUnit.Pixel);
		}

		private void LEDControl_EnabledChanged(object sender, EventArgs e)
		{
			if (!Enabled)
			{
				lastLEDState = LEDState;
				LEDState = LEDState.Disabled;
				timer.Stop();
			}
			else
			{
				LEDState = lastLEDState;
				timer.Start();
			}
		}
	}

	public class LEDControlBlinkEventArgs : EventArgs
	{
		public LEDControlBlinkEventArgs(LEDState state)
		{
			ledState_ = state;
		}

		private LEDState ledState_;
		public LEDState LEDState { get { return ledState_; } }
	}

	public delegate void LEDControlBlinkedEventHandler(object o, LEDControlBlinkEventArgs e);


	[TypeDescriptionProvider(typeof(ForwardToolboxBitmapAttribute<LEDControl, ToolStripLED>))]
	public class ToolStripLED : ToolStripControlHost<LEDControl>
	{
		public ToolStripLED()
			: base(new LEDControl())
		{
		}
	}



}
