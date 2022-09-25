using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace Megahard.Data
{
	[Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[Editor(typeof(Visualization.VisualTypeEditor<Visualization.CollectionVisualizer>), typeof(Visualization.VisualTypeEditor))]
	public class ReadOnlyObservableCollection<T> : Megahard.Collections.ReadOnlyCollection<T>, IObservableCollection
	{
		public ReadOnlyObservableCollection(ObservableCollection<T> list)
			: base(list)
		{
			System.Diagnostics.Debug.Assert(list != null);
			list.CollectionChanged += Wrapped_CollectionChanged;
			list.CollectionChanging += Wrapped_CollectionChanging;
		}

		public ReadOnlyObservableCollection(IEnumerable<T> enumerable) : this(new ObservableCollection<T>(enumerable))
		{
		}

		void Wrapped_CollectionChanging(object sender, CollectionChangeEventArgs<T> e)
		{
			var copy = CollectionChanging;
			if (copy != null)
				copy(this, e);
			var nonGenericCopy = nonGenericColChging_;
			if (nonGenericCopy != null)
				nonGenericCopy(this, e);
		}

		void Wrapped_CollectionChanged(object sender, CollectionChangeEventArgs<T> e)
		{
			var copy = CollectionChanged;
			if (copy != null)
				copy(this, e);
			var nonGenericCopy = nonGenericColChg_;
			if (nonGenericCopy != null)
				nonGenericCopy(this, e);
		}

		public event EventHandler<CollectionChangeEventArgs<T>> CollectionChanged;
		public event EventHandler<CollectionChangeEventArgs<T>> CollectionChanging;

		#region IObservableCollection Members

		EventHandler<CollectionChangeEventArgs> nonGenericColChg_;
		event EventHandler<CollectionChangeEventArgs> IObservableCollection.CollectionChanged
		{
			add { nonGenericColChg_ += value; }
			remove { nonGenericColChg_ -= value; }
		}

		EventHandler<CollectionChangeEventArgs> nonGenericColChging_;
		event EventHandler<CollectionChangeEventArgs> IObservableCollection.CollectionChanging
		{
			add { nonGenericColChging_ += value; }
			remove { nonGenericColChging_ -= value; }
		}

		#endregion
	}
}
