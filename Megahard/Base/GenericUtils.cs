using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Reflection
{
	public static class GenericUtils
	{
		public static string DecodeGenericName(string name)
		{
			int backTickPos = name.IndexOf('`');
			return backTickPos == -1 ? name : name.Substring(0, backTickPos);
		}
	}
}
