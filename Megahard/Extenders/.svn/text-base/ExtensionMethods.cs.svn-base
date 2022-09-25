using System;
namespace System.ComponentModel
{

	public static class mhITypeDescriptorContextExtender
	{
		public static ServiceType GetService<ServiceType>(this ITypeDescriptorContext c) where ServiceType : class
		{
			return c.GetService(typeof(ServiceType)) as ServiceType;
		}
	}

	public static class mhAttributeCollectionExtender
	{
		public static T GetAttribute<T>(this AttributeCollection attrs) where T : Attribute
		{
			return attrs[typeof(T)] as T;
		}
	}

	public static class mhPropertyDesriptorExtender
	{
		public static PropertyDescriptor ChangePropertyType(this PropertyDescriptor prop, Type newType)
		{
			return new Megahard.ComponentModel.ChangePropertyType(prop, newType);
		}
	}
}

// A lot of old code has using Megahard.ExtensionMethods in it, so i dont have to go remove them all, i include the namespace here 
namespace Megahard.ExtensionMethods
{
}
