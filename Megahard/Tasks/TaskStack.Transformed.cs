
namespace Megahard.Tasks
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class TaskStack
	{
	
		readonly PropertyBacking<Task> propActiveTask_ = new PropertyBacking<Task>("ActiveTask");
			
		public Task ActiveTask
		{
			get { return  propActiveTask_.GetValue(); }
			
			private
			set 
			{ 
				if( propActiveTask_.WouldChange(value))
				{
					var chged =  propActiveTask_.SetValueNoEqualCheck(this, value);
					AfterActiveTaskChanged(chged);
				}
			}
		}
		partial void AfterActiveTaskChanged(ObjectChangedEventArgs<Task> newVal);
	
	}
}
	
