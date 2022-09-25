using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection
{
	public static class mhMemberInfoExtender
	{
#if WTF
		public static T GetCustomAttribute<T>(this MemberInfo mi, bool inherit) where T : Attribute
		{
			var attrs = mi.GetCustomAttributes(typeof(T), inherit);
			if (attrs.Length > 0)
				return attrs[0] as T;
			return null;
		}
		public static string GetDisplayName(this MemberInfo mi)
		{
			var dispName = GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>(mi, false);
			if (dispName == null)
				return mi.Name;
			return dispName.DisplayName;
		}
#endif
	}
	public static class mhPropertyInfoExtender
	{

	}
}
