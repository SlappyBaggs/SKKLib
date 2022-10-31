using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Megahard.Barcode
{
	public enum HumanReadablePlacement { None, Top, Bottom };
	public class Code39
	{
		public Code39()
		{
			Background = Color.White;
			Font = new Font(FontFamily.GenericMonospace, 8);
			HumanReadablePlacement = HumanReadablePlacement.None;
			WideToNarrowRatio = DefaultWideToNarrowRatio;
			NarrowBarWidth = MinNarrowBarWidth;
			InterCharGapRatio = DefaultInterCharGapRatio;
			Data = "";
			EnforceValidSettings = true;
		}

		void ValidateSettings()
		{
			var minH = CalculateMinHeight(Width);
			Height = Math.Max(Height, minH);
		}

		[DefaultValue(HumanReadablePlacement.None)]
		public HumanReadablePlacement HumanReadablePlacement
		{
			get;
			set;
		}

		public Color Background
		{
			get;
			set;
		}
		bool ShouldSerializeBackground()
		{
			return Background != Color.White;
		}

		[DefaultValue(0.0f)]
		public float Height
		{
			get;
			set;
		}

		public float Width
		{
			get
			{
				return CalculateWidth(Data, NarrowBarWidth, InterCharGapWidth, WideToNarrowRatio);
			}
		}

		public Font Font
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool EnforceValidSettings
		{
			get;
			set;
		}

		bool ShouldSerializeFont()
		{
			return HumanReadablePlacement != HumanReadablePlacement.None;
		}

		public string Data
		{
			get;
			set;
		}

		bool ShouldSerializeData()
		{
			return Data.HasChars();
		}

		[DefaultValue(MinNarrowBarWidth)]
		public float NarrowBarWidth
		{
			get;
			set;
		}


		[DefaultValue(DefaultWideToNarrowRatio)]
		public float WideToNarrowRatio
		{
			get;
			set;
		}
		[DefaultValue(DefaultInterCharGapRatio)]
		public float InterCharGapRatio
		{
			get;
			set;
		}

		public float InterCharGapWidth
		{
			get { return NarrowBarWidth * InterCharGapRatio; }
		}

		// X	the width of the smallest bar
		// N	wide to narrow multiplier
		// I	inter character gap width
		// H	height of the bars

		const float MinNarrowBarWidth = 0.008f;
		const float MaxNarrowBarWidth = 0.03f;
		const float MinWideToNarrowRatio = 2.0f;
		const float MaxWideToNarrowRatio = 3.0f;
		const float DefaultWideToNarrowRatio = 2.1f;
		const float MinInterCharGapRatio = 1.0f;
		const float DefaultInterCharGapRatio = 1.5f;
		const float MaxInterCharGapRatio = 3.0f;
		const float MinHeight = 0.25f;
		RectangleF[] rects_;

		void BuildRects()
		{
			string actualdata = "*" + Data + "*";
			List<RectangleF> rects = new List<RectangleF>(actualdata.Length * 5);
			float baseX = 0;
			foreach (char c in actualdata)
			{
				rects.AddRange(GenerateRects(c, NarrowBarWidth, WideToNarrowRatio, Height, baseX));
				baseX += (3 * WideToNarrowRatio + 6) * NarrowBarWidth + InterCharGapWidth;
			}
			rects_ = rects.ToArray();
		}

		static IEnumerable<RectangleF> GenerateRects(char c, float X, float N, float H, float baseX)
		{
			float NX = N * X;
			float[] widths = null;
			switch(c)
			{
				case '0':
					widths = new float[] { X, X, X, NX, NX, X, NX, X, X};
					break;
				case '1':
					widths = new float[] { NX, X, X, NX, X, X, X, X, NX};
					break;
				case '2':
					widths = new float[] { X, X, NX, NX, X, X, X, X, NX};
					break;
				case '3':
					widths = new float[] { NX, X, NX, NX, X, X, X, X, X};
					break;
				case '4':
					widths = new float[] { X, X, X, NX, NX, X, X, X, NX};
					break;
				case '5':
					widths = new float[] { NX, X, X, NX, NX, X, X, X, X};
					break;
				case '6':
					widths = new float[] { X, X, NX, NX, NX, X, X, X, X};
					break;
				case '7':
					widths = new float[] { X, X, X, NX, X, X, NX, X, NX};
					break;
				case '8':
					widths = new float[] { NX, X, X, NX, X, X, NX, X, X};
					break;
				case '9':
					widths = new float[] { X, X, NX, NX, X, X, NX, X, X};
					break;
				case 'A':
					widths = new float[] { NX, X, X, X, X, NX, X, X, NX};
					break;
				case 'B':
					widths = new float[] { X, X, NX, X, X, NX, X, X, NX};
					break;
				case 'C':
					widths = new float[] { NX, X, NX, X, X, NX, X, X, X};
					break;
				case 'D':
					widths = new float[] { X, X, X, X, NX, NX, X, X, NX};
					break;
				case 'E':
					widths = new float[] { NX, X, X, X, NX, NX, X, X, X};
					break;
				case 'F':
					widths = new float[] { X, X, NX, X, NX, NX, X, X, X};
					break;
				case 'G':
					widths = new float[] { X, X, X, X, X, NX, NX, X, NX};
					break;
				case 'H':
					widths = new float[] { NX, X, X, X, X, NX, NX, X, X};
					break;
				case 'I':
					widths = new float[] { X, X, NX, X, X, NX, NX, X, X};
					break;
				case 'J':
					widths = new float[] { X, X, X, X, NX, NX, NX, X, X};
					break;
				case 'K':
					widths = new float[] { NX, X, X, X, X, X, X, NX, NX};
					break;
				case 'L':
					widths = new float[] { X, X, NX, X, X, X, X, NX, NX};
					break;
				case 'M':
					widths = new float[] { NX, X, NX, X, X, X, X, NX, X};
					break;
				case 'N':
					widths = new float[] { X, X, X, X, NX, X, X, NX, NX};
					break;
				case 'O':
					widths = new float[] { NX, X, X, X, NX, X, X, NX, X};
					break;
				case 'P':
					widths = new float[] { X, X, NX, X, NX, X, X, NX, X};
					break;
				case 'Q':
					widths = new float[] { X, X, X, X, X, X, NX, NX, NX};
					break;
				case 'R':
					widths = new float[] { NX, X, X, X, X, X, NX, NX, X};
					break;
				case 'S':
					widths = new float[] { X, X, NX, X, X, X, NX, NX, X};
					break;
				case 'T':
					widths = new float[] { X, X, X, X, NX, X, NX, NX, X};
					break;
				case 'U':
					widths = new float[] { NX, NX, X, X, X, X, X, X, NX};
					break;
				case 'V':
					widths = new float[] { X, NX, NX, X, X, X, X, X, NX};
					break;
				case 'W':
					widths = new float[] { NX, NX, NX, X, X, X, X, X, X};
					break;
				case 'X':
					widths = new float[] { X, NX, X, X, NX, X, X, X, NX};
					break;
				case 'Y':
					widths = new float[] { NX, NX, X, X, NX, X, X, X, X};
					break;
				case 'Z':
					widths = new float[] { X, NX, NX, X, NX, X, X, X, X};
					break;
				case '-':
					widths = new float[] { X, NX, X, X, X, X, NX, X, NX};
					break;
				case '.':
					widths = new float[] { NX, NX, X, X, X, X, NX, X, X};
					break;
				case ' ':
					widths = new float[] { X, NX, NX, X, X, X, NX, X, X};
					break;
				case '*':
					widths = new float[] { X, NX, X, X, NX, X, NX, X, X};
					break;
				case '$':
					widths = new float[] { X, NX, X, NX, X, NX, X, X, X};
					break;
				case '/':
					widths = new float[] { X, NX, X, NX, X, X, X, NX, X};
					break;
				case '+':
					widths = new float[] { X, NX, X, X, X, NX, X, NX, X};
					break;
				case '%':
					widths = new float[] { X, X, X, NX, X, NX, X, NX, X};
					break;
				default:
					throw new InvalidOperationException(string.Format("Invalid character '{0}' in code39 barcode", c));
			}

			float pos = baseX;
			for (int i = 0; i < widths.Length; i += 2)
			{
				yield return new RectangleF(pos, 0.0f, widths[i], H);
				if(i < (widths.Length - 1))
					pos += widths[i] + widths[i + 1];
			}
		}

		const string ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -.$/+%";
		bool IsValidString(string s)
		{
			foreach(char c in s)
			{
				if (!ValidCharacters.Contains(c))
					return false;
			}
			return true;
		}


		void DrawHumanReadable(Graphics g)
		{
			if (HumanReadablePlacement == HumanReadablePlacement.None)
				return;
			
			Font f = Font ?? new Font(FontFamily.GenericMonospace, 8);
			var format = StringFormat.GenericTypographic;
			format.Alignment = StringAlignment.Center;
			var sz = g.MeasureString(Data, f, new SizeF(Width, 0), format);

			RectangleF rect = (HumanReadablePlacement == HumanReadablePlacement.Top) ? 
				new RectangleF(0, 0, Width, sz.Height) : new RectangleF(0, Height - sz.Height, Width, sz.Height);

			using (var br = new SolidBrush(Background))
			{
				g.FillRectangle(br, rect);
				g.DrawString(Data, f, Brushes.Black, rect, format);
			}
		}
		public void DrawBarCode(Graphics g)
		{
			if(EnforceValidSettings)
				ValidateSettings();
			var pu = g.PageUnit;
			try
			{
				g.PageUnit = GraphicsUnit.Inch;
				BuildRects();
				g.FillRectangles(Brushes.Black, rects_);
				DrawHumanReadable(g);
			}
			finally
			{
				g.PageUnit = pu;
			}
		}
		public void DrawBarCode(Graphics g, float x, float y)
		{
			if (EnforceValidSettings)
				ValidateSettings();
			var pu = g.PageUnit;
			var state = g.Save();
			try
			{
				g.PageUnit = GraphicsUnit.Inch;
				g.TranslateTransform(x, y);
				BuildRects();
				g.FillRectangles(Brushes.Black, rects_);
				DrawHumanReadable(g);
			}
			finally
			{
				g.Restore(state);
				g.PageUnit = pu;
			}
		}

		public float CharWidth
		{
			get
			{
				return (3 * WideToNarrowRatio + 6) * NarrowBarWidth;
			}
		}

		public float CharactersPerInch
		{
			get
			{
				return 1.0f / (CharWidth + InterCharGapWidth);
			}
		}
		public static float CalculateMinWidth(string data)
		{
			data = data ?? "";
			float L = (data.Length + 2) * (3 * MinWideToNarrowRatio + 6) * MinNarrowBarWidth + (data.Length + 1) * MinInterCharGapRatio * MinNarrowBarWidth;
			return L;
		}
		public static float CalculateMaxWidth(string data)
		{
			data = data ?? "";
			float L = (data.Length + 2) * (3 * MaxWideToNarrowRatio + 6) * MaxNarrowBarWidth + (data.Length + 1) * MaxInterCharGapRatio * MaxNarrowBarWidth;
			return L;
		}
		public static float CalculateWidth(string data, float X, float I, float N)
		{
			data = data ?? "";
			float L = (data.Length + 2) * (3 * N + 6) * X + (data.Length + 1) * I;
			return L;
		}

		public static float CalculateMinHeight(string data)
		{
			float l = CalculateMinWidth(data);
			return Math.Max(MinHeight, l * 0.15f);
		}

		public static float CalculateMinHeight(float width)
		{
			return Math.Min(MinHeight, width * 0.15f);
		}
	}
}
