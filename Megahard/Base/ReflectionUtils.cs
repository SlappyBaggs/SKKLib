﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Megahard.Reflection
{
	public static class ReflectionUtils
	{
		/// <summary>
		/// Expands a namespace into a list
		/// </summary>
		/// <param name="s">Namespace: e.g. System.Reflection</param>
		/// <returns></returns>
		public static List<string> ExpandNameSpace(string s)
		{
			List<string> ret = new List<string>();
			int pos = s.IndexOf(".");
			while (pos != -1)
			{
				string n = s.Substring(0, pos);
				ret.Add(n);
				s = s.Substring(pos + 1);
				pos = s.IndexOf(".");
			}
			ret.Add(s);
			return ret;
		}
	}
}