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
		public PropertyPath Property { get; private set; }


		public CollectionChangeType ChangeType { get; private set; }
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
		public new T Item { get; private set; }
	}
}