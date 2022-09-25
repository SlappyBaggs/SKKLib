using System;
using System.Windows.Forms;
using System.ComponentModel;
namespace Megahard.Data
{
	public interface IObservableObject: INotifyPropertyChanged, INotifyPropertyChanging
	{
		event EventHandler<ObjectChangedEventArgs> ObjectChanged;
		event EventHandler<ObjectChangingEventArgs> ObjectChanging;

		AttachedObserver AttachObserver(PropertyPath prop, Action<ObjectChangedEventArgs> callBack, Action<ObjectChangingEventArgs> changingCallback);
	}

	public interface IObservableCollection
	{
		event EventHandler<CollectionChangeEventArgs> CollectionChanged;
		event EventHandler<CollectionChangeEventArgs> CollectionChanging;
	}

	public interface IEditableCollection
	{

	}
}
