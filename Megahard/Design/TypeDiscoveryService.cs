using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.Design;

namespace Megahard.Design
{
	public class TypeDiscoveryService : ITypeDiscoveryService
	{
		protected TypeDiscoveryService(ITypeDiscoveryService parent)
		{
			parent_ = parent;
		}

		readonly ITypeDiscoveryService parent_;
		public virtual ICollection GetTypes(Type baseType, bool excludeGlobalTypes)
		{
			if (parent_ != null)
				return parent_.GetTypes(baseType, excludeGlobalTypes);
			return new Type[0];
		}

	}

}
