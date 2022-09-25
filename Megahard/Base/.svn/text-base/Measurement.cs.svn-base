using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Megahard
{
	/// <summary>
	/// Measurement class encapsulate's a measurement of something, it supports two modes absolute and percent
	/// absolutement the value is what it is, percent means it is a percentage of some other value, what the other
	/// value might be is not encapsulated by this concept, this class is intended to be used when u have a value that is
	/// sometimes one mode and sometimes the other, if it is always percent or absolute, just use an int or double or such
	/// </summary>
	[TypeConverter(typeof(Measurement.Converter))]
	public struct Measurement : IEquatable<Measurement>
	{
		public Measurement(double val) : this()
		{
			value_ = val;
		}

		Measurement(double val, bool isPercent) : this()
		{
			value_ = val;
			_isPercent = isPercent;
		}

		public static Measurement AsPercent(double val)
		{
			return new Measurement(val, true);
		}

		readonly bool _isPercent;

		public static Measurement Parse(string s)
		{
			s = s.Trim();
			if (s.EndsWith("%"))
			{
				return new Measurement(double.Parse(s.Substring(0, s.Length - 1)), true);
			}
			return double.Parse(s);
		}

		public static implicit operator Measurement(int val)
		{
			return new Measurement(val);
		}
		public static implicit operator Measurement(float val)
		{
			return new Measurement(val);
		}
		public static implicit operator Measurement(double val)
		{
			return new Measurement(val);
		}
		public static implicit operator Measurement(decimal val)
		{
			return new Measurement((double)val);
		}
		readonly double value_;
		double Value
		{
			get
			{
				return value_;
			}
		}

		public float Evaluate(float reference)
		{
			if (IsPercent)
				return (float)(Value * reference / 100.0);
			return (float)Value;
		}

		public double Evaluate(double reference)
		{
			if (IsPercent)
				return Value * reference / 100.0;
			return Value;
		}

		[Browsable(false)]
		public bool IsPercent
		{
			get { return _isPercent; }
		}

		public override string ToString()
		{
			if(IsPercent)
				return Value.ToString() + "%";
			else
				return Value.ToString();
		}

		static readonly Measurement s_def = new Measurement();
		public static Measurement DefaultValue
		{
			get
			{
				return s_def;
			}
		}

		class Converter : TypeConverter
		{
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				return new StandardValuesCollection(new[] 
				{
					"25%", "33%", "50%", "66%", "75%", "100%"
				});
			}
			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
			{
				if (value is string)
				{
					return Parse(value as string);
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					Measurement meas = (Measurement)value;
					if (meas.IsPercent)
					{
						var mem = typeof(Measurement).GetMember("AsPercent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)[0];
						var instDesc = new InstanceDescriptor(mem, new object[] { meas.Value });
						return instDesc;
					}
					else
					{
						System.Reflection.ConstructorInfo ctor = typeof(Measurement).GetConstructor(new Type[] { typeof(IConvertible)});
						var instDesc = new InstanceDescriptor(ctor, new object[] { meas.Value });
						return instDesc;
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		#region IEquatable<Measurement> Members

		public bool Equals(Measurement other)
		{
			return other.IsPercent == IsPercent && other.Value == Value;
		}

		#endregion
	}

	
}
