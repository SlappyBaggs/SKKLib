﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace Megahard.Data
{
	/// <summary>
	/// The ObservableCollection also implements IBindingList for convenience and interoperability with standard winforms binding
	/// </summary>
	/// 
	/// <typeparam name="T"></typeparam>
	[Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[Editor(typeof(Visualization.VisualTypeEditor<Visualization.CollectionVisualizer>), typeof(Visualization.VisualTypeEditor))]
	public class ObservableCollection<T> : ObservableObject, System.Collections.Generic.IList<T>, System.Collections.IList, Megahard.Collections.IIndexable<T, int>, IObservableCollection, IBindingList, ICloneable
	{
		private readonly List<T> list_;
		private readonly IEventExecutor eventExec_;
		static readonly System.Reflection.ConstructorInfo ctor_ = typeof(T).GetConstructor(System.Type.EmptyTypes);

		private readonly Threading.SynchronizedEventBacking<CollectionChangeEventArgs<T>> collectionChanged_ = new Megahard.Threading.SynchronizedEventBacking<CollectionChangeEventArgs<T>>();
		public event EventHandler<CollectionChangeEventArgs<T>> CollectionChanged
		{
			add { collectionChanged_.SynchronizedEvent += value; }
			remove { collectionChanged_.SynchronizedEvent -= value; }
		}

		private readonly Threading.SynchronizedEventBacking<CollectionChangeEventArgs<T>> collectionChanging_ = new Megahard.Threading.SynchronizedEventBacking<CollectionChangeEventArgs<T>>();
		public event EventHandler<CollectionChangeEventArgs<T>> CollectionChanging
		{
			add { collectionChanging_.SynchronizedEvent += value; }
			remove { collectionChanging_.SynchronizedEvent -= value; }
		}

		protected virtual void OnCollectionChanged(CollectionChangeEventArgs<T> args)
		{
			if (!base.CanRaiseChangeEvents)
				return;
			collectionChanged_.RaiseEvent(this, args, eventExec_);
			var ifaceColChged = ifaceColChged_;
			if (ifaceColChged != null)
				ifaceColChged(this, args);
			var listChg = listChgEvent_;
			if (listChg != null)
			{
				ListChangedEventArgs lcArgs = null;
				switch (args.ChangeType)
				{
					case CollectionChangeType.Reset:
						lcArgs = new ListChangedEventArgs(ListChangedType.Reset, -1);
						break;
					case CollectionChangeType.ItemAdded:
						lcArgs = new ListChangedEventArgs(ListChangedType.ItemAdded, args.Index);
						break;
					case CollectionChangeType.ItemChanged:
						lcArgs = new ListChangedEventArgs(ListChangedType.ItemChanged, args.Index);
						break;
					case CollectionChangeType.ItemPropertyChanged:
						int index = IndexOf(args.Item);
						lcArgs = new ListChangedEventArgs(ListChangedType.ItemChanged, index,  TypeDescriptor.GetProperties(args.Item).Find(args.Property.Root, true));
						break;
					case CollectionChangeType.ItemRemoved:
						lcArgs = new ListChangedEventArgs(ListChangedType.ItemDeleted, args.Index);
						break;
				}
				if (lcArgs != null)
					listChg(this, lcArgs);
			}
		}

		/// <summary>
		/// If this returns true then the operation that caused the change is canceled when possible
		/// </summary>
		protected virtual bool OnCollectionChanging(CollectionChangeEventArgs<T> args)
		{
			if (base.CanRaiseChangeEvents)
			{
				collectionChanging_.RaiseEvent(this, args, eventExec_);
				var ifaceColChging = ifaceColChging_;
				if (ifaceColChging != null)
					ifaceColChging(this, args);
			}
			return false;
		}


		/// <summary>
		/// Creates the collection and populates it with given enumerable
		/// </summary>
		public ObservableCollection(IEnumerable<T> enumerable) : this()
		{
			list_.AddRange(enumerable);
			foreach (T ob in enumerable)
				WatchItem(ob);
		}

		public ObservableCollection()
		{
			list_ = new List<T>();
			eventExec_ = EventExecutor.DefaultSync;

		}
		public ObservableCollection(IEventExecutor exec)
		{
			if (exec == null)
				eventExec_ = EventExecutor.DefaultSync;
			else
				eventExec_ = exec;
		
			list_ = new List<T>();
		}

		/// <summary>
		/// Initial capacity of the list storage
		/// </summary>word
		/// <param name="initialCapacity"></param>
		public ObservableCollection(int initialCapacity, IEventExecutor exec)
		{
			if (exec == null)
				eventExec_ = EventExecutor.DefaultSync;
			else
				eventExec_ = exec;

			list_ = new List<T>(initialCapacity);
		}
		
		private void WatchItem(T item)
		{
			IObservableObject observable = item as IObservableObject;
			if(observable != null)
			{
				observable.ObjectChanged += item_ObjectChanged;
				observable.ObjectChanging += item_ObjectChanging;
			}
			else if(item is INotifyPropertyChanged)
			{
				(item as INotifyPropertyChanged).PropertyChanged += item_PropertyChanged;
			}
		}

		private void StopWatchingItem(T item)
		{
			IObservableObject observable = item as IObservableObject;
			if (observable != null)
			{
				observable.ObjectChanged -= item_ObjectChanged;
				observable.ObjectChanging -= item_ObjectChanging;
			}
			else if (item is INotifyPropertyChanged)
			{
				(item as INotifyPropertyChanged).PropertyChanged -= item_PropertyChanged;
			}
		}

		void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is T)
			{
				OnCollectionChanged(new CollectionChangeEventArgs<T>((T)sender, e.PropertyName));
				base.RaiseObjectChanged(new ObjectChangedEventArgs("[" + IndexOf((T)sender) + "]." + e.PropertyName));
			}
		}

		void item_ObjectChanging(object sender, ObjectChangingEventArgs e)
		{
			if (sender is T)
			{
				OnCollectionChanging(new CollectionChangeEventArgs<T>((T)sender, e.PropertyName));
				base.RaiseObjectChanging(new ObjectChangingEventArgs("[" + IndexOf((T)sender) + "]." + e.PropertyName, e.NewValue));
			}
		}
		
		void item_ObjectChanged(object sender, ObjectChangedEventArgs e)
		{
			if (sender is T)
			{
				OnCollectionChanged(new CollectionChangeEventArgs<T>((T)sender, e.PropertyName));
				base.RaiseObjectChanged(new ObjectChangedEventArgs("[" + IndexOf((T)sender) + "]." + e.PropertyName, e.OldValue, e.NewValue));
			}
		}

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return list_.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			var oldCount = Count;
			CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.ItemAdded, index, item);
			if (OnCollectionChanging(colChg))
				return;
			base.RaiseObjectChanging(new ObjectChangingEventArgs("Count", oldCount + 1));
			list_.Insert(index, item);
			WatchItem(item);
			OnCollectionChanged(colChg);
			base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Count", oldCount, Count));
			base.RaiseObjectChanged(new ObjectChangedEventArgs<T>("[" + index + "]", default(T), item));
		}

		public void RemoveAt(int index)
		{
			var oldCount = Count;
			T item = list_[index];
			CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.ItemRemoved, index, item);
			if (OnCollectionChanging(colChg))
				return;
			base.RaiseObjectChanging(new ObjectChangingEventArgs("Count", oldCount - 1));
			list_.RemoveAt(index);
			StopWatchingItem(item);
			OnCollectionChanged(colChg);
			base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Count", oldCount, Count));
		}

		public T this[int index]
		{
			get
			{
				return list_[index];
			}
			set
			{
				if (!EqualityComparer<T>.Default.Equals(list_[index], value))
				{
					CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.ItemChanged, index, value);
					if (OnCollectionChanging(colChg))
						return;
					StopWatchingItem(list_[index]);
					list_[index] = value;
					WatchItem(value);
					OnCollectionChanged(colChg);
				}
			}
		}

		#endregion

		#region ICollection<T> Members
		public void Add(T item)
		{
			Add(item, true);
		}

		void Add(T item, bool fireObjectChanged)
		{
			var oldCount = Count;
			//CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.ItemAdded, list_.Count - 1, item);
			CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.ItemAdded, list_.Count, item);
			if (OnCollectionChanging(colChg))
				return;
			if (fireObjectChanged)
				base.RaiseObjectChanging(new ObjectChangingEventArgs("Count", oldCount + 1));
			list_.Add(item);
			WatchItem(item);
			OnCollectionChanged(colChg);
			if(fireObjectChanged)
				base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Count", oldCount, Count));
		}

		public void AddRange(IEnumerable<T> items)
		{
			var oldCount = Count;
			CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.Reset, -1, default(T));
			if (OnCollectionChanging(colChg))
				return;
			foreach (T item in items)
			{
				Add(item, false);
			}
			OnCollectionChanged(colChg);
			base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Count", oldCount, Count));
		}

		public void Clear()
		{
			if (Count == 0)
				return;
			CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.Reset, -1, default(T));
			if (OnCollectionChanging(colChg))
				return;
			var oldCount = Count;
			foreach (T item in list_)
			{
				StopWatchingItem(item);
			}
			list_.Clear();
			OnCollectionChanged(colChg);
			base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Count", oldCount, Count));
		}

		public bool Contains(T item)
		{
			return list_.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			list_.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return list_.Count; }
		}

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			var oldCount = Count;
			int index = list_.IndexOf(item);
			if (index == -1)
				return false;

			CollectionChangeEventArgs<T> colChg = new CollectionChangeEventArgs<T>(CollectionChangeType.ItemRemoved, index, item);
			if (OnCollectionChanging(colChg))
				return false;
			bool b = list_.Remove(item);
			if (b)
			{
				StopWatchingItem(item);
				OnCollectionChanged(colChg);
				base.RaiseObjectChanged(new ObjectChangedEventArgs<int>("Count", oldCount, Count));
			}
			return b;
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return list_.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return list_.GetEnumerator();
		}

		#endregion

		#region IList Members

		int IList.Add(object value)
		{
			Add((T)value);
			return Count - 1;
		}

		void IList.Clear()
		{
			Clear();
		}

		bool IList.Contains(object value)
		{
			return (list_ as IList).Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return (list_ as IList).IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (T)value);
		}

		bool IList.IsFixedSize
		{
			get { return false; }
		}

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		void IList.Remove(object value)
		{
			Remove((T)value);
		}

		void IList.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		object IList.this[int index]
		{
			get
			{
				return list_[index];
			}
			set
			{
				this[index] = (T)value;
			}
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			(list_ as IList).CopyTo(array, index);
		}

		int ICollection.Count
		{
			get { return list_.Count; }
		}

		bool ICollection.IsSynchronized
		{
			get { return (list_ as ICollection).IsSynchronized; }
		} 

		object ICollection.SyncRoot
		{
			get { return (list_ as ICollection).SyncRoot; }
		}

		#endregion

		int Collections.IReadIndexable<T, int>.Length { get { return this.Count; } }

		public ReadOnlyObservableCollection<T> AsReadOnly() { return new ReadOnlyObservableCollection<T>(this); }

		#region IBindingList Members

		void IBindingList.AddIndex(PropertyDescriptor property)
		{

		}

		protected virtual T OnAddNew()
		{
			T item = (T)ctor_.Invoke(null);
			return item;
		}

		public T AddNew()
		{
			if (!AllowNew)
				throw new NotSupportedException();
			T item = OnAddNew();
			Add(item);
			return item;
		}

		object IBindingList.AddNew()
		{
			return AddNew();
		}

		bool IBindingList.AllowEdit
		{
			get { return true; }
		}

		public virtual bool AllowNew
		{
			get { return ctor_ != null; }
		}

		bool IBindingList.AllowRemove
		{
			get { return true; }
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		bool IBindingList.IsSorted
		{
			get { throw new NotSupportedException(); }
		}

		event ListChangedEventHandler IBindingList.ListChanged
		{
			add
			{
				listChgEvent_ += value;
			}
			remove
			{
				listChgEvent_ -= value;
			}
		}

		ListChangedEventHandler listChgEvent_;

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{

		}

		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		ListSortDirection IBindingList.SortDirection
		{
			get { throw new NotSupportedException(); }
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get { throw new NotSupportedException(); }
		}

		bool IBindingList.SupportsChangeNotification
		{
			get { return true; }
		}

		bool IBindingList.SupportsSearching
		{
			get { return false; }
		}

		bool IBindingList.SupportsSorting
		{
			get { return false; }
		}

		#endregion

		EventHandler<CollectionChangeEventArgs> ifaceColChged_;
		event EventHandler<CollectionChangeEventArgs> IObservableCollection.CollectionChanged
		{
			add
			{
				ifaceColChged_ += value;
			}
			remove
			{
				ifaceColChged_ -= value;
			}
		}

		EventHandler<CollectionChangeEventArgs> ifaceColChging_;
		event EventHandler<CollectionChangeEventArgs> IObservableCollection.CollectionChanging
		{
			add
			{
				ifaceColChging_ += value;
			}
			remove
			{
				ifaceColChging_ -= value;
			}
		}


		public ObservableCollection<T> Clone()
		{
			return new ObservableCollection<T>(this);
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}

}