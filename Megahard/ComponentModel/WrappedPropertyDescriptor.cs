using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Megahard.ExtensionMethods;
using System.ComponentModel;
namespace Megahard.ComponentModel
{
	public class WrappedPropertyDescriptor : PropertyDescriptor
	{
		public WrappedPropertyDescriptor(PropertyDescriptor property, object target) : base(property.Name, property.Attributes.ToArray<Attribute>() )
		{
			System.Diagnostics.Debug.Assert(property != null);
			System.Diagnostics.Debug.Assert(target != null);
			target_ = target;
			property_ = property;
		}

		public static PropertyDescriptorCollection WrapProperties(object ob, Attribute[] attrs)
		{
			System.Diagnostics.Debug.Assert(ob != null);
			return WrapProperties(TypeDescriptor.GetProperties(ob, attrs), ob);
		}
		
		public static PropertyDescriptorCollection WrapProperties(object ob)
		{
			System.Diagnostics.Debug.Assert(ob != null);
			return WrapProperties(TypeDescriptor.GetProperties(ob), ob);
		}

		public static PropertyDescriptorCollection WrapProperties(PropertyDescriptorCollection pdc, object ob)
		{
			PropertyDescriptor[] arr = new PropertyDescriptor[pdc.Count];
			for (int i = 0; i < pdc.Count; ++i)
				arr[i] = new WrappedPropertyDescriptor(pdc[i], ob);
			return new PropertyDescriptorCollection(arr);
		}

		protected override object GetInvocationTarget(Type type, object instance)
		{
			return target_;
		}

		readonly PropertyDescriptor property_;
		readonly object target_;
		public override bool CanResetValue(object component)
		{
			return property_.CanResetValue(target_);
		}

		public override Type ComponentType
		{
			get { return property_.ComponentType; }
		}

		public override object GetValue(object component)
		{
			return property_.GetValue(target_);
		}

		public override bool IsReadOnly
		{
			get { return property_.IsReadOnly; }
		}

		public override Type PropertyType
		{
			get { return property_.PropertyType; }
		}

		public override void ResetValue(object component)
		{
			property_.ResetValue(target_);
		}

		public override void SetValue(object component, object value)
		{
			property_.SetValue(target_, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return property_.ShouldSerializeValue(target_);
		}

		public override void AddValueChanged(object component, EventHandler handler)
		{
			property_.AddValueChanged(component, handler);
		}

		public override void RemoveValueChanged(object component, EventHandler handler)
		{
			property_.RemoveValueChanged(component, handler);
		}

		public override TypeConverter Converter
		{
			get
			{
				return property_.Converter;
			}
		}

		public override bool SupportsChangeEvents
		{
			get
			{
				return property_.SupportsChangeEvents;
			}
		}
	}
}
