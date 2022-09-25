using System;
using System.Drawing;
using System.Windows.Forms;

namespace Megahard.Drawing
{
	public class TextImageData
	{
		internal TextImageData(Font font, string txt, Color fg, Color bg, int width, int height, Bitmap bmp)
		{
			bmp_ = bmp;
			fg_ = fg;
			bg_ = bg;
			font_ = font;
			propText_ = txt;
			Color[] ca = new Color[width * height];
			int i = 0;
			for (int h = 0; h < height; h++)
				for (int w = 0; w < width; w++)
					ca[i++] = bmp.GetPixel(w, h);
			width_ = width;
			height_ = height;
			colors_ = ca;
		}

		readonly string propText_;
		public string Text
		{
			get { return propText_; }
		}

		readonly Font font_;
		public Font Font
		{
			get { return font_; }
		}

		readonly int width_;
		public int Width { get { return width_; } }

		readonly int height_;
		public int Height { get { return height_; } }

		readonly Color[] colors_;
		public Color[] ColorData { get { return colors_; } }
		public Color GetColor(int x, int y)
		{
			return colors_[x + y * Width];
		}

		readonly Color fg_;
		public Color Foreground
		{
			get { return fg_; }
		}
		readonly Color bg_;
		public Color Background
		{
			get { return bg_; }
		}

		readonly Bitmap bmp_;

		public Bitmap Bitmap { get { return bmp_; } }
	}
}


namespace System.Drawing
{
	public static class mhFontExtension
	{
		public static Megahard.Drawing.TextImageData GetColorArray(this Font f, string s)
		{
			return f.GetColorArray(s, Color.Black, Color.White);
		}

		public static Megahard.Drawing.TextImageData GetColorArray(this Font f, string s, Color fC, Color bC)
		{
			SolidBrush foreBrush = new SolidBrush(fC);
			SolidBrush backBrush = new SolidBrush(bC);
			Size sz = TextRenderer.MeasureText(s, f);
			Bitmap bmap = new Bitmap(sz.Width, sz.Height);
			//bmap.SetResolution(300, 300);
			using (Graphics g = System.Drawing.Graphics.FromImage(bmap))
			{
				g.FillRectangle(backBrush, 0, 0, sz.Width, sz.Height);
				g.DrawString(s, f, foreBrush, 0.0f, 0.0f);

				return new Megahard.Drawing.TextImageData(f, s, fC, bC, sz.Width, sz.Height, bmap);
			}
		}
	}
}
