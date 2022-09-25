using System;
using System.ComponentModel;

namespace Megahard.ComponentModel
{
	class ChangePropertyType : ChainingPropertyDescriptor
	{
		public ChangePropertyType(PropertyDescriptor oldDescriptor, Type newPropType) : base(oldDescriptor)
		{
			_newPropType = newPropType ?? oldDescriptor.PropertyType;
		}
		readonly Type _newPropType;

		public override Type PropertyType
		{
			get
			{
				return _newPropType;
			}
		}
	}
}
