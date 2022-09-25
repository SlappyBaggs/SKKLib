using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Megahard.Data
{
	class TypeDescriptorContext : ITypeDescriptorContext
	{
		public TypeDescriptorContext(IContainer cont, object instance, PropertyDescriptor pd, IServiceProvider provider)
		{
			Container = cont;
			Instance = instance;
			provider_ = provider;
			PropertyDescriptor = pd;
		}

		readonly IServiceProvider provider_;
		#region ITypeDescriptorContext Members

		public System.ComponentModel.IContainer Container { get; private set; }
		public object Instance { get; private set; }
		public System.ComponentModel.PropertyDescriptor PropertyDescriptor { get; private set; }

		public void OnComponentChanged()
		{

		}

		public bool OnComponentChanging()
		{
			return true;
		}
		#endregion

		#region IServiceProvider Members

		public object GetService(Type serviceType)
		{
			if (provider_ != null)
				return provider_.GetService(serviceType);
			return null;
		}

		#endregion
	}
}
