using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Megahard.Collections
{
	/// <summary>
	/// Creates a wrapper around the supplied list to expose only the range of the list specified
	/// </summary>
	public sealed class ListSlice<T> : Collection<T>
	{
		public ListSlice(IList<T> list, int startIndex, int length)
			: base(list)
		{
			// Base will create an empty array if list is null which is why i used items property even in the ctor
			if (startIndex < 0)
				startIndex += Items.Count;
			if (startIndex < 0 || (startIndex + length) > Items.Count || length < 0)
				throw new InvalidOperationException(string.Format("ListSlice: Bad start({0}) or length({1}) parameters", startIndex, length));
			start_ = startIndex;
			length_ = length;
		}

		readonly int start_;
		readonly int length_;

		public int Start
		{
			get { return start_; }
		}

		public override int IndexOf(T item)
		{
			if (item == null)
			{
				for (int i = 0; i < Count; ++i)
				{
					if (this[i] == null)
						return i;
				}
				return -1;
			}

			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < Count; ++i)
			{
				if (comparer.Equals(this[i], item))
					return i;
			}
			return -1;
		}

		public override T this[int key]
		{
			get
			{
				return base[ModifyKey(key)];
			}
			set
			{
				base[ModifyKey(key)] = value;
			}
		}

		public override bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			foreach (T item in this)
				array[arrayIndex++] = item;
		}
		public override int Count
		{
			get
			{
				return length_;
			}
		}

		public override bool IsFixedSize
		{
			get
			{
				return true;
			}
		}
		public override IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Count; ++i)
				yield return this[i];
		}

		int ModifyKey(int key)
		{
			if (key < 0 || key >= (Count))
				throw new ArgumentOutOfRangeException("key", key, "ListSlice key is not in the slice range");
			int newKey = key + start_;
			return newKey;
		}
	}
}
