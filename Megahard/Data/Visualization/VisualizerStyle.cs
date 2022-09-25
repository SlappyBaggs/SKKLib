using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public enum VisualizerStyle { Normal, CompactDropDown, CompactModalPopup, UITypeEditor };

	[AttributeUsage(AttributeTargets.Property)]
	public class VisualizerStyleAttribute : Attribute
	{
		public VisualizerStyleAttribute(VisualizerStyle style)
		{
			_style = style;
		}
		public VisualizerStyle VisualizerStyle
		{
			get { return _style; }
		}
		readonly VisualizerStyle _style;
	}
}
