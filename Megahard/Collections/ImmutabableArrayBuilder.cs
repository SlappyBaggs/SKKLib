using System.Collections.Generic;
namespace Megahard.Collections
{
	partial class ImmutableArray<T>
	{
		public class Builder : IReadIndexable<T, int>
		{
			const int DefaultInitialCapacity = 8;
			internal Builder(int initialCapacity)
			{
				buildArray_ = new List<T>(initialCapacity <= 0 ? DefaultInitialCapacity : initialCapacity);
			}

			public Builder Add(T item)
			{
				buildArray_.Add(item);
				return this;
			}

			public Builder Add(ImmutableArray<T> arr)
			{
				return AddRange(arr);
			}

			public Builder AddRange(IEnumerable<T> collection)
			{
				buildArray_.AddRange(collection);
				return this;
			}

			public Builder AddRange(T[] arr, int offset, int count)
			{
				for (int i = 0; i < count; ++i)
					buildArray_.Add(arr[offset++]);
				return this;
			}

			public Builder AddRange(IList<T> arr, int offset, int count)
			{
				for (int i = 0; i < count; ++i)
					buildArray_.Add(arr[offset++]);
				return this;
			}


			public ImmutableArray<T> ToArray()
			{
				var arr = buildArray_;
				var ret = new ImmutableArray<T>(this);
				System.Diagnostics.Debug.Assert(object.ReferenceEquals(arr, ret.Items));
				return ret;
			}

			internal List<T> ToList()
			{
				var arr = buildArray_;
				buildArray_ = new List<T>(DefaultInitialCapacity);
				return arr;
			}

			public int Count
			{
				get { return buildArray_.Count; }
			}

			public T this[int i]
			{
				get { return buildArray_[i]; }
			}

			List<T> buildArray_;

			#region IReadIndexable<T,int> Members


			public int IndexOf(T value)
			{
				return buildArray_.IndexOf(value);
			}

			int IReadIndexable<T,int>.Length
			{
				get { return Count; }
			}

			#endregion

			#region IEnumerable<T> Members

			public IEnumerator<T> GetEnumerator()
			{
				return buildArray_.GetEnumerator();
			}

			#endregion

			#region IEnumerable Members

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			#endregion
		}
	}
}
