
namespace Megahard.Data
{
	using System;
	using Category = System.ComponentModel.CategoryAttribute;
	using Megahard.Data;
	partial class ObservableObject
	{
	

		readonly Megahard.Threading.SynchronizedEventBacking<ObjectChangingEventArgs> ehObjectChanging_ = new Megahard.Threading.SynchronizedEventBacking<ObjectChangingEventArgs> ();
		[Category("Observation")]
		public event EventHandler<ObjectChangingEventArgs> ObjectChanging
		{
			add { ehObjectChanging_.SynchronizedEvent += value; }
			remove { ehObjectChanging_.SynchronizedEvent -= value; }
		}
		partial void PreRaiseObjectChanging(ObjectChangingEventArgs args, ref bool fireEvent);
		partial void PostRaiseObjectChanging(ObjectChangingEventArgs args);
		protected virtual void OnObjectChanging(ObjectChangingEventArgs args) { }
		protected void RaiseObjectChanging(ObjectChangingEventArgs args)
		{
			bool fireEvent = true;
			PreRaiseObjectChanging(args, ref fireEvent);
			if(fireEvent)
			{
				OnObjectChanging(args);
				ehObjectChanging_.RaiseEvent(this, args);
				PostRaiseObjectChanging(args);
			}
		}
	
	}
}
	

namespace Megahard.Data
{
	using System;
	using Category = System.ComponentModel.CategoryAttribute;
	using Megahard.Data;
	partial class ObservableObject
	{
	

		readonly Megahard.Threading.SynchronizedEventBacking<ObjectChangedEventArgs> ehObjectChanged_ = new Megahard.Threading.SynchronizedEventBacking<ObjectChangedEventArgs> ();
		[Category("Observation")]
		public event EventHandler<ObjectChangedEventArgs> ObjectChanged
		{
			add { ehObjectChanged_.SynchronizedEvent += value; }
			remove { ehObjectChanged_.SynchronizedEvent -= value; }
		}
		partial void PreRaiseObjectChanged(ObjectChangedEventArgs args, ref bool fireEvent);
		partial void PostRaiseObjectChanged(ObjectChangedEventArgs args);
		protected virtual void OnObjectChanged(ObjectChangedEventArgs args) { }
		protected void RaiseObjectChanged(ObjectChangedEventArgs args)
		{
			bool fireEvent = true;
			PreRaiseObjectChanged(args, ref fireEvent);
			if(fireEvent)
			{
				OnObjectChanged(args);
				ehObjectChanged_.RaiseEvent(this, args);
				PostRaiseObjectChanged(args);
			}
		}
	
	}
}
	
