using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Visualization
{
	/// <summary>
	/// For use as the base type in TypeDescriptor.GetEditor(..., base);
	/// </summary>
	public abstract class VisualTypeEditor
	{
		public abstract IDataVisualizer CreateVisualizer(VisualizerStyle style);
	}

	/// <summary>
	/// for use as first arg in EditorAttribute
	/// </summary>
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
