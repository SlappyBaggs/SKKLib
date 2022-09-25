using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Science
{

	public struct PhysicalQuantity : IComparable<PhysicalQuantity>, IConvertible
	{
		public PhysicalQuantity(PhysicalUnit unit) : this()
		{
			unit_ = unit;
		}

		public PhysicalQuantity(double val, PhysicalUnit unit) : this()
		{
			Value = val;
			unit_ = unit;
		}

		public override string ToString()
		{
			return Value.ToString() + " " + Unit.Symbol;
		}

		public double Value { get; set; }
		public PhysicalUnit Unit
		{
			get
			{
				return unit_;
			}
		}

		public PhysicalQuantity ValueInBaseUnits
		{
			get
			{
				return Unit.ConvertToBase(Value);
			}
		}

		public PhysicalQuantity ConvertTo(PhysicalUnit unit)
		{
			EnsureCompatible(Unit, unit);
			return unit.ConvertFromBase(ValueInBaseUnits.Value);
		}

		static bool AreCompatible(PhysicalQuantity q1, PhysicalQuantity q2)
		{
			return AreCompatible(q1.Unit, q2.Unit);
		}
		static bool AreCompatible(PhysicalUnit u1, PhysicalUnit u2)
		{
			return PhysicalUnit.CanConvert(u1, u2);
		}
		static void EnsureCompatible(PhysicalUnit u1, PhysicalUnit u2)
		{
			if (!PhysicalUnit.CanConvert(u1, u2))
				throw new InvalidOperationException(string.Format("{0} and {1} are not part of same base quantity", u1, u2));
		}


		readonly PhysicalUnit unit_;

		#region IComparable<PhysicalQuantity> Members

		public int CompareTo(PhysicalQuantity other)
		{
			EnsureCompatible(Unit, other.Unit);
			var v1 = ValueInBaseUnits.Value;
			var v2 = other.ValueInBaseUnits.Value;
			if (v1 == v2)
				return 0;
			if (v1 < v2)
				return -1;
			return 1;
		}

		#endregion

		public static bool operator >(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			return val1.CompareTo(val2) > 0;
		}

		public static bool operator <(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			return val1.CompareTo(val2) < 0;
		}

		public static bool operator <=(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			return val1.CompareTo(val2) <= 0;
		}
		public static bool operator >=(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			return val1.CompareTo(val2) >= 0;
		}
		
		public static PhysicalQuantity operator +(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			EnsureCompatible(val1.Unit, val2.Unit);
			return PhysicalUnit.SelectLarger(val1.Unit, val2.Unit).ConvertFromBase(val1.ValueInBaseUnits.Value + val2.ValueInBaseUnits.Value);
		}

		public static PhysicalQuantity operator -(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			EnsureCompatible(val1.Unit, val2.Unit);
			return PhysicalUnit.SelectLarger(val1.Unit, val2.Unit).ConvertFromBase(val1.ValueInBaseUnits.Value - val2.ValueInBaseUnits.Value);
		}


		public static PhysicalQuantity operator +(PhysicalQuantity val, double d)
		{
			return new PhysicalQuantity(val.Value + d, val.Unit);
		}

		public static PhysicalQuantity operator -(PhysicalQuantity val, double d)
		{
			return new PhysicalQuantity(val.Value + d, val.Unit);
		}



		#region Equality Overloads
		public static bool operator ==(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			return AreCompatible(val1, val2) && val1.ValueInBaseUnits.Value == val2.ValueInBaseUnits.Value;
		}

		public static bool operator !=(PhysicalQuantity val1, PhysicalQuantity val2)
		{
			return !AreCompatible(val1, val2) || val1.ValueInBaseUnits.Value != val2.ValueInBaseUnits.Value;
		}

		public override int GetHashCode()
		{
			return ValueInBaseUnits.Value.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is PhysicalQuantity)
			{
				return Equals((PhysicalQuantity)obj);
			}
			return false;
		}

		public bool Equals(PhysicalQuantity val)
		{
			return AreCompatible(this, val) && ValueInBaseUnits == val.ValueInBaseUnits;
		}
		#endregion

		#region IConvertible Members

		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Double;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(Value, provider);
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(Value, provider);
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(Value, provider);
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(Value, provider);
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(Value, provider);
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(Value, provider);
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(Value, provider);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(Value, provider);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(Value, provider);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(Value, provider);
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(Value, provider);
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return ToString();
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(Value, conversionType, provider);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(Value, provider);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(Value, provider);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(Value, provider);
		}

		#endregion
	}

	public class BaseQuantity
	{
		protected BaseQuantity(string name)
		{
			propName_ = name;
		}

		protected void RegisterUnits(params PhysicalUnit[] units)
		{
			foreach (var u in units)
			{
				units_.Add(u.Symbol, u);
				if (u.IsBaseUnit && BaseUnit == null)
					BaseUnit = u;
			}
		}

		readonly Dictionary<string, PhysicalUnit> units_ = new Dictionary<string, PhysicalUnit>();

		public PhysicalUnit ResolveSymbol(string symbol)
		{
			PhysicalUnit ret;
			units_.TryGetValue(symbol, out ret);
			return ret;
		}

		public PhysicalQuantity Parse(string s)
		{
			int pos = s.IndexOf(' ');
			if (pos == -1)
				return new PhysicalQuantity(double.Parse(s), BaseUnit);
			var val = double.Parse(s.Substring(0, pos));
			var unit = ResolveSymbol(s.Substring(pos + 1));
			if(unit == null)
				throw new FormatException(string.Format("'{0}' is not a valid symbol for the BaseQuantity '{1}'", unit, Name));

			return new PhysicalQuantity(val, unit);
		}

		#region string Name { get; readonly; }
		readonly string propName_;
		public string Name
		{
			get { return propName_; }
		}
		#endregion

		public PhysicalUnit BaseUnit
		{
			get;
			private set;
		}
	}

	public class PhysicalUnit
	{
		/// <summary>
		/// baseMult is how many base units equal one of this unit
		/// </summary>
		public PhysicalUnit(string symbol, string name, BaseQuantity baseQ, double baseMult)
		{
			propSymbol_ = symbol;
			baseQuantity_ = baseQ;
			baseMult_ = baseMult;
			propName_ = name;
		}
		#region string Name { get; readonly; }
		readonly string propName_;
		public string Name
		{
			get { return propName_; }
		}
		#endregion
		readonly BaseQuantity baseQuantity_;
		readonly double baseMult_;
		#region string Symbol { get; readonly; }
		readonly string propSymbol_;
		public string Symbol
		{
			get { return propSymbol_; }
		}
		#endregion

		public static bool CanConvert(PhysicalUnit unit1, PhysicalUnit unit2)
		{
			return unit1.baseQuantity_ == unit2.baseQuantity_;
		}

		public PhysicalQuantity ConvertToBase(double d)
		{
			return new PhysicalQuantity(d * baseMult_, baseQuantity_.BaseUnit);
		}

		public PhysicalQuantity ConvertFromBase(double d)
		{
			return new PhysicalQuantity(d / baseMult_, this);
		}

		public override string ToString()
		{
			return Symbol; 
		}

		public bool IsBaseUnit
		{
			get { return baseMult_ == 1; }
		}

		internal static PhysicalUnit SelectLarger(PhysicalUnit u1, PhysicalUnit u2)
		{
			return u1.baseMult_ > u2.baseMult_ ? u1 : u2;
		}

	}

	public static class UnitConverter
	{
		public static float InchToCentimeter(float inches)
		{
			return inches * 2.54f;
		}
		public static double InchToCentimeter(double inches)
		{
			return inches * 2.54;
		}

		public static decimal InchToCentimeter(decimal inches)
		{
			return inches * 2.54m;
		}

		public static float CentimeterToInch(float cm)
		{
			return cm / 2.54f;
		}
		public static double CentimeterToInch(double cm)
		{
			return cm / 2.54;
		}

		public static decimal CentimeterToInch(decimal cm)
		{
			return cm / 2.54m;
		}

		public static float InchToPoints(float inch)
		{
			return inch * 72.0f;
		}

		public static double InchToPoints(double inch)
		{
			return inch * 72.0;
		}

		public static decimal InchToPoints(decimal inch)
		{
			return inch * 72.0m;
		}

		public static float PointsToInch(float points)
		{
			return points / 72.0f;
		}

		public static double PointsToInch(double points)
		{
			return points / 72.0;
		}

		public static decimal PointsToInch(decimal points)
		{
			return points / 72.0m;
		}

	}
}
