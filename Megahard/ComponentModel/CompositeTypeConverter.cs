using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.ComponentModel
{
	public class CompositeTypeConverter : TypeConverter
	{
		protected CompositeTypeConverter() : this(null) { }
		protected CompositeTypeConverter(TypeConverter conv)
		{
			conv_ = conv ?? new TypeConverter();
		}
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
		{
			return conv_.CanConvertFrom(context, sourceType);	
		}
		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
		{
			return conv_.CanConvertTo(context, destinationType);
		}
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			return conv_.ConvertFrom(context, culture, value);
		}
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
		{
			return conv_.ConvertTo(context, culture, value, destinationType);
		}
		public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
		{
			return conv_.CreateInstance(context, propertyValues);
		}
		public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetCreateInstanceSupported(context);
		}
		public override System.ComponentModel.PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributes)
		{
			return conv_.GetProperties(context, value, attributes);
		}
		public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetPropertiesSupported(context);
		}
		public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetStandardValuesExclusive(context);
		}
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetStandardValues(context);
		}
		public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetStandardValuesSupported(context);
		}
		public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
		{
			return conv_.IsValid(context, value);
		}

		readonly TypeConverter conv_;
	}

	
}
