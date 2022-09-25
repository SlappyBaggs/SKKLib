using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public partial class RealTimePlotter : UserControl
	{
		public RealTimePlotter()
		{
			InitializeComponent();

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			MinimumValue = 0;
			MaximumValue = 100;
			//CurrentValue = 50;

			NumPointsToShow = 20;
			DrawGrid = false;
			_gridWidth = 1;
			GridSpace = 10.0;
			
			DrawTicks = false;
			DrawTickValues = false;
			DrawTickUnits = false;		
			TickDecimals = 0;
			NumTicks = 10;
			TickWidth = 1;
			TickLength = 10;
			TickInside = false;

			DrawGraphValues = false;
			DrawGraphUnits = false;
			GraphDecimals = 0;
			
			ValueUnit = "";

			_tickColor = Color.LimeGreen;
			_gridColor = Color.DarkGray;
			
			timer1.Tick += new EventHandler(TimerEvent);
			timer1.Interval = 500;
			//timer1.Start();

			SetActiveQueue(GetNewQueue());
		}
		private PointQueue activeQueue;

		////////////////////////
		//  PENS AND BRUSHES
		Pen tickPen;
		Pen gridPen;
		SolidBrush textBrush;

		
		
		//////////////
		//  COLORS
		[DefaultValue(typeof(Color), "LimeGreen")]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; textBrush = null; }
		}

		[Category("Real Time Plotter Colors")]
		[Description("Color of the plot line")]
		[DefaultValue(typeof(Color), "LimeGreen")]
		public Color LineColor
		{
			get { return activeQueue._lineColor; }
			set { activeQueue._lineColor = value; }
		}

		private Color _tickColor;
		[Category("Real Time Plotter Colors")]
		[Description("Color of the ticks, if drawn")]
		[DefaultValue(typeof(Color), "LimeGreen")]
		public Color TickColor
		{
			get { return _tickColor; }
			set { _tickColor = value; tickPen = null; }
		}

		private Color _gridColor;
		[Category("Real Time Plotter Colors")]
		[Description("Color of the grid, if drawn")]
		[DefaultValue(typeof(Color), "DarkGray")]
		public Color GridColor
		{
			get { return _gridColor; }
			set { _gridColor = value; gridPen = null; }
		}


		
		////////////////////
		//  DRAW OPTIONS
		[Category("Real Time Plotter Draw Options")]
		[Description("Number of time steps the plotter will display")]
		[DefaultValue(20)]
		public int NumPointsToShow { get; set; }

		[Category("Real Time Plotter Draw Options")]
		[Description("Width of the plot line")]
		[DefaultValue(2)]
		public int LineWidth
		{
			get { return activeQueue._lineWidth; }
			set { activeQueue._lineWidth = value; }
		}

		[Category("Real Time Plotter Draw Options")]
		[Description("Draw the point values on the graph")]
		[DefaultValue(false)]
		public bool DrawGraphValues { get; set; }

		[Category("Real Time Plotter Draw Options")]
		[Description("Draw the units next to the graph values")]
		[DefaultValue(false)]
		public bool DrawGraphUnits { get; set; }

		[Category("Real Time Plotter Draw Options")]
		[Description("How many decimals to display in values")]
		[DefaultValue(0)]
		public int GraphDecimals { get; set; }

		

		
		////////////////////		
		//  GRID OPTIONS
		[Category("Real Time Plotter Grid Options")]
		[Description("Draw a grid over the entire control")]
		[DefaultValue(false)]
		public bool DrawGrid { get; set; }

		private int _gridWidth;
		[Category("Real Time Plotter Grid Options")]
		[Description("Width of the grid lines")]
		[DefaultValue(1)]
		public int GridWidth
		{
			get { return _gridWidth; }
			set { _gridWidth = value; gridPen = null; }
		}

		[Category("Real Time Plotter Grid Options")]
		[Description("Space between grid lines")]
		[DefaultValue(10.0)]
		public double GridSpace { get; set; }

		
		
		
		////////////////////
		//  TICK OPTIONS
		[Category("Real Time Plotter Tick Options")]
		[Description("Draw tick marks denoting value spacing")]
		[DefaultValue(false)]
		public bool DrawTicks { get; set; }

		[Category("Real Time Plotter Tick Options")]
		[Description("Draw text values next to tick marks, if ticks are drawn")]
		[DefaultValue(false)]
		public bool DrawTickValues { get; set; }
		
		[Category("Real Time Plotter Tick Options")]
		[Description("Draw the units next to the tick values")]
		[DefaultValue(false)]
		public bool DrawTickUnits { get; set; }

		[Category("Real Time Plotter Tick Options")]
		[Description("Draw the units next to the value units")]
		[DefaultValue(0)]
		public int TickDecimals { get; set; }

		[Category("Real Time Plotter Tick Options")]
		[Description("Number of tick marks to draw")]
		[DefaultValue(10)]
		public int NumTicks { get; set; }

		private int _tickWidth;
		[Category("Real Time Plotter Tick Options")]
		[Description("Width of tick lines")]
		[DefaultValue(1)]
		public int TickWidth
		{
			get { return _tickWidth; }
			set { _tickWidth = value; tickPen = null; }
		}

		[Category("Real Time Plotter Tick Options")]
		[Description("Length of tick lines")]
		[DefaultValue(10)]
		public int TickLength { get; set; }

		[Category("Real Time Plotter Tick Options")]
		[Description("Draw ticks inside of graph")]
		[DefaultValue(false)]
		public bool TickInside { get; set; }
		
		
		

		//////////////
		//  VALUES
		[Category("Real Time Plotter Value Options")]
		[Description("Minimum Value the plotter will plot")]
		[DefaultValue(0.0)]
		public double MinimumValue { get; set; }

		[Category("Real Time Plotter Value Options")]
		[Description("Maximum value the plotter will plot")]
		[DefaultValue(100.0)]
		public double MaximumValue { get; set; }
		
		[Category("Real Time Plotter Value Options")]
		[Description("Milliseconds between polling")]
		[DefaultValue(500)]
		public int Interval
		{
			get { return timer1.Interval; }
			set { timer1.Interval = value; }
		}

		[Category("Real Time Plotter Value Options")]
		[Description("The units the values are in")]
		[DefaultValue("")]
		public string ValueUnit { get; set; }

		public void Stop()
		{
			timer1.Stop();
		}

		public void Start()
		{
			timer1.Start();
		}


		public Func<double> PollFunction
		{
			set { activeQueue.PollFunction = value; }
		}
		
		//////////////////////
		// LIST HANDLING        
		private int _activeIndex;
		public void SetActiveQueue(int index)
		{
			if(index < allPoints.Count)
			{
				activeQueue = allPoints[index];
				_activeIndex = index;
			}
		}

		public int GetActiveIndex() { return _activeIndex; }

		public int GetNewQueue()
		{
			PointQueue q = new PointQueue();
			allPoints.Add(q);
			// Make a new one the active index?
			return allPoints.Count - 1;
		}

		public void SetQueueColor(Color c)
		{
			activeQueue._lineColor = c;
		}
		
		/////////////////
		//  FUNCTIONS
		public double CurrentValue { get { return activeQueue.CurrentValue; } }

		public void AddValue(double v)
		{
			bool b = false;
			AddValue(v, b);
		}

		public void AddValue(double v, bool b)
		{
			activeQueue.Enqueue(new PlotPoint(v, b));
			Invalidate();
		}

		// Each queue will have its own poll function, but all in all we only have 1 timer and
		// this timer event will signal the poll functions in all the Queues...
		private void TimerEvent(Object o, EventArgs e)
		{
			foreach (PointQueue q in allPoints)
			{
				activeQueue = q;
				// Add the last value
				if (DesignMode)
				{
					Random r = new Random();
					//AddValue((MaximumValue - MinimumValue) * r.NextDouble() + MinimumValue, false);
				}
				else
					AddValue((q.PollFunction != null) ? q.PollFunction() : q.CurrentValue, false);

				// If we have too many, remove the old
				int num = q.CountNonIntermediate();
				if (q.CountNonIntermediate() > (NumPointsToShow + 1))
				{
					// Remove all intermediates
					while (q.Peek().Intermediate)
						q.Dequeue();

					// Remove first non-intermediate
					q.Dequeue();
				}
			}
		}

		private void RealTimePlotter_Paint(object sender, PaintEventArgs e)
		{
			double x, y;
			string s;
			Size sz;

			if (tickPen == null) tickPen = new Pen(_tickColor, TickWidth);
			if (gridPen == null) gridPen = new Pen(_gridColor, GridWidth);
			if (textBrush == null) textBrush = new SolidBrush(ForeColor);

			int w = ClientSize.Width;
			int h = ClientSize.Height;
			int off_w = 0;
			int off_h = 0;

			if (TickInside)
			{
				w -= TickLength;
				h -= TickLength;
				off_w = off_h = TickLength;

				if (DrawTicks && DrawTickValues)
				{
					s = MaximumValue.ToString("F" + TickDecimals.ToString());
					sz = TextRenderer.MeasureText(s, Font);
					off_w += sz.Width;
				}
			}

			if (DrawTicks)
			{
				y = (double)h / (double)NumTicks;
				x = (double)w / (double)NumPointsToShow;
				for (int i = 0; i < NumTicks; i++)
					e.Graphics.DrawLine(tickPen, 0, (float)(y * i), TickLength, (float)(y * i));

				for (int i = 0; i < NumPointsToShow; i++)
					e.Graphics.DrawLine(tickPen, (float)(x * i + off_w), ClientSize.Height, (float)(x * i + off_w), ClientSize.Height - TickLength);// Keep these using ClientSize.Height

				if (DrawTickValues)
				{
					for (int i = 0; i <= NumTicks; i++)
					{
						s = (i * (MaximumValue - MinimumValue) / NumTicks + MinimumValue).ToString("F" + TickDecimals.ToString()) + (DrawTickUnits ? (" " + ValueUnit) : "");
						sz = TextRenderer.MeasureText(s, Font);
						x = ClientSize.Height - (float)(y * i) - off_h;
						if (i == 0) x -= sz.Height;
						else if (i != NumTicks) x -= sz.Height / 2;
						e.Graphics.DrawString(s, Font, textBrush, TickLength + 1, (float)x);
					}
				}
			}

			if (DrawGrid)
			{
				x = w / GridSpace;
				y = h / GridSpace;
				for (int i = 0; i < x; i++)
					e.Graphics.DrawLine(gridPen, (float)(GridSpace * i), 0, (float)(GridSpace * i), h);
				for (int i = 0; i < y; i++)
					e.Graphics.DrawLine(gridPen, off_w, (float)(GridSpace * i), w + off_w, (float)(GridSpace * i));
			}

			// Total time span this graph will show...
			double totTime = NumPointsToShow * Interval;

			// Pixel/Time ratio - width
			double xfac = w / totTime;

			// Pixel/Value ratio - height
			double yfac = h / (MaximumValue - MinimumValue);

			// Point holders
			double x1, y1, x2, y2;

			double buffX;
			int blanks;
			bool first;
			PlotPoint lastPoint;
			DateTime firstX = DateTime.Now;

			foreach(PointQueue q in allPoints)
			{
				if (q.Count == 0) continue;

				if (q.linePen == null) q.linePen = new Pen(q._lineColor, q._lineWidth);
			   
				// How many points do we need to skip
				blanks = NumPointsToShow - q.CountNonIntermediate();

				// Where does the first point start?
				buffX = blanks * Interval * xfac;

				// We record the first point, but do no drawing...
				first = true;

				lastPoint = null;
				
				// Loop through the points
				foreach (PlotPoint p in q)
				{
					if (first)
					{
						first = false;
						firstX = p.Time;
					}
					else
					{
						x1 = off_w + buffX + (lastPoint.Time - firstX).TotalMilliseconds * xfac;
						y1 = h - (lastPoint.Y - MinimumValue)* yfac;

						x2 = off_w + buffX + (p.Time - firstX).TotalMilliseconds * xfac;
						y2 = h - (p.Y - MinimumValue) * yfac;

						if (x1 < off_w) x1 = off_w;
						if (x2 < off_w) x2 = off_w;
						e.Graphics.DrawLine(q.linePen, (float)x1, (float)y1, (float)x2, (float)y2);
						if (DrawGraphValues)
						{
							s = p.Y.ToString("f" + GraphDecimals.ToString()) + (DrawGraphUnits ? (" " + ValueUnit) : "");
							sz = TextRenderer.MeasureText(s, Font);
							if ((y2 + sz.Height) > h) y2 = h - sz.Height;
							e.Graphics.DrawString(s, Font, textBrush, (float)x2, (float)y2);
						}
					}
					lastPoint = p;
				}
			}
		}

		
		
		
		/////////////////////////////////////////////////////
		// POINT STRUCTURES
		//

		private List<PointQueue> allPoints = new List<PointQueue>();
		
		private class PointQueue : Queue<PlotPoint>
		{
			internal PointQueue()
			{
				linePen = null;
				_lineWidth = 2;
				_lineColor = Color.LimeGreen;
			}

			internal double CurrentValue { get { return (this.Count == 0) ? 0 : this.Last().Y; } }
			
			internal Pen linePen;
			internal Func<double> PollFunction;

			internal int _lineWidth;
			internal Color _lineColor;

			internal int CountNonIntermediate()
			{
				int ret = 0;
				foreach (PlotPoint p in this) if (!p.Intermediate) ret++;
				return ret;
			}
		}

		private class PlotPoint
		{
			internal PlotPoint(double y, bool i)
			{
				_y = y;
				_intermediate = i;
				_time = DateTime.Now;
			}

			private DateTime _time;
			internal DateTime Time { get { return _time; } }

			private double _y;
			internal double Y { get { return _y; } }

			private bool _intermediate;
			internal bool Intermediate { get { return _intermediate; } }
		}
	}
}
