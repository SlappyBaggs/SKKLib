using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Megahard.ComponentModel
{

	/// <summary>
	/// Exposes an indexed property as a PropertyDescriptor
	/// The set/get value will only work if you bind in the specific index parameters at the time of creation
	/// </summary>
	class IndexPropertyDescriptor : PropertyDescriptor
	{
		readonly PropertyInfo propInfo_;
		readonly object[] indexArgs_;
		public IndexPropertyDescriptor(PropertyInfo propInfo) : base(propInfo.Name, (from attr in propInfo.GetCustomAttributes(true) select (Attribute)attr).ToArray())
		{
			System.Diagnostics.Debug.Assert(propInfo != null);
			propInfo_ = propInfo;
		}
		public IndexPropertyDescriptor(PropertyInfo propInfo, object[] indexArgs)
			: this(propInfo)
		{
			indexArgs_ = indexArgs;
		}
		
		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override Type ComponentType
		{
			get { return propInfo_.DeclaringType; }
		}

		public override object GetValue(object component)
		{
			return GetValue(component, indexArgs_);
		}

		public object GetValue(object component, params object[] indexArgs)
		{
			if (indexArgs == null)
				throw new ArgumentNullException("indexArgs");
			return propInfo_.GetValue(component, indexArgs);
		}

		public override bool IsReadOnly
		{
			get { return propInfo_.CanWrite; }
		}

		public override Type PropertyType
		{
			get { return propInfo_.PropertyType; }
		}

		public override void ResetValue(object component)
		{
			
		}

		public override void SetValue(object component, object value)
		{
			SetValue(component, value, indexArgs_);
		}

		public void SetValue(object component, object value, params object[] indexArgs)
		{
			if (indexArgs_ == null)
				throw new ArgumentNullException("indexArgs");
			propInfo_.SetValue(component, value, indexArgs);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}
}
