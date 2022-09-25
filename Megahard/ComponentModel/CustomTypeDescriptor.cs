using System;
using System.ComponentModel;
namespace Megahard.ComponentModel
{
	/// <summary>
	/// This CustomTypeDescriptor returns null for all members
	/// </summary>
	public class BlankTypeDescriptor : System.ComponentModel.ICustomTypeDescriptor
	{
		#region ICustomTypeDescriptor Members

		public virtual System.ComponentModel.AttributeCollection GetAttributes()
		{
			return null;
		}

		public virtual string GetClassName()
		{
			return null;
		}

		public virtual string GetComponentName()
		{
			return null;
		}

		public virtual System.ComponentModel.TypeConverter GetConverter()
		{
			return null;
		}

		public virtual System.ComponentModel.EventDescriptor GetDefaultEvent()
		{
			return null;
		}

		public virtual System.ComponentModel.PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		public virtual object GetEditor(Type editorBaseType)
		{
			return null;
		}

		public virtual System.ComponentModel.EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return null;
		}

		public virtual System.ComponentModel.EventDescriptorCollection GetEvents()
		{
			return GetEvents(null);
		}

		public virtual System.ComponentModel.PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return null;
		}

		public virtual System.ComponentModel.PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		public virtual object GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
		{
			return null;
		}

		#endregion

		sealed class thenull : BlankTypeDescriptor
		{
		}

		static readonly ICustomTypeDescriptor s_Null = new thenull();
		public static ICustomTypeDescriptor NullDescriptor
		{
			get { return s_Null; }
		}
	}

	/// <summary>
	/// This CustomTypeDescriptor has a parent and a child, it first tries the child, if it returns null, it then tries the parent
	/// </summary>
	public class DelegatingTypeDescriptor : ICustomTypeDescriptor
	{
		public DelegatingTypeDescriptor(ICustomTypeDescriptor parent, ICustomTypeDescriptor child)
		{
			parent_ = parent ?? BlankTypeDescriptor.NullDescriptor;
			child_ = child ?? BlankTypeDescriptor.NullDescriptor;
		}

		readonly ICustomTypeDescriptor parent_;
		readonly ICustomTypeDescriptor child_;

		#region ICustomTypeDescriptor Members

		public AttributeCollection GetAttributes()
		{
			return child_.GetAttributes() ?? parent_.GetAttributes();
		}

		public string GetClassName()
		{
			return child_.GetClassName() ?? parent_.GetClassName();
		}

		public string GetComponentName()
		{
			return child_.GetComponentName() ?? parent_.GetComponentName();
		}

		public TypeConverter GetConverter()
		{
			return child_.GetConverter() ?? parent_.GetConverter();
		}

		public EventDescriptor GetDefaultEvent()
		{
			return child_.GetDefaultEvent() ?? parent_.GetDefaultEvent();
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return child_.GetDefaultProperty() ?? parent_.GetDefaultProperty();
		}

		public object GetEditor(Type editorBaseType)
		{
			return child_.GetEditor(editorBaseType) ?? parent_.GetEditor(editorBaseType);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return child_.GetEvents(attributes) ?? parent_.GetEvents(attributes);
		}

		public EventDescriptorCollection GetEvents()
		{
			return child_.GetEvents() ?? parent_.GetEvents();
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return child_.GetProperties(attributes) ?? parent_.GetProperties(attributes);
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return child_.GetProperties() ?? parent_.GetProperties();
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return child_.GetPropertyOwner(pd) ?? parent_.GetPropertyOwner(pd);
		}

		#endregion
	}
}