using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Megahard.Reflection;
using System.Reflection;

namespace Megahard
{
	public interface IConvertTo
	{
		object ConvertTo(Type destType);
		bool CanConvertTo(Type destType);
	}

	public interface IConvertTo<T> : IConvertTo
	{
		T Convert();
	}

	public interface IConvertFromTo<From, To>
	{
		To ConvertFrom(From from);
	}
	[AttributeUsage(AttributeTargets.Constructor, Inherited = false)]
	public class SmartConverterAttribute : Attribute
	{
	}

	public static class SmartConvert
	{
		public static bool ConvertTo(object value, Type destType, out object convertedValue)
		{
			return ConvertTo(value, destType, null, out convertedValue);
		}
		public static bool ConvertTo(object value, Type destType, TypeConverter converter, out object convertedValue)
		{
			if (value == null)
			{
				if (destType.IsValueType)
					convertedValue = Activator.CreateInstance(destType);
				else
					convertedValue = null;
				return true;
			}

			Type type = value.GetType();
			if (destType.IsAssignableFrom(type))
			{
				convertedValue = value;
				return true;
			}

			TypeConverter tc = converter ?? TypeDescriptor.GetConverter(destType);
			if (tc.CanConvertFrom(type))
			{
				convertedValue = tc.ConvertFrom(value);
				return true;
			}
			tc = TypeDescriptor.GetConverter(value);
			if (tc.CanConvertTo(destType))
			{
				convertedValue = tc.ConvertTo(value, destType);
				return true;
			}
			if(destType.IsGenericType && destType.GetGenericTypeDefinition() == typeof(Nullable<>) && tc.CanConvertTo(destType.GetGenericArguments()[0]))
			{
				convertedValue = tc.ConvertTo(value, destType.GetGenericArguments()[0]);
				return true;
			}
			

			if (destType.IsEnum && type.IsPrimitive)
			{
				try
				{
					convertedValue = Enum.ToObject(destType, value);
					return true;
				}
				catch (ArgumentException)
				{
				}
			}
			try
			{
				if (value is IConvertible && destType.IsPrimitive)
				{
					convertedValue = Convert.ChangeType(value, destType);
					return true;
				}
				if (value is IConvertTo && (value as IConvertTo).CanConvertTo(destType))
				{
					convertedValue = (value as IConvertTo).ConvertTo(destType);
					return true;
				}
			}
			catch (InvalidCastException)
			{
				
			}

			try
			{
				// Try to find a constructor that takes our value as a param and is marked with SmartConverterAttribute, then call it
				var ctor = destType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { type }, null);
				if (ctor != null && ctor.GetCustomAttributes(typeof(SmartConverterAttribute), false).Length > 0)
				{
					convertedValue = ctor.Invoke(new object[] { value} );
					return true;
				}
			}
			catch (System.Exception)
			{
				
			}
			convertedValue = null;
			return false;
		}
		public static object ConvertTo(object value, Type destType)
		{
			return ConvertTo(value, destType, null);
		}
		public static object ConvertTo(object value, Type destType, TypeConverter converter)
		{
			object ret;
			if (ConvertTo(value, destType, converter, out ret))
				return ret;
			throw new InvalidOperationException("SmartTypeConverter cannot convert from " + value.GetType().Describe() + " to " + destType.Describe());
		}
		public static T ConvertTo<T>(object value)
		{
			return (T)ConvertTo(value, typeof(T));
		}
	}
}
