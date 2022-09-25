using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Megahard.ExtensionMethods;
using System.Reflection;
using System.ComponentModel;
namespace Megahard.ComponentModel
{
	/// <summary>
	/// Sets up a TypeDescriptor provider that will only be used when there is a specific instance of the type provided
	/// It includes a nested provider
	/// </summary>
	public class InstanceOfTypeDescriptor<ThisType, DescribedType> : BlankTypeDescriptor where ThisType : InstanceOfTypeDescriptor<ThisType, DescribedType>, new()
	{
		/// <summary>
		/// Guaranteed to be non-null
		/// </summary>
		protected DescribedType Instance
		{
			get;
			private set;
		}

		protected ICustomTypeDescriptor OriginalTypeDescriptor
		{
			get;
			private set;
		}
		protected virtual void Initialize()
		{
		}

		public class Provider : TypeDescriptionProvider
		{
			readonly TypeDescriptionProvider baseProvider_;
			public Provider()
			{
				baseProvider_ = TypeDescriptor.GetProvider(typeof(DescribedType));
			}
			public override System.ComponentModel.ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
			{
				if (instance != null)
				{
					var origDescriptor = baseProvider_.GetTypeDescriptor(objectType, instance);
					var ppp = new ThisType() { Instance = (DescribedType)instance, OriginalTypeDescriptor = origDescriptor };
					ppp.Initialize();
					var ret = new DelegatingTypeDescriptor(origDescriptor, ppp);
					return ret;
				}
				else
				{
					return baseProvider_.GetTypeDescriptor(objectType, instance);
				}
			}
		}
	}
}
