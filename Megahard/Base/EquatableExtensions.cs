using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class EquatableExtensions
	{
		public static bool GreaterThan<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) > 0;
		}

		public static bool LessThan<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) < 0;
		}

		public static bool GreaterThanOrEqual<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool LessThanOrEqual<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool Equal<T>(this T left, T right) where T : IComparable<T>
		{
			return left.CompareTo(right) == 0;
		}
		
		public static bool Between<T>(this T target, T start, T end) where T : IComparable
		{
			if (start.CompareTo(end) == 1)
				return (target.CompareTo(end) >= 0) && (target.CompareTo(start) <= 0);

			return (target.CompareTo(start) >= 0) && (target.CompareTo(end) <= 0);
		}  


		public static T Clamp<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			if (lower.GreaterThanOrEqual(value))
				return lower;
			if (upper.LessThanOrEqual(value))
				return upper;
			return value;
		}
	}
}
