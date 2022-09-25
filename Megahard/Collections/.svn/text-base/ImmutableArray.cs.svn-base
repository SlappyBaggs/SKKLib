using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Collections
{
	public static class ImmutableArray
	{
		public static ImmutableArray<T> Create<T>(IEnumerable<T> enumerable)
		{
			return new ImmutableArray<T>(enumerable);
		}

		/// <summary>
		/// This is not particularly useful but it doesnt hurt anything to expose this as a public function
		/// It makes unit testing easier and maybe debug scenario? dunno
		/// </summary>
		public static bool AreInternalListsEqual<T>(ImmutableArray<T> arr1, ImmutableArray<T> arr2)
		{
			return ImmutableArray<T>.AreInternalListsEqual(arr1, arr2);
		}

		public static EmptyArray Empty
		{
			get { return new EmptyArray(); }
		}
	}

	public struct EmptyArray 
	{ 
	}

    public partial class ImmutableArray<T> : ReadOnlyCollection<T>, ICloneable
    {

		public static implicit operator ImmutableArray<T>(EmptyArray empty)
		{
			return Empty;
		}

		public static implicit operator ImmutableArray<T>(T[] arr)
		{
			if (arr == null)
				return Empty;
			return new ImmutableArray<T>(arr, true);
		}

		#region Static Helpers
		static IList<T> Copy(ICollection<T> list)
		{
			if (list is ImmutableArray<T>)
			{
				return (list as ImmutableArray<T>).Items;
			}

			if(list == null || list.Count == 0)
				return s_Empty;
			var arr = new T[list.Count];
			list.CopyTo(arr, 0);
			return arr;
		}

		static IList<T> Copy(IEnumerable<T> e, bool forceTrueArray)
		{
			if (e is ICollection<T>)
			{
				var items = Copy(e as ICollection<T>);
				if (!forceTrueArray || items is T[])
					return items;
				return items.ToArray();
			}

			if (e == null)
				return s_Empty;

			int count = e.Count();
			if(count == 0)
				return s_Empty;
			T[] arr = new T[count];

			int pos = 0;
			foreach (T elem in e)
				arr[pos++] = elem;
			return arr;
		}
		#endregion
		public ImmutableArray()
			: base(s_Empty)
		{
		}

		public ImmutableArray(IEnumerable<T> enumerable) : this(enumerable, false)
        {
        }

		protected ImmutableArray(IEnumerable<T> enumerable, bool forceTrueArray) : base(Copy(enumerable, forceTrueArray))
		{
			System.Diagnostics.Debug.Assert(!forceTrueArray || Items is T[]);
		}

		protected ImmutableArray(Builder builder) : this(builder.ToList())
		{
		}

        /// <summary>
        /// Always keep private, so only this class can directly set the list member
        /// </summary>
		ImmutableArray(IList<T> list) : base(list ?? s_Empty)
		{

		}

		internal static bool AreInternalListsEqual(ImmutableArray<T> arr1, ImmutableArray<T> arr2)
		{
			if (arr1 == null || arr2 == null)
				return false;
			return object.ReferenceEquals(arr1.Items, arr2.Items);
		}

		public ImmutableArray<T> Slice(int start)
		{
			int length = start < 0 ? -start : Length - start;
			return new ImmutableArray<T>(base.SliceCore(start, length));
		}

		public new ImmutableArray<T> Slice(int start, int length)
		{
			return new ImmutableArray<T>(base.SliceCore(start, length));
		}
		protected override IList<T>  SliceCore(int start, int length)
		{
			return this.Slice(start, length);
		}

		public static Builder Build()
		{
			return new Builder(0);
		}

		public static Builder Build(int initialCapacity)
		{
			return new Builder(initialCapacity);
		}

		static T[] s_Empty = new T[0];

		static ImmutableArray<T> s_EmptyImmutable = new ImmutableArray<T>();

        public bool MemberWiseEqual(ImmutableArray<T> compareTo)
        {
            if (object.ReferenceEquals(Items, compareTo.Items))
                return true;
            return MemberWiseEqual(compareTo as IList<T>);
        }

		public bool MemberWiseEqual(IList<T> compareTo)
		{
			if (Length != compareTo.Count)
				return false;
			for (int i = 0; i < Length; ++i)
			{
				if (!this[i].Equals(compareTo[i]))
					return false;
			}
			return true;
		}

		public ImmutableArray<T> AsImmutable()
		{
			return this;
		}

		/// <summary>
		/// Synonym for count
		/// </summary>
		public int Length
		{
			get { return Count; }
		}


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override int Count
		{
			get
			{
				return base.Count;
			}
		}

        public bool IsEmpty
        {
			get { return Count == 0; }
        }

		object ICloneable.Clone()
        {
            return this;
        }

		public static bool IsNullOrEmpty(ImmutableArray<T> arr)
		{
			if (arr == null)
				return true;
			return arr.IsEmpty;
		}

		public static ImmutableArray<T> Concat(ImmutableArray<T> arr1, ImmutableArray<T> arr2)
		{
			if (IsNullOrEmpty(arr1))
				return arr2 ?? s_EmptyImmutable;

			if (IsNullOrEmpty(arr2))
				return arr1 ?? s_EmptyImmutable;
			// Todo: There is some room to optimize here, but alas for another day
			var bb = Build(arr1.Length + arr2.Length);
			bb.Add(arr1).Add(arr2);
			return bb.ToArray();
		}

		public static ImmutableArray<T> Empty
		{
			get { return s_EmptyImmutable; }
		}
	}
}
