using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Megahard.Collections
{
	public class ReadOnlyCollection<T> : IList<T>, IReadIndexable<T, int>, System.Collections.IList
	{
		public ReadOnlyCollection(IList<T> collectionToWrap)
		{
			items_ = collectionToWrap ?? new T[0];
		}
		readonly IList<T> items_;

		[NonSerialized]
		object syncRoot_;
		protected IList<T> Items
		{
			get { return items_; }
		}

		protected virtual void OnNotSupported(string msg)
		{
			throw new NotSupportedException(msg);
		}

		private static bool IsCompatibleObject(object value)
		{
			if (!(value is T) && ((value != null) || typeof(T).IsValueType))
			{
				return false;
			}
			return true;
		}

		private static void VerifyValueType(object value)
		{
			if (!ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("Megahard.Collections.ReadOnlyCollection<T> - Wrong Type", "value");
			}
		}

		public IList<T> Slice(int start, int length)
		{
			return SliceCore(start, length);
		}

		protected virtual IList<T> SliceCore(int start, int length)
		{
			return new ListSlice<T>(Items, start, length);
		}

		#region IList<T> Members

		public virtual int IndexOf(T item)
		{
			return items_.IndexOf(item);
		}

		void IList<T>.Insert(int index, T item)
		{
			OnNotSupported("Insert");
		}

		void IList<T>.RemoveAt(int index)
		{
			OnNotSupported("RemoveAt");
		}

		public virtual T this[int index]
		{
			get
			{
				return items_[index];
			}
		}

		T IList<T>.this[int index]
		{
			get { return this[index]; }
			set
			{
				OnNotSupported("Indexer Set");
			}
		}

		#endregion

		#region ICollection<T> Members

		void ICollection<T>.Add(T item)
		{
			OnNotSupported("Add");
		}

		void ICollection<T>.Clear()
		{
			OnNotSupported("Clear");
		}

		public virtual bool Contains(T item)
		{
			return items_.Contains(item);
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			items_.CopyTo(array, arrayIndex);
		}

		public virtual int Count
		{
			get { return items_.Count; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		bool ICollection<T>.Remove(T item)
		{
			OnNotSupported("Remove");
			return false;
		}

		#endregion

		#region IEnumerable<T> Members

		public virtual IEnumerator<T> GetEnumerator()
		{
			return items_.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region IList Members

		int System.Collections.IList.Add(object value)
		{
			OnNotSupported("Add");
			return -1;
		}

		void System.Collections.IList.Clear()
		{
			OnNotSupported("Clear");
		}

		bool System.Collections.IList.Contains(object value)
		{
			VerifyValueType(value);
			return this.Contains((T)value);
		}

		int System.Collections.IList.IndexOf(object value)
		{
			VerifyValueType(value);
			return this.IndexOf((T)value);
		}

		void System.Collections.IList.Insert(int index, object value)
		{
			OnNotSupported("Insert");
		}

		bool System.Collections.IList.IsFixedSize
		{
			get { return true; }
		}

		bool System.Collections.IList.IsReadOnly
		{
			get { return true; }
		}

		void System.Collections.IList.Remove(object value)
		{
			OnNotSupported("Remove");
		}

		void System.Collections.IList.RemoveAt(int index)
		{
			OnNotSupported("RemoveAt");
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				OnNotSupported("Indexer Set");
			}
		}

		#endregion

		#region ICollection Members

		void System.Collections.ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException("array");
			T[] localArray = array as T[];
			if (localArray != null)
			{
				this.CopyTo(localArray, index);
			}
			else
			{
				var elemType = array.GetType().GetElementType();
				object[] obArray = array as object[];
				if (obArray == null || !elemType.IsAssignableFrom(typeof(T)))
					throw new ArgumentException("Megahard.Collections.ReadOnlyCollection.CopyTo - Invalid array type", "array");
				for (int i = 0; i < Count; ++i)
					obArray[index++] = this[i];
			}
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get { return false; }
		}

		object System.Collections.ICollection.SyncRoot
		{
			get
			{
				if (this.syncRoot_ == null)
				{
					var items = this.items_ as System.Collections.ICollection;
					if (items != null)
					{
						this.syncRoot_ = items.SyncRoot;
					}
					else
					{
						System.Threading.Interlocked.CompareExchange(ref this.syncRoot_, new object(), null);
					}
				}
				return syncRoot_;
			}
		}

		#endregion

		int IReadIndexable<T, int>.Length
		{
			get { return Count; }
		}
	}
}
