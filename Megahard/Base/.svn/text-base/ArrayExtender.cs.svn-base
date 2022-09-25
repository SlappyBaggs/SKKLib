using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class mhArrayExtender
	{
		public static bool MemberWiseEqual<T>(this T[] a, T[] b) where T : IEquatable<T>
		{
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; ++i)
			{
				if(!a[i].Equals(b[i]))
					return false;
			}
			return true;
		}

		public static T[] CopySection<T>(this T[] arr, int offset, int length)
		{
			T[] copy = new T[length];
			Array.Copy(arr, offset, copy, 0, length);
			return copy;
		}

		public static T[] CopySection<T>(this IList<T> arr, int offset, int length)
		{
			T[] copy = new T[length];
			for (int i = 0; i < length; ++i)
			{
				copy[i] = arr[offset + i];
			}
			return copy;
		}

		public static T[] ToArray<T>(this ICollection col) 
		{
			T[] ret = new T[col.Count];
			int i = 0;
			foreach (object o in col)
				ret[i++] = (T)o;
			return ret;
		}


	}

}