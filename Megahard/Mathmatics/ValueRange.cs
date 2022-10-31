using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Mathematics
{
	public struct ValueRange<T> where T : struct, IComparable<T>
	{
		public ValueRange(T? lowerBound, T? upperBound)
		{
			propMax_ = upperBound;
			propMin_ = lowerBound;
		}
		#region T LowerBound { get; readonly; }
		readonly T? propMin_;
		public T? LowerBound
		{
			get 
			{ 
				return propMin_; 
			}
		}
		#endregion
		#region T UpperBound { get; readonly; }
		readonly T? propMax_;
		public T? UpperBound
		{
			get 
			{ 
				return propMax_; 
			}
		}
		#endregion

		public override string ToString()
		{
			if (IsInfinite)
				return "(all)";

			var sb = new StringBuilder();
			if (HasLowerBound && HasUpperBound)
			{
				if (LowerBound.Value.CompareTo(UpperBound.Value) == 0)
					sb.Append(LowerBound);
				else
					sb.Append("(").Append(LowerBound).Append(", ").Append(UpperBound).Append(")");
			}
			else if (HasLowerBound)
				sb.Append(">= ").Append(LowerBound);
			else
				sb.Append("<= ").Append(UpperBound);
			return sb.ToString();
		}

		public bool Contains(T val)
		{
			if (HasLowerBound)
			{
				if (LowerBound.Value.CompareTo(val) > 0)
					return false;
			}
			if (HasUpperBound)
			{
				if (UpperBound.Value.CompareTo(val) < 0)
					return false;
			}
			return true;
		}
		public bool HasUpperBound
		{
			get { return propMax_.HasValue; }
		}
		public bool HasLowerBound
		{
			get { return propMin_.HasValue; }
		}
		public bool IsUnbounded
		{
			get { return !HasLowerBound || !HasUpperBound; }
		}

		public bool IsEmpty
		{
			get { return propMin_.HasValue && propMax_.HasValue && (propMin_.Value.CompareTo(propMax_.Value) > 0); }
		}
		public bool IsInfinite
		{
			get { return !HasUpperBound && !HasLowerBound; }
		}
		public bool IsFinite
		{
			get { return IsBounded; }
		}
		public bool IsBounded
		{
			get { return HasUpperBound && HasLowerBound; }
		}

		public static ValueRange<T> Infinite
		{
			get { return new ValueRange<T>(null, null); }
		}
	}

	public static class ValueRange
	{
		public static ValueRange<T> Create<T>(T lower, T upper) where T : struct, IComparable<T>
		{
			return new ValueRange<T>(lower, upper);
		}
	}
}
