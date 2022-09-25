using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class mhStringExtender
	{
		/// <summary>
		/// Executes a Func for each char in the string, until the func returns true
		/// </summary>
		/// <returns>pos of char that eval returned true for, or -1 if eval never returned true</returns>
		public static int FindFirst(this string s, int startPos, Func<Char, bool> eval)
		{
			for (int i = startPos; i < s.Length; ++i)
			{
				if (eval(s[i]))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Reports index of first occurrence of any of the strings passed in
		/// </summary>
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


		/// <summary>
		/// Returns true if the string has any characters in it, ie non null and non empty
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool HasChars(this string s)
		{
			return !string.IsNullOrEmpty(s);
		}

		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}


		/// <summary>
		/// Returns null if the string is emptpy otherwise returns the given string
		/// </summary>
		public static string NullIfEmpty(this string s)
		{
			return s == string.Empty ? null : s;
		}

		/// <summary>
		/// returns empty string is s is null, otherwise s
		/// </summary>
		public static string EmptyIfNull(this string s)
		{
			return s ?? string.Empty;
		}
	}
}