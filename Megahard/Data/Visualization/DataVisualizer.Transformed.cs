
namespace Megahard.Data.Visualization
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class DataVisualizer
	{
	PropertyBacking<DataObject> propData_ = new PropertyBacking<DataObject>("Data", null);
			[DefaultValue(null)]
		public  DataObject Data
		{
			get { return  propData_.GetValue(); }
			
			
			set 
			{ 
				BeforeSetData(ref value);
				if( propData_.WouldChange(value))
				{
					var chged =  propData_.SetValueNoEqualCheck(this, value);
					AfterDataChanged(chged);
				}
			}
		}
		
		partial void BeforeSetData(ref DataObject incomingValue);
		partial void AfterDataChanged(ObjectChangedEventArgs<DataObject> newVal);
	
	}
}
	

namespace Megahard.Data.Visualization
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class DataVisualizer
	{
	PropertyBacking<bool> propAllowEditing_ = new PropertyBacking<bool>("AllowEditing", false);
			[DefaultValue(false)]
		public  bool AllowEditing
		{
			get { return  propAllowEditing_.GetValue(); }
			
			
			set 
			{ 
				BeforeSetAllowEditing(ref value);
				if( propAllowEditing_.WouldChange(value))
				{
					var chged =  propAllowEditing_.SetValueNoEqualCheck(this, value);
					AfterAllowEditingChanged(chged);
				}
			}
		}
		
		partial void BeforeSetAllowEditing(ref bool incomingValue);
		partial void AfterAllowEditingChanged(ObjectChangedEventArgs<bool> newVal);
	
	}
}
	

namespace Megahard.Data.Visualization
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class DataVisualizer
	{
	PropertyBacking<UpdateMode> propUpdateMode_ = new PropertyBacking<UpdateMode>("UpdateMode", UpdateMode.Automatic);
			[DefaultValue(UpdateMode.Automatic)][Category("Data")]
		public  UpdateMode UpdateMode
		{
			get { return  propUpdateMode_.GetValue(); }
			
			
			set 
			{ 
				BeforeSetUpdateMode(ref value);
				if( propUpdateMode_.WouldChange(value))
				{
					var chged =  propUpdateMode_.SetValueNoEqualCheck(this, value);
					AfterUpdateModeChanged(chged);
				}
			}
		}
		
		partial void BeforeSetUpdateMode(ref UpdateMode incomingValue);
		partial void AfterUpdateModeChanged(ObjectChangedEventArgs<UpdateMode> newVal);
	
	}
}
	

namespace Megahard.Data.Visualization
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class DataVisualizer
	{
	PropertyBacking<CommitMode> propCommitMode_ = new PropertyBacking<CommitMode>("CommitMode", CommitMode.Automatic);
			[DefaultValue(CommitMode.Automatic)][Category("Data")]
		public  CommitMode CommitMode
		{
			get { return  propCommitMode_.GetValue(); }
			
			
			set 
			{ 
				BeforeSetCommitMode(ref value);
				if( propCommitMode_.WouldChange(value))
				{
					var chged =  propCommitMode_.SetValueNoEqualCheck(this, value);
					AfterCommitModeChanged(chged);
				}
			}
		}
		
		partial void BeforeSetCommitMode(ref CommitMode incomingValue);
		partial void AfterCommitModeChanged(ObjectChangedEventArgs<CommitMode> newVal);
	
	}
}
	
