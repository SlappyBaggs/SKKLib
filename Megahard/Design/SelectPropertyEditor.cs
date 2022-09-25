using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;

namespace Megahard.Design
{
	/// <summary>
	/// An editor which displays a tree of all the properties of the Target ob and all expandable properties
	/// Let's use select one and it is returned....
	/// </summary>
	public abstract class SelectPropertyEditor : Megahard.Design.UITypeEditor
	{
		public SelectPropertyEditor()
		{
			base.EditorStyle = System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}

		/// <summary>
		/// Implement this function to indicate to the editor which object it is we wish to take a list of properties from
		/// </summary>
		/// <returns>The object whose properties will be iterated</returns>
		protected abstract object GetTarget(ITypeDescriptorContext context, IServiceProvider provider, object currentVal);

		protected override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, IWindowsFormsEditorService formEditSvc, object currentValue)
		{
			object origVal = currentValue;
			try
			{
				object target = GetTarget(context, provider, currentValue);

				var tree = new Controls.PropertyTree() { SelectedObject = target };
				tree.SelectedProperty = currentValue == null ? string.Empty : currentValue.ToString();

				tree.Dock = DockStyle.Fill;

				UserControl ctl = new UserControl() { MinimumSize = new System.Drawing.Size(200, 200) };
				ctl.AutoScroll = true;
				ctl.Controls.Add(tree);
				Button okBut = new Button() { Text = "OK" };
				ctl.Controls.Add(okBut);
				var loc = okBut.Location;
				loc.Y = tree.Height + tree.Location.Y + 2;
				okBut.Location = loc;
				okBut.Dock = DockStyle.Bottom;


				okBut.MouseClick += (s, e) =>
				{
					formEditSvc.CloseDropDown();
				};

				// It seems convuluted but the treeview selection events are imho kind of weird, so this is a way that seemed to work ++Jeff
				ctl.Load += delegate
				{
					tree.NodeMouseClick += (s, e) =>
					{
						if (e.Node.IsSelected)
							formEditSvc.CloseDropDown();
					};
				};
				Megahard.Debug.DesignTrace.Info("SelectPropertyEditor EditObject");
				formEditSvc.DropDownControl(ctl);

				return SmartConvert.ConvertTo(currentValue, context.PropertyDescriptor.PropertyType);
			}
			catch(Exception e)
			{
				MessageBox.Show("SelectPropertyEditor Error: " + e.Message);
				return origVal;
			}
		}

		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

	}
}
