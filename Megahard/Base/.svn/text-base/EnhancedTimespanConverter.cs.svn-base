using System.ComponentModel;
using System;
namespace Megahard.TypeConverters
{

	enum EnhancedTimeSpanConverterStyle { Milliseconds, Seconds, Minutes, Hours, Days };
	public abstract class EnhancedTimeSpanConverter : TimeSpanConverter
	{
		private readonly EnhancedTimeSpanConverterStyle style_;
		internal EnhancedTimeSpanConverter(EnhancedTimeSpanConverterStyle style)
		{
			style_ = style;
		}
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
		{
			if (destinationType == typeof(string) && value is TimeSpan)
			{
				TimeSpan ts = (TimeSpan)value;
				switch (style_)
				{
					case EnhancedTimeSpanConverterStyle.Milliseconds:
						return ts.TotalMilliseconds.ToString();
					case EnhancedTimeSpanConverterStyle.Seconds:
						return ts.TotalSeconds.ToString();
					case EnhancedTimeSpanConverterStyle.Minutes:
						return ts.TotalMinutes.ToString();
					case EnhancedTimeSpanConverterStyle.Hours:
						return ts.TotalHours.ToString();
					case EnhancedTimeSpanConverterStyle.Days:
						return ts.TotalDays.ToString();
				}
			}

			return base.ConvertTo(context, culture, value, destinationType);
		} 

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				string s = (string)value;
				if (s == string.Empty)
					return TimeSpan.FromMilliseconds(0);
				try
				{
					double d = double.Parse(s);
					switch (style_)
					{
						case EnhancedTimeSpanConverterStyle.Milliseconds:
							return TimeSpan.FromMilliseconds(d);
						case EnhancedTimeSpanConverterStyle.Seconds:
							return TimeSpan.FromSeconds(d);
						case EnhancedTimeSpanConverterStyle.Minutes:
							return TimeSpan.FromMinutes(d);
						case EnhancedTimeSpanConverterStyle.Hours:
							return TimeSpan.FromHours(d);
						case EnhancedTimeSpanConverterStyle.Days:
							return TimeSpan.FromDays(d);
					}
				}
				catch (FormatException ex)
				{
					throw new FormatException("Unable to convert string to TimeSpan via EnhancedTimeSpanConverter", ex);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}
	}

	public class MillisecondsTimeSpanConverter : EnhancedTimeSpanConverter
	{
		MillisecondsTimeSpanConverter() : base(EnhancedTimeSpanConverterStyle.Milliseconds) { }
	}

	public class SecondsTimeSpanConverter : EnhancedTimeSpanConverter
	{
		SecondsTimeSpanConverter() : base(EnhancedTimeSpanConverterStyle.Seconds) { }
	}

	public class MinutesTimeSpanConverter : EnhancedTimeSpanConverter
	{
		MinutesTimeSpanConverter() : base(EnhancedTimeSpanConverterStyle.Minutes) { }
	}

	public class HoursTimeSpanConverter : EnhancedTimeSpanConverter
	{
		HoursTimeSpanConverter() : base(EnhancedTimeSpanConverterStyle.Hours) { }
	}

	public class DaysTimeSpanConverter : EnhancedTimeSpanConverter
	{
		DaysTimeSpanConverter() : base(EnhancedTimeSpanConverterStyle.Days) { }
	}


}