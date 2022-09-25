using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Reflection;

using System.Collections;
namespace Megahard.Design
{
	public class ListComponentsInContainer : UITypeEditor
	{
		protected Func<IComponent, bool> filterDelegate_;
		public ListComponentsInContainer(Func<IComponent, bool> filterDelegate)
		{
			filterDelegate_ = filterDelegate;
		}

		protected ListComponentsInContainer()
		{

		}

		protected override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, IWindowsFormsEditorService edSvc, object currentValue)
		{
			IContainer cont = null;
			if (context != null)
				cont = context.Container;
			if (cont == null)
			{
				Debug.DesignTrace.Info("Unable to get container in ListComponentsInContainer");
				return null;
			}

			ComponentList editControl = new ComponentList(filterDelegate_, context, currentValue, edSvc, cont);
			edSvc.DropDownControl(editControl);
			return editControl.Selected;
		}

		class ComponentList : ListBox
		{
			private object objectSelected_;
			public object Selected
			{
				get
				{
					return objectSelected_;
				}
				private set
				{
					objectSelected_ = value;
				}
			}


			ArrayList items_ = new ArrayList();
			public ComponentList(Func<IComponent, bool> filter, ITypeDescriptorContext context, object defaultVal, IWindowsFormsEditorService editorService,
									IContainer cont)
			{
				objectSelected_ = defaultVal;

				SelectedIndexChanged += (s, e) => { Invalidate(); objectSelected_ = items_[SelectedIndex]; editorService.CloseDropDown(); };
				Size = new Size(100, 200);
				if (cont == null) return;

				foreach (IComponent component in cont.Components)
				{
					if (filter(component))
						items_.Add(component);
				}
				items_.Sort(new Comparer((x, y) =>
				{ return StringComparer.CurrentCulture.Compare(TypeDescriptor.GetComponentName(x), TypeDescriptor.GetComponentName(y)); }));

				foreach (object o in items_)
				{
					Items.Add(TypeDescriptor.GetComponentName(o));
				}
				Items.Insert(0, "(none)");
				items_.Insert(0, null);
				SelectedIndex = items_.IndexOf(defaultVal);
			}
		}
	}


	public class ListComponentsOfTypeInContainer<T> : ListComponentsInContainer where T : class, IComponent
	{
		public ListComponentsOfTypeInContainer() : base(x=>typeof(T).IsInstanceOfType(x) || typeof(T).IsAssignableFrom(x.GetType()))
		{
		}
	}

	public class ListCompatibleTypesInContainer : ListComponentsInContainer
	{
		public ListCompatibleTypesInContainer()
		{

		}

		protected override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, IWindowsFormsEditorService formEditSvc, object currentValue)
		{
			filterDelegate_ = x =>
				{
					return x != context.Instance && (context.PropertyDescriptor.PropertyType.IsInstanceOfType(x) || context.PropertyDescriptor.PropertyType.IsAssignableFrom(x.GetType()));
				};
			return base.EditValue(context, provider, formEditSvc, currentValue);
		}
	}
}
