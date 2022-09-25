using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Reflection;
using Megahard.Reflection;

namespace Megahard.Design
{
	class ActionList : DesignerActionList, ICustomTypeDescriptor, IComponent
	{
		public ActionList(IComponent comp)
			: base(comp)
		{


		}
		readonly List<PropertyDescriptor> customProps_ = new List<PropertyDescriptor>();
		class ActionMethod : DesignerActionList
		{
			private readonly MethodInfo mi_;
			readonly bool fireComponentChg_;
			public ActionMethod(MethodInfo mi, IComponent comp, bool fireComponentChg)
				: base(comp)
			{
				mi_ = mi;
				fireComponentChg_ = fireComponentChg;
			}
			
			public void DoAction()
			{
				try
				{
					mi_.Invoke(Component, null);
					if(fireComponentChg_)
					{
						IComponentChangeService chg = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
						if (chg != null)
							chg.OnComponentChanged(base.Component, null, null, null);
					}
				}
				catch (Exception e)
				{
					string msg = string.Format("Exception: {0}{1}{2}{3}{4}", e.GetType().Name, Environment.NewLine, e.Message, Environment.NewLine, "Do you want to debug?");
					if (System.Windows.Forms.MessageBox.Show(msg, "Exception performing Action Method", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
						System.Diagnostics.Debugger.Break();
				}
			}
		}
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			HashSet<string> cats = new HashSet<string>();
			customProps_.Clear();
			DesignerActionItemCollection ret = new DesignerActionItemCollection();
			foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(Component, new Attribute[] { new ShowInSmartPanelAttribute() }))
			{
				if (!(prop.Attributes[typeof(ShowInSmartPanelAttribute)] as ShowInSmartPanelAttribute).Show)
					continue;

				if (prop.Converter.GetPropertiesSupported() && prop.IsReadOnly)
				{
					var propVal = prop.GetValue(Component);
					if (propVal != null)
					{
						ret.Add(new DesignerActionHeaderItem(prop.Name));
						foreach (PropertyDescriptor childProp in prop.Converter.GetProperties(null, propVal, new Attribute[] { new ReadOnlyAttribute(false)  }))
						{
							customProps_.Add(new ComponentModel.WrappedPropertyDescriptor(childProp, propVal));
							ret.Add(new DesignerActionPropertyItem(childProp.Name, childProp.DisplayName, prop.Name, childProp.Description));
						}
					}
					continue;
				}
				if (!string.IsNullOrEmpty(prop.Category) && cats.Add(prop.Category))
					ret.Add(new DesignerActionHeaderItem(prop.Category));
				ret.Add(new DesignerActionPropertyItem(prop.Name, prop.DisplayName, prop.Category, prop.Description));

				customProps_.Add(new ComponentModel.WrappedPropertyDescriptor(prop, Component));
			}


			foreach (MethodInfo mi in Component.GetType().GetMethods<ShowInSmartPanelAttribute>(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, true))
			{
				string displayName = string.Empty;
				string category = string.Empty;
				string description = string.Empty;
				ShowInSmartPanelAttribute showInSmartPanelAttr = null;

				foreach (Attribute attr in mi.GetCustomAttributes(true))
				{
					if (attr is DisplayNameAttribute)
						displayName = (attr as DisplayNameAttribute).DisplayName;
					else if (attr is CategoryAttribute)
						category = (attr as CategoryAttribute).Category;
					else if (attr is DescriptionAttribute)
						description = (attr as DescriptionAttribute).Description;
					else if (attr is ShowInSmartPanelAttribute)
						showInSmartPanelAttr = attr as ShowInSmartPanelAttribute;

				}
				if (!string.IsNullOrEmpty(category) && cats.Add(category))
					ret.Add(new DesignerActionHeaderItem(category));
				if (displayName == string.Empty)
					displayName = mi.Name;

				bool useAsVerb = showInSmartPanelAttr.IncludeAsVerb;
				ret.Add(new DesignerActionMethodItem(new ActionMethod(mi, Component, showInSmartPanelAttr.AutoFireComponentChange), "DoAction", displayName, category, description, useAsVerb));
			}

			TypeDescriptor.Refresh(this);
			return ret;
		}

		#region ICustomTypeDescriptor Members

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		public string GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		public string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public EventDescriptor GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		public object GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			List<PropertyDescriptor> pdc = new List<PropertyDescriptor>();
			if (customProps_ != null)
			{
				foreach (var prop in customProps_)
				{
					if(prop.Attributes.Contains(attributes) || attributes == null)
						pdc.Add(prop);
				}
			}

			foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this, attributes, true))
				pdc.Add(prop);
			return new PropertyDescriptorCollection(pdc.ToArray(), true);
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			object ret = null;
			if (TypeDescriptor.GetProperties(this, true).Contains(pd))
				ret = this;
			else
				ret = Component ?? (object)this;
			return ret;
		}

		#endregion

		event EventHandler IComponent.Disposed
		{
			add {  }
			remove { }
		}

		ISite IComponent.Site
		{
			get
			{
				return Component != null ? Component.Site : null;
			}
			set
			{
				
			}
		}

		void IDisposable.Dispose()
		{
			
		}
	}
}
