using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class mhStringExtender
	{
		public static int FindFirst(this string s, int startPos, Func<Char, bool> eval)
		{
			for (int i = startPos; i < s.Length; ++i)
			{
				if (eval(s[i]))
					return i;
			}
			return -1;
		}
		public static int IndexOf(this string me, params string[] strings)
		{
			foreach (string s in strings)
			{
				int i = me.IndexOf(s);
				if (i != -1)
					return i;
			}
			return -1;
		}
		public static bool HasChars(this string s)
		{
			return !string.IsNullOrEmpty(s);
		}

		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}
		public static string NullIfEmpty(this string s)
		{
			return s == string.Empty ? null : s;
		}
		public static string EmptyIfNull(this string s)
		{
			return s ?? string.Empty;
		}
	}
}