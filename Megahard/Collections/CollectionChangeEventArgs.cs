using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace Megahard.Data
{
	public enum CollectionChangeType { ItemAdded, ItemRemoved, ItemChanged, ItemPropertyChanged, Reset }

	public class CollectionChangeEventArgs : EventArgs
	{
		public CollectionChangeEventArgs(CollectionChangeType lct, int index, object item)
		{
			if (lct == CollectionChangeType.ItemPropertyChanged)
				throw new InvalidOperationException("CollectionChange Type cannot be ItemPropertyChanged when using this ctor");
			System.Diagnostics.Debug.Assert(index >= 0 || lct == CollectionChangeType.Reset);
			ChangeType = lct;
			Index = index;
			Item = item;
		}

		public CollectionChangeEventArgs(object item, PropertyPath prop)
		{
			ChangeType = CollectionChangeType.ItemPropertyChanged;
			Index = -1;
			Item = item;
			Property = prop;
		}

		/// <summary>
		/// The name of the property that changed, always null except when ChangeType is ItemPropertyChanged
		/// </summary>
		public PropertyPath Property { get; private set; }


		public CollectionChangeType ChangeType { get; private set; }
		/// <summary>
		/// Meaning of value depends on ChangeType
		/// ItemAdded - index of the newly added item
		/// ItemRemoved - index the item used to be in
		/// ItemChanged - index slot of the item that changed
		/// ItemPropertyChanged - always -1, you can lookup the index of item if u want it, saves a little cpu by not auto setting it
		/// Reset - value is -1 and has no meaning
		/// </summary>
		public int Index { get; private set; }
		public object Item { get; private set; }
	}

	public class CollectionChangeEventArgs<T> : CollectionChangeEventArgs
	{
		public CollectionChangeEventArgs(CollectionChangeType lct, int index, T item) : base(lct, index, item)
		{
			this.Item = item;
		}

		public CollectionChangeEventArgs(T item, PropertyPath prop) : base(item, prop)
		{
			this.Item = item;
		}
		
		/// <summary>
		/// The item the event is happening to, for Reset this is null or 0
		/// </summary>
		public new T Item { get; private set; }
	}
}