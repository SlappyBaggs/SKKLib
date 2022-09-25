using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.Data
{
	public abstract class BindingListBase
	{
		readonly Threading.SyncLock locker_;
		protected readonly IBindingList blist_;
		protected readonly IEventExecutor eventExecutor_;

		protected BindingListBase(IBindingList blist, Threading.SyncLock locker, IEventExecutor exec)
		{
			locker_ = locker ?? new Threading.SyncLock();
			blist_ = blist;
			eventExecutor_ = exec;

			using(locker_.Lock())
				blist_.ListChanged += OnListChanged;
		}

		protected abstract void OnListChanged(object sender, ListChangedEventArgs e);

		protected BindingListBase(BindingListBase toWrap) : this(toWrap.blist_, toWrap.locker_, toWrap.eventExecutor_)
		{

		}
		protected IBindingList BaseList { get { return blist_; } }

		protected Threading.LockKey Lock()
		{
			return locker_.Lock();
		}
	}
	/// <summary>
	/// This class is fully thread safe which means yes even iteration is
	/// This is an implementation of IBindingList which is Syncronized, and does not support
	/// modification of the list by external parties, events are fired via IEventExecutor interface
	/// </summary>
	public class BindingList<ListType> : BindingListBase, IBindingList, IEnumerable<ListType>, IRaiseItemChangedEvents
	{

		public BindingList(IEventExecutor exec)
			: base(new System.ComponentModel.BindingList<ListType>(), new Threading.SyncLock(), exec)
		{
		}

		public BindingList(BindingListBase blist) : base(blist)
		{
		}

		protected override void OnListChanged(object sender, ListChangedEventArgs e)
		{
			using (listChgLockOb_.Lock())
			{
				if (listChanged_ == null)
					return;
				eventExecutor_.FireEvent(listChanged_, this, e, null);
			}
		}

	


		protected void Add(ListType item)
		{
			using (Lock())
			{
				blist_.Add(item);
			}
		}

		protected void Clear()
		{
			using (Lock())
			{
				blist_.Clear();
			}
		}

		public int Count
		{
			get
			{
				using (Lock())
				{
					return blist_.Count;
				}
			}
		}

		public ListType this[int index]
		{
			get
			{
				using (Lock())
				{
					return (ListType)blist_[index];
				}
			}
		}


		#region IBindingList Members

		public void AddIndex(PropertyDescriptor property)
		{
			using (Lock())
			{
				BaseList.AddIndex(property);
			}
		}

		public object AddNew()
		{
			throw new NotSupportedException();
		}

		public bool AllowEdit
		{
			get { return false; }
		}

		public bool AllowNew
		{
			get { return false; }
		}

		public bool AllowRemove
		{
			get { return false; }
		}

		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			using (Lock())
			{
				BaseList.ApplySort(property, direction);
			}
		}

		public int Find(PropertyDescriptor property, object key)
		{
			using (Lock())
			{
				return BaseList.Find(property, key);
			}
		}

		public bool IsSorted
		{
			get
			{
				using (Lock())
				{
					return BaseList.IsSorted;
				}
			}
		}

		private ListChangedEventHandler listChanged_;
		const string ListChgLockName = "ListChangeLock";
		private readonly Threading.SyncLock listChgLockOb_ = new Megahard.Threading.SyncLock(ListChgLockName);
		// Provide our own listchanged event, so we can forward the one from bindinglist and change the sender to this class
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				using(listChgLockOb_.Lock())
				{
					listChanged_ += value;
				}
			}

			remove
			{
				using(listChgLockOb_.Lock())
				{
					listChanged_ -= value;
				}
			}
		}

		public void RemoveIndex(PropertyDescriptor property)
		{
			using (Lock())
			{
				BaseList.RemoveIndex(property);
			}
		}

		public void RemoveSort()
		{
			using (Lock())
			{
				BaseList.RemoveSort();
			}
		}

		public ListSortDirection SortDirection
		{
			get
			{
				using (Lock())
				{
					return BaseList.SortDirection;
				}
			}
		}

		public PropertyDescriptor SortProperty
		{
			get
			{
				using (Lock())
				{
					return BaseList.SortProperty;
				}
			}
		}

		public bool SupportsChangeNotification
		{
			get { return true; }
		}

		public bool SupportsSearching
		{
			get { return BaseList.SupportsSearching; }
		}

		public bool SupportsSorting
		{
			get { return BaseList.SupportsSorting; }
		}

		#endregion

		#region IList Members

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		void System.Collections.IList.Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(object value)
		{
			using (Lock())
			{
				return BaseList.Contains(value);
			}
		}

		public int IndexOf(object value)
		{
			using (Lock())
			{
				return BaseList.IndexOf(value);
			}
		}

		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				using (Lock())
				{
					return BaseList[index];
				}
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		#endregion

		public void CopyTo(ListType[] array, int index)
		{
			using (Lock())
				BaseList.CopyTo(array, index);
		}
		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			using (Lock())
				BaseList.CopyTo(array, index);
		}

		public bool IsSynchronized
		{
			get { return true; }
		}

		public object SyncRoot
		{
			get { return null; }
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator IEnumerable.GetEnumerator()
		{
			using (Lock())
			{
				foreach (var mfc in blist_)
					yield return mfc;
			}
		}
		#endregion

		#region IEnumerable<ListType> Members

		IEnumerator<ListType> IEnumerable<ListType>.GetEnumerator()
		{
			using (Lock())
			{
				foreach (ListType lt in blist_)
				{
					yield return lt;
				}
			}
		}

		#endregion


		#region IRaiseItemChangedEvents Members

		bool IRaiseItemChangedEvents.RaisesItemChangedEvents
		{
			get { return true; }
		}

		#endregion
	}
}