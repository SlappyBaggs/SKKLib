using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace Megahard.Controls
{
	[Designer(typeof(ViewPanel.Designer))]
	public partial class ViewPanel : Component
	{
		public ViewPanel()
		{
			InitializeComponent();
		}

		public ViewPanel(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Panel Panel
		{
			get { return panel_; }
		}

		class Designer : Design.ComponentDesigner
		{
			ViewPanel ViewPanel
			{
				get { return (ViewPanel)base.Component; }
			}
			public override void Initialize(IComponent component)
			{
				base.Initialize(component);

				bool ret = EnableDesignMode(ViewPanel.Panel, "Panel");
			}

			protected bool EnableDesignMode(Control child, string name)
			{
				if (child == null)
				{
					throw new ArgumentNullException("child");
				}
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				INestedContainer service = this.GetService(typeof(INestedContainer)) as INestedContainer;
				if (service == null)
				{
					return false;
				}
				for (int i = 0; i < service.Components.Count; i++)
				{
					if (service.Components[i].Equals(child))
					{
						return true;
					}
				}
				service.Add(child, name);
				return true;
			}

 


			//INestedContainer nestedContainer_;
			public override System.Collections.ICollection AssociatedComponents
			{
				get
				{
					return new IComponent[] { ViewPanel.Panel };
				}
			}
		}
	}
}
