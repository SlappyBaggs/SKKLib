/*****************************************************************
* Module: EnumDescConverter.cs
* Type: C# Source Code
* Version: 1.0
* Description: Enum Converter using Description Attributes
* 
* Revisions
* ------------------------------------------------
* [F] 24/02/2004, Jcl - Shaping up
* [B] 25/02/2004, Jcl - Made it much easier :-)
* 
 * http://www.codeproject.com/KB/cs/enumdescconverter.aspx?df=100&forumid=34468&select=1802196
*****************************************************************/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Collections;
using Megahard.ExtensionMethods;
namespace Megahard.TypeConverters
{
	public class EnumDescConverter : System.ComponentModel.EnumConverter
	{
		protected System.Type enumType;
		public static string GetEnumDescription(Enum value)
		{
			string s = value.ToString();
			FieldInfo fi = value.GetType().GetField(s, BindingFlags.Public | BindingFlags.Static);
			if (fi == null)
				return s;
			var attr = fi.GetCustomAttribute<DescriptionAttribute>(false);
			return attr != null ? attr.Description : s;
		}
		public static string GetEnumDescription(System.Type value, string name)
		{
			FieldInfo fi = value.GetField(name, BindingFlags.Public | BindingFlags.Static);
			if (fi == null)
				return name;
			var attr = fi.GetCustomAttribute<DescriptionAttribute>(false);
			return attr != null ? attr.Description : name;
		}
		public static object GetEnumValue(System.Type value, string description)
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

		public EnumDescConverter(System.Type type)
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
}