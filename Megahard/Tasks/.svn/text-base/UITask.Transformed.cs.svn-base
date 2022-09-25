
namespace Megahard.Tasks
{
	using System;
	using Category = System.ComponentModel.CategoryAttribute;
	using Megahard.Data;
	partial class UITask
	{
	

		readonly Megahard.Threading.SynchronizedEventBacking ehTaskDeactivated_ = new Megahard.Threading.SynchronizedEventBacking ();
		
		public event EventHandler TaskDeactivated
		{
			add { ehTaskDeactivated_.SynchronizedEvent += value; }
			remove { ehTaskDeactivated_.SynchronizedEvent -= value; }
		}
		partial void PreRaiseTaskDeactivated(EventArgs args, ref bool fireEvent);
		partial void PostRaiseTaskDeactivated(EventArgs args);
		protected virtual void OnTaskDeactivated(EventArgs args) { }
		protected void RaiseTaskDeactivated(EventArgs args)
		{
			bool fireEvent = true;
			PreRaiseTaskDeactivated(args, ref fireEvent);
			if(fireEvent)
			{
				OnTaskDeactivated(args);
				ehTaskDeactivated_.RaiseEvent(this, args);
				PostRaiseTaskDeactivated(args);
			}
		}
	
	}
}
	

namespace Megahard.Tasks
{
	using System;
	using Category = System.ComponentModel.CategoryAttribute;
	using Megahard.Data;
	partial class UITask
	{
	

		readonly Megahard.Threading.SynchronizedEventBacking ehTaskActivated_ = new Megahard.Threading.SynchronizedEventBacking ();
		
		public event EventHandler TaskActivated
		{
			add { ehTaskActivated_.SynchronizedEvent += value; }
			remove { ehTaskActivated_.SynchronizedEvent -= value; }
		}
		partial void PreRaiseTaskActivated(EventArgs args, ref bool fireEvent);
		partial void PostRaiseTaskActivated(EventArgs args);
		protected virtual void OnTaskActivated(EventArgs args) { }
		protected void RaiseTaskActivated(EventArgs args)
		{
			bool fireEvent = true;
			PreRaiseTaskActivated(args, ref fireEvent);
			if(fireEvent)
			{
				OnTaskActivated(args);
				ehTaskActivated_.RaiseEvent(this, args);
				PostRaiseTaskActivated(args);
			}
		}
	
	}
}
	

namespace Megahard.Tasks
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class UITask
	{
	PropertyBacking<string> propTaskName_ = new PropertyBacking<string>("TaskName");
			[DefaultValue("")]
		public  string TaskName
		{
			get { return  propTaskName_.GetValue(); }
			
			
			set 
			{ 
				BeforeSetTaskName(ref value);
				if( propTaskName_.WouldChange(value))
				{
					var chged =  propTaskName_.SetValueNoEqualCheck(this, value);
					AfterTaskNameChanged(chged);
				}
			}
		}
		
		partial void BeforeSetTaskName(ref string incomingValue);
		partial void AfterTaskNameChanged(ObjectChangedEventArgs<string> newVal);
	
	}
}
	
