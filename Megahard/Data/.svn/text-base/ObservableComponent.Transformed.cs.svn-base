
namespace Megahard.Data
{
	using System;
	using Category = System.ComponentModel.CategoryAttribute;
	using Megahard.Data;
	partial class ObservableComponent
	{
	

		readonly Megahard.Threading.SynchronizedEventBacking ehDisposed_ = new Megahard.Threading.SynchronizedEventBacking ();
		
		public event EventHandler Disposed
		{
			add { ehDisposed_.SynchronizedEvent += value; }
			remove { ehDisposed_.SynchronizedEvent -= value; }
		}
		partial void PreRaiseDisposed(EventArgs args, ref bool fireEvent);
		partial void PostRaiseDisposed(EventArgs args);
		protected virtual void OnDisposed(EventArgs args) { }
		protected void RaiseDisposed(EventArgs args)
		{
			bool fireEvent = true;
			PreRaiseDisposed(args, ref fireEvent);
			if(fireEvent)
			{
				OnDisposed(args);
				ehDisposed_.RaiseEvent(this, args);
				PostRaiseDisposed(args);
			}
		}
	
	}
}
	
