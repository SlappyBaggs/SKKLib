using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace SKKLib.TypeConverters
{
#if USE
	public class EnumDescConverter : EnumConverter
	{
		protected Type enumType;

		/// <summary>
		/// Gets Enum Value's Description Attribute
		/// </summary>
		/// <param name="value">The value you want the description attribute for</param>
		/// <returns>The description, if any, else it's .ToString()</returns>
		public static string GetEnumDescription(Enum value)
		{
			string s = value.ToString();
			FieldInfo fi = value.GetType().GetField(s, BindingFlags.Public | BindingFlags.Static);
			if (fi == null)
				return s;
			var attr = fi.GetCustomAttribute<DescriptionAttribute>(false);
			return attr != null ? attr.Description : s;
		}

		/// <summary>
		/// Gets the description for certaing named value in an Enumeration
		/// </summary>
		/// <param name="value">The type of the Enumeration</param>
		/// <param name="name">The name of the Enumeration value</param>
		/// <returns>The description, if any, else the passed name</returns>
		public static string GetEnumDescription(Type value, string name)
		{
			FieldInfo fi = value.GetField(name, BindingFlags.Public | BindingFlags.Static);
			if (fi == null)
				return name;
			var attr = fi.GetCustomAttribute<DescriptionAttribute>(false);
			return attr != null ? attr.Description : name;
		}


		/// <summary>
		/// Gets the value of an Enum, based on it's Description Attribute or named value
		/// </summary>
		/// <param name="value">The Enum type</param>
		/// <param name="description">The description or name of the element</param>
		/// <returns>The value, or the passed in or null if it was not found</returns>
		public static object GetEnumValue(Type value, string description)
		{
			FieldInfo[] fis = value.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fi in fis)
			{
				var attr = fi.GetCustomAttribute<DescriptionAttribute>(false);
				if (attr != null && attr.Description == description)
				{
					return fi.GetValue(null);
				}
				if (fi.Name == description)
				{
					return fi.GetValue(null);
				}
			}
			return null;
		}

		public EnumDescConverter(Type type)
			: base(type)
		{
			enumType = type;
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (value is Enum && destinationType == typeof(string))
			{
				return GetEnumDescription((Enum)value);
			}
			if (value is string && destinationType == typeof(string))
			{
				return GetEnumDescription(enumType, (string)value);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				return GetEnumValue(enumType, (string)value);
			}
			//if (value is Enum)
			//{
			//	return GetEnumDescription((Enum)value);
			//}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (value is string)
			{
				return GetEnumValue(enumType, (string)value) != null;
			}
			return base.IsValid(context, value);
		}

		protected virtual ICollection FilterStandardValues(ICollection vals)
		{
			return vals;
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ArrayList values = new ArrayList();
			foreach (FieldInfo fi in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
			{
				var omit = fi.GetCustomAttribute<OmitDescription>(false);
				if (omit != null)
					continue;
				var attr = fi.GetCustomAttribute<DescriptionAttribute>(false);
				values.Add(attr != null ? attr.Description : fi.Name);
			}
			return new TypeConverter.StandardValuesCollection(FilterStandardValues(values));
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public class OmitDescription : Attribute
	{
	}
#endif
}
