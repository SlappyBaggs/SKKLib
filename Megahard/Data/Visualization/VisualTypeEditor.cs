using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	public abstract class VisualTypeEditor
	{
		public abstract IDataVisualizer CreateVisualizer(VisualizerStyle style);
	}
	public sealed class VisualTypeEditor<VisualizerType> : VisualTypeEditor where VisualizerType : IDataVisualizer, new()
	{
		public override IDataVisualizer CreateVisualizer(VisualizerStyle style)
		{
			switch (style)
			{
				case VisualizerStyle.UITypeEditor:
					return VisualUITypeEditor.CreateVisualizer();
				case VisualizerStyle.Normal:
					return new VisualizerType();
				case VisualizerStyle.CompactDropDown:
					return null;
				case VisualizerStyle.CompactModalPopup:
					return new CompactModalPopupVisualizer(new VisualizerType());
				default:
					throw new ArgumentOutOfRangeException("VisualizerStyle has unknown value");
			}
		}
	}
}
