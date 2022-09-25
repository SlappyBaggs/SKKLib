
namespace Megahard.Data.Controls
{
	using System;
	using System.ComponentModel;
	using Megahard.Data;
	partial class DataBox
	{
	PropertyBacking<Megahard.Data.Visualization.VisualizerStyle> propVisualizerStyle_ = new PropertyBacking<Megahard.Data.Visualization.VisualizerStyle>("VisualizerStyle", Megahard.Data.Visualization.VisualizerStyle.Normal);
			[DefaultValue(Megahard.Data.Visualization.VisualizerStyle.Normal)][Category("Appearance")]
		public  Megahard.Data.Visualization.VisualizerStyle VisualizerStyle
		{
			get { return  propVisualizerStyle_.GetValue(); }
			
			
			set 
			{ 
				BeforeSetVisualizerStyle(ref value);
				if( propVisualizerStyle_.WouldChange(value))
				{
					var chged =  propVisualizerStyle_.SetValueNoEqualCheck(this, value);
					AfterVisualizerStyleChanged(chged);
				}
			}
		}
		
		partial void BeforeSetVisualizerStyle(ref Megahard.Data.Visualization.VisualizerStyle incomingValue);
		partial void AfterVisualizerStyleChanged(ObjectChangedEventArgs<Megahard.Data.Visualization.VisualizerStyle> newVal);
	
	}
}
	
