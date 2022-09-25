
namespace Megahard.Tasks
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class Task
	{
	
		readonly PropertyBacking<bool> propActivated_ = new PropertyBacking<bool>("Activated", false);
			[DefaultValue(false)]
		public bool Activated
		{
			get { return  propActivated_.GetValue(); }
			
			private
			set 
			{ 
				if( propActivated_.WouldChange(value))
				{
					var chged =  propActivated_.SetValueNoEqualCheck(this, value);
					AfterActivatedChanged(chged);
				}
			}
		}
		partial void AfterActivatedChanged(ObjectChangedEventArgs<bool> newVal);
	
	}
}
	

namespace Megahard.Tasks
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class Task
	{
	
		readonly PropertyBacking<string> propTaskName_ = new PropertyBacking<string>("TaskName", "");
			[DefaultValue("")]
		public string TaskName
		{
			get { return  propTaskName_.GetValue(); }
			
			protected
			set 
			{ 
				if( propTaskName_.WouldChange(value))
				{
					var chged =  propTaskName_.SetValueNoEqualCheck(this, value);
					AfterTaskNameChanged(chged);
				}
			}
		}
		partial void AfterTaskNameChanged(ObjectChangedEventArgs<string> newVal);
	
	}
}
	

namespace Megahard.Tasks
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class Task
	{
	
		readonly PropertyBacking<TaskState> propState_ = new PropertyBacking<TaskState>("State", TaskState.Incomplete);
			[DefaultValue(TaskState.Incomplete)]
		public TaskState State
		{
			get { return  propState_.GetValue(); }
			
			private
			set 
			{ 
				if( propState_.WouldChange(value))
				{
					var chged =  propState_.SetValueNoEqualCheck(this, value);
					AfterStateChanged(chged);
				}
			}
		}
		partial void AfterStateChanged(ObjectChangedEventArgs<TaskState> newVal);
	
	}
}
	
