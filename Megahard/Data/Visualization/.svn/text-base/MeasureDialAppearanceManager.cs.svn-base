using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Megahard.Data.Visualization
{
	public partial class MeasureDialAppearanceManager : Component
	{
		public MeasureDialAppearanceManager()
		{
			InitializeComponent();
		}

		public MeasureDialAppearanceManager(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		private readonly MeasureDialColors colors_ = new MeasureDialColors();
		[Category("Common Properties")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public MeasureDialColors Colors
		{
			get
			{
				return colors_;
			}
		}
	}
}
