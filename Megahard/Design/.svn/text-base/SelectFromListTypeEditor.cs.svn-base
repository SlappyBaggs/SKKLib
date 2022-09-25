using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms.Design;

namespace Megahard.Design
{
	public abstract class SelectFromListTypeEditor : UITypeEditor
	{
		protected SelectFromListTypeEditor()
		{
			EditorStyle = System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}
		protected override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, IWindowsFormsEditorService formEditSvc, object value)
		{
			ListBox lb = null;
			try
			{
				lb = CreateListBox(context);
				SetupList(lb, context, value);
				if (lb.Items.Count > 0)
				{
					bool canceled = false;
					lb.MouseClick += delegate { formEditSvc.CloseDropDown(); };

					lb.KeyDown += (sender, keyDownArg)=>
					{
						if (keyDownArg.KeyCode == Keys.Enter || keyDownArg.KeyCode == Keys.Space)
						{
							formEditSvc.CloseDropDown();
						}
						if (keyDownArg.KeyCode == Keys.Escape)
						{
							canceled = true;
							formEditSvc.CloseDropDown();
						}
					};

					formEditSvc.DropDownControl(lb);
					if (lb.SelectedIndex >= 0 && !canceled)
					{
						value = lb.Items[lb.SelectedIndex];
						if (lb.ValueMember != "")
							value = TypeDescriptor.GetProperties(value)[lb.ValueMember].GetValue(value);
					}
				}
			}
			finally
			{
			}
			return value;
		}

		protected virtual ListBox CreateListBox(ITypeDescriptorContext context) { return new ListBox(); }
		protected abstract void SetupList(ListBox lb, ITypeDescriptorContext context, object value);
	}
}
