using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Megahard.Debug
{
	public static class CompileTimeAssert
	{
		[Conditional("DEBUG")]
		public static void IsValueType<T>() where T : struct { }
		
		[Conditional("DEBUG")]
		public static void IsReferenceType<T>() where T : class { }
	}
}
