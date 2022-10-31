using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Mathematics
{
	public static class NumericInfo
	{
		public static IConvertible MinValue(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Decimal:
					return decimal.MinValue;
				case TypeCode.Double:
					return double.MinValue;
				case TypeCode.Int16:
					return short.MinValue;
				case TypeCode.Int32:
					return int.MinValue;
				case TypeCode.Int64:
					return Int64.MinValue;
				case TypeCode.SByte:
					return sbyte.MinValue;
				case TypeCode.Single:
					return float.MinValue;
				case TypeCode.UInt16:
					return ushort.MinValue;
				case TypeCode.UInt32:
					return uint.MinValue;
				case TypeCode.UInt64:
					return UInt64.MinValue;
				case TypeCode.Byte:
					return byte.MinValue;
				default:
					return 0;
			}
		}
		
		public static IConvertible MaxValue(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Decimal:
					return decimal.MaxValue;
				case TypeCode.Double:
					return double.MaxValue;
				case TypeCode.Int16:
					return short.MaxValue;
				case TypeCode.Int32:
					return int.MaxValue;
				case TypeCode.Int64:
					return Int64.MaxValue;
				case TypeCode.SByte:
					return sbyte.MaxValue;
				case TypeCode.Single:
					return float.MaxValue;
				case TypeCode.UInt16:
					return ushort.MaxValue;
				case TypeCode.UInt32:
					return uint.MaxValue;
				case TypeCode.UInt64:
					return UInt64.MaxValue;
				case TypeCode.Byte:
					return byte.MaxValue;
				default:
					return 0;
			}
		}


		public static bool IsIntegerNumber(object val)
		{
			return val == null ? false : IsIntegerNumber(val.GetType());
		}
		public static bool IsIntegerNumber(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Int16:
					return true;
				case TypeCode.Int32:
					return true;
				case TypeCode.Int64:
					return true;
				case TypeCode.SByte:
					return true;
				case TypeCode.UInt16:
					return true;
				case TypeCode.UInt32:
					return true;
				case TypeCode.UInt64:
					return true;
				case TypeCode.Byte:
					return true;
				default:
					return false;
			}
		}

		public static bool IsRealNumber(object val)
		{
			return  val == null ? false : IsRealNumber(val.GetType());
		}
		public static bool IsRealNumber(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Decimal:
					return true;
				case TypeCode.Double:
					return true;
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}

		public static bool IsNumber(object val)
		{
			return val == null ? false : IsNumber(val.GetType());
		}

		public static bool IsNumber(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
				case TypeCode.Decimal:
					return true;
				case TypeCode.Double:
					return true;
				case TypeCode.Int16:
					return true;
				case TypeCode.Int32:
					return true;
				case TypeCode.Int64:
					return true;
				case TypeCode.SByte:
					return true;
				case TypeCode.Single:
					return true;
				case TypeCode.UInt16:
					return true;
				case TypeCode.UInt32:
					return true;
				case TypeCode.UInt64:
					return true;
				case TypeCode.Byte:
					return true;
				default:
					return false;
			}
		}

		static readonly Type[] numericTypes_ = new Type[] 
				{ 
					typeof(sbyte), typeof(short), typeof(int), typeof(byte), typeof(ushort), typeof(uint), 
					typeof(float), typeof(double), typeof(decimal), typeof(Int64), typeof(UInt64),
				};
		public static IEnumerable<Type> NumericTypes
		{
			get
			{
				return numericTypes_;
			}
		}
	}
}
