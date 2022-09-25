using System.Windows.Forms.Design.Behavior;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.Design;
using Megahard.Data.Binding;
namespace Megahard.Data.Controls
{
	class DataBoxDesigner : Design.ControlDesigner
	{
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
		}

		DataBox DataBox
		{
			get { return Control as DataBox; }
		}



		protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);

			var dbox = DataBox;
			if (dbox == null || dbox.Data != null)
				return;

			var selectionService = GetService<ISelectionService>();
			if (selectionService == null || !selectionService.GetComponentSelected(dbox))
			{
				ControlPaint.DrawBorder(pe.Graphics, pe.ClipRectangle, Color.LightBlue, ButtonBorderStyle.Dotted);
			}
		}
	}
}
