
namespace Megahard.Data
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class DataObjectSource
	{
	PropertyBacking<object> propData_ = new PropertyBacking<object>("Data", null);
			[DefaultValue(null)][Category("Data")]
		public object Data
		{
			get { return  propData_.GetValue(); }
			
			
			set 
			{ 
				if( propData_.WouldChange(value))
				{
					var chged =  propData_.SetValueNoEqualCheck(this, value);
					AfterDataChanged(chged);
				}
			}
		}
		partial void AfterDataChanged(ObjectChangedEventArgs<object> newVal);
	
	}
}
	
