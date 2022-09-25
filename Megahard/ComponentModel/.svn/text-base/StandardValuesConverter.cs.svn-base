using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.TypeConverters
{

	public class StandardValuesConverter : TypeConverter
	{
		public StandardValuesConverter(Type t)
		{
			type_ = t;
		}
		readonly Type type_;

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			var props = type_.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
			List<object> values = new List<object>();

			foreach (var prop in props)
			{
				var attrs = prop.GetCustomAttributes(typeof(Megahard.ComponentModel.StandardValueAttribute), true);
				if (attrs.Length == 0)
					continue;
				Megahard.ComponentModel.StandardValueAttribute stdVal = attrs[0] as Megahard.ComponentModel.StandardValueAttribute;
				if(prop.GetIndexParameters().Length > 0)
					continue;
				if (stdVal.IsStandardValue)
					values.Add(prop.GetValue(null, null));
			}
			return new StandardValuesCollection(values);
		}
	}

	public class StandardValuesExclusiveConverter : StandardValuesConverter
	{
		public StandardValuesExclusiveConverter(Type t) : base(t)
		{
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
