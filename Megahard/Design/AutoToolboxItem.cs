using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace Megahard.Design
{
	class AutoToolboxItem : TypeDiscoveryService
	{
		public AutoToolboxItem(ITypeDiscoveryService parent) : base(parent) { }
		static AutoToolboxItem()
		{
			s_toolstripItemType = typeof(System.Windows.Forms.ToolStripItem);
		}
		static readonly Type s_toolstripItemType;

		public override System.Collections.ICollection GetTypes(Type baseType, bool excludeGlobalTypes)
		{
			var ret = base.GetTypes(baseType, excludeGlobalTypes);
			if (baseType != s_toolstripItemType)
				return ret;
			throw new NotImplementedException("not done yet");
			//return new Type[] { typeof(Controls.ToolStripLED) };
		}
	}
}
