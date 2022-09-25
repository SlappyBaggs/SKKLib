using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.ComponentModel
{
	/// <summary>
	/// This is a TypeConverter that will forward all Converter functions to another converter
	/// Intent is you derive from them and selectively handle some functions
	/// </summary>
	public class CompositeTypeConverter : TypeConverter
	{
		protected CompositeTypeConverter() : this(null) { }
		protected CompositeTypeConverter(TypeConverter conv)
		{
			conv_ = conv ?? new TypeConverter();
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
		{
			return conv_.CanConvertFrom(context, sourceType);	
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to.</param>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
		{
			return conv_.CanConvertTo(context, destinationType);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <returns>An <see cref="T:System.Object"/> that represents the converted value.</returns>
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			return conv_.ConvertFrom(context, culture, value);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.</param>
		/// <returns>An <see cref="T:System.Object"/> that represents the converted value.</returns>
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
		{
			return conv_.ConvertTo(context, culture, value, destinationType);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary"/> of new property values.</param>
		/// <returns>An <see cref="T:System.Object"/> representing the given <see cref="T:System.Collections.IDictionary"/>, or null if the object cannot be created. This method always returns null.</returns>
		public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
		{
			return conv_.CreateInstance(context, propertyValues);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value; otherwise, false.</returns>
		public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetCreateInstanceSupported(context);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="value">An <see cref="T:System.Object"/> that specifies the type of array for which to get properties.</param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are exposed for this data type, or null if there are no properties.</returns>
		public override System.ComponentModel.PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributes)
		{
			return conv_.GetProperties(context, value, attributes);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be called to find the properties of this object; otherwise, false.</returns>
		public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetPropertiesSupported(context);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exhaustive list of possible values; false if other values are possible.</returns>
		public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetStandardValuesExclusive(context);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetStandardValues(context);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be called to find a common set of values the object supports; otherwise, false.</returns>
		public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return conv_.GetStandardValuesSupported(context);
		}

		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to test for validity.</param>
		/// <returns>true if the specified value is valid for this object; otherwise, false.</returns>
		public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
		{
			return conv_.IsValid(context, value);
		}

		readonly TypeConverter conv_;
	}

	
}
