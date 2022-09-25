using System;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
	public static class mhGenericCollectionsExtensionMethods
	{
		public static IList<T> ConvertTo<F, T>(this List<F> l) where F : T 
		{
			List<T> ret = new List<T>(l.Count);
			foreach (F f in l)
				ret.Add(f);
			return ret;
		}

		public static Megahard.Collections.ListSlice<T> Slice<T>(this IList<T> l, int startIndex, int length)
		{
			return new Megahard.Collections.ListSlice<T>(l, startIndex, length);
		}
		public static Megahard.Collections.ListSlice<T> Slice<T>(this IList<T> l, int startIndex)
		{
			int length = startIndex < 0 ? -startIndex : l.Count - startIndex;
			return new Megahard.Collections.ListSlice<T>(l, startIndex, length);
		}

		public static int FindIndex<T>(this IList<T> l, Predicate<T> matchCheck)
		{
			for (int i = 0; i < l.Count; ++i)
			{
				if (matchCheck(l[i]))
					return i;
			}
			return -1;
		}

		public static Megahard.Collections.ImmutableArray<T> ToImmutable<T>(this IEnumerable<T> e)
		{
			return new Megahard.Collections.ImmutableArray<T>(e);
		}

		public static bool IsImmutable<T>(this IEnumerable<T> e)
		{
			return e is Megahard.Collections.ImmutableArray<T>;
		}

		public static short ToInt16(this IList<byte> bytes, int pos)
		{
			byte[] arr = new byte[2];
			arr[0] = bytes[pos];
			arr[1] = bytes[pos + 1];
			return BitConverter.ToInt16(arr, 0);
		}
		public static ushort ToUInt16(this IList<byte> bytes, int pos)
		{
			byte[] arr = new byte[2];
			arr[0] = bytes[pos];
			arr[1] = bytes[pos + 1];
			return BitConverter.ToUInt16(arr, 0);
		}
		public static int ToInt32(this IList<byte> bytes, int pos)
		{
			byte[] arr = new byte[4];
			arr[0] = bytes[pos];
			arr[1] = bytes[pos + 1];
			arr[2] = bytes[pos + 2];
			arr[3] = bytes[pos + 3];
			return BitConverter.ToInt32(arr, 0);
		}
		public static uint ToUInt32(this IList<byte> bytes, int pos)
		{
			byte[] arr = new byte[4];
			arr[0] = bytes[pos];
			arr[1] = bytes[pos + 1];
			arr[2] = bytes[pos + 2];
			arr[3] = bytes[pos + 3];
			return BitConverter.ToUInt32(arr, 0);
		}
	}
}

namespace System.Collections
{
	public static class mhCollectionExtensionMethods
	{
		public static T[] ToArray<T>(ICollection col)
		{
			if (col == null)
				return new T[0];
			T[] ret = new T[col.Count];
			col.CopyTo(ret, 0);
			return ret;
		}
	}
}

namespace System.Drawing
{
}

