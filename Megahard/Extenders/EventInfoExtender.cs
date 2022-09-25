using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection
{
	public static class mhEventInfoExtender
	{
		public static Type GetExplicitlyImplementedInterface(this EventInfo ei)
		{
			try
			{
				var mit = ei.DeclaringType.GetExplicitInterfaceMethods(true).First(x => x.Item1 == ei.GetAddMethod(true));
				return mit.Item2;
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}
	}
}
