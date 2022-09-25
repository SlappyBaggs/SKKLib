using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
namespace Megahard.Controls
{
	[System.Windows.Forms.Design.ToolStripItemDesignerAvailability(System.Windows.Forms.Design.ToolStripItemDesignerAvailability.All)]
	public class ToolStripControlHost<ControlType> : System.Windows.Forms.ToolStripControlHost where ControlType : System.Windows.Forms.Control, new()
	{
		/*
		static ToolStripControlHost()
		{
			TypeDescriptor.AddProvider(new DescriptorProvider(), typeof(ToolStripControlHost<ControlType>));
		}
		 */
		public ToolStripControlHost(ControlType ctl) : base(ctl)
		{

		}

		public ToolStripControlHost()
			: base(new ControlType())
		{

		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new ControlType Control
		{
			get
			{
				return (ControlType)base.Control;
			}
		}
	}

	

	public class ForwardToolboxBitmapAttribute<From, To> : ComponentModel.ForwardAttribute<From, To, ToolboxBitmapAttribute>
	{
	}


	public static class ToolStripControlHost
	{
		public static ToolStripControlHost<T> Create<T>(T ctl) where T : System.Windows.Forms.Control, new()
		{
			return new ToolStripControlHost<T>(ctl);
		}
	}
}
