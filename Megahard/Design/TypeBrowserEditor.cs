using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.Design;

namespace Megahard.Design
{
	public class TypeBrowserEditor : Design.UITypeEditor
	{
		protected override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, System.Windows.Forms.Design.IWindowsFormsEditorService formEditSvc, object currentValue)
		{
			if (provider != null)
			{
				var browser = new Megahard.Controls.CollectionBrowser() { CategoryMember = "Namespace", DisplayMember = "Name" };
				browser.CategoryHeader.Heading = "Namespace";
				browser.MemberHeader.Heading = "Type Name";
				browser.AttachObserver("SelectedObjected", x =>
				{
					formEditSvc.CloseDropDown();
				});
				var disc = provider.GetService(typeof(System.ComponentModel.Design.ITypeDiscoveryService)) as System.ComponentModel.Design.ITypeDiscoveryService;
				if (disc != null)
				{
					foreach (Type t in disc.GetTypes(typeof(object), true))
					{
						try
						{
							browser.Add(t);
						}
						catch (Exception)
						{
						}
					}
				}

				formEditSvc.DropDownControl(browser);
				return browser.SelectedObjects.FirstOrDefault();
			}
			return base.EditValue(context, provider, formEditSvc, currentValue);
		}
	}
}
