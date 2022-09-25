using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Megahard.Collections
{
	/// <summary>
	/// This exception is thrown in case where there index of item was requested from a collection
	/// and the item was not in the collection, usually employed in collectiosn which cannot support
	/// returning a special index value to indicate that the item is not a member
	/// </summary>
	public class ItemNotInCollectionException : Exception
	{
		public ItemNotInCollectionException(object item) : base("Item not in collection")
		{
			propItem_ = item;
		}
		#region object Item { get; readonly; }
		readonly object propItem_;
		public object Item
		{
			get { return propItem_; }
		}
		#endregion

	}

	public class Collection<T> : IList<T>, IIndexable<T, int>, System.Collections.IList
	{
		public Collection()
		{
			items_ = new List<T>();
		}

		public Collection(IList<T> listToWrap)
		{
			items_ = listToWrap ?? new List<T>();
		}
		readonly IList<T> items_;


		protected virtual void OnNotSupported(string msg)
		{
			throw new NotSupportedException(msg);
		}

		virtual public IList<T> Slice(int start, int length)
		{
			return new ListSlice<T>(Items, start, length);
		}
		
		[NonSerialized]
		object syncRoot_;
		protected IList<T> Items
		{
			get { return items_; }
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
			if (!Collection<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("Megahard.Collections.Collection<T> - Wrong Type", "value");
			}
		}

		#region IList<T> Members
		public virtual T this[int index]
		{
			get { return items_[index]; }
			set 
			{
				if (IsReadOnly)
					OnNotSupported("this[] - set");
				items_[index] = value; 
			}
		}

		public virtual int IndexOf(T item)
		{
			return items_.IndexOf(item);
		}

		public virtual void Insert(int index, T item)
		{
			if (IsReadOnly || IsFixedSize)
				OnNotSupported("Insert");

			items_.Insert(index, item);
		}

		public virtual void RemoveAt(int index)
		{
			if (IsReadOnly || IsFixedSize)
				OnNotSupported("RemoveAt");
			items_.RemoveAt(index);
		}

		#endregion

		#region ICollection<T> Members

		public virtual void Add(T item)
		{
			if (IsReadOnly || IsFixedSize)
				OnNotSupported("Add");

			items_.Add(item);
		}

		public virtual void Clear()
		{
			if (IsReadOnly || IsFixedSize)
				OnNotSupported("Clear");

			items_.Clear();
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

		public virtual bool IsReadOnly
		{
			get { return items_.IsReadOnly && !(items_ is T[]); }
		}

		public virtual bool IsFixedSize
		{
			get
			{
				System.Collections.IList items = items_ as System.Collections.IList;
				return items != null && items.IsFixedSize;
			}
		}

		public virtual bool Remove(T item)
		{
			return items_.Remove(item);
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
			return this.GetEnumerator();
		}

		#endregion

		#region IReadIndexable<T,int> Members


		int IReadIndexable<T, int>.Length
		{
			get { return this.Count; }
		}

		#endregion

		#region IList Members

		int System.Collections.IList.Add(object value)
		{
			VerifyValueType(value);
			this.Add((T)value);
			return this.Count - 1;
		}

		void System.Collections.IList.Clear()
		{
			this.Clear();
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
			VerifyValueType(value);
			this.Insert(index, (T)value);
		}

		bool System.Collections.IList.IsFixedSize
		{
			get
			{
				return IsFixedSize;
			}
		}


		void System.Collections.IList.Remove(object value)
		{
			VerifyValueType(value);
			this.Remove((T)value);
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				VerifyValueType(value);
				this[index] = (T)value;
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
					throw new ArgumentException("Megahard.Collections.Collection.CopyTo - Invalid array type", "array");
				for (int i = 0; i < Count; ++i)
					obArray[index++] = this[i];
			}
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
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
	}

}
