using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Megahard
{
	public class Comparer<T> : IComparer<T>
	{
		private readonly Func<T, T, int> func_;
		public Comparer(Func<T, T, int> compareFunc)
		{
			func_ = compareFunc;
		}
		#region IComparer<T> Members

		public int Compare(T x, T y)
		{
			return func_(x, y);
		}

		#endregion
	}

	public class Comparer : Comparer<object>, IComparer
	{
		public Comparer(Func<object, object, int> func) : base(func) { }

	}
}