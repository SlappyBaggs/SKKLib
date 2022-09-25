using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Reflection;
using Megahard.Reflection;
using ComponentDesignerAlias = System.ComponentModel.Design.ComponentDesigner;
using SelectionRulesAlias = System.Windows.Forms.Design.SelectionRules;
using INameCreationService = System.ComponentModel.Design.Serialization.INameCreationService;
using System.Windows.Forms;

namespace Megahard.Design
{
	public class ControlDesigner : System.Windows.Forms.Design.ControlDesigner
	{
		protected override void PreFilterProperties(System.Collections.IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor prop = (PropertyDescriptor)properties["Dock"];
			if (prop != null)
			{
				properties["Dock"] = TypeDescriptor.CreateProperty(prop.ComponentType, prop, ShowInSmartPanelAttribute.True);
			}
		}
		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
		{
			Utils.GenerateComponentName(base.Component);
			base.InitializeNewComponent(defaultValues);
			if (Component is ISupportDesignTimeInitialization)
			{
				(Component as ISupportDesignTimeInitialization).DesignTimeInitialize(this, InitializationType.InitializeNew);
			}
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			ActionLists.Add(new ActionList(component));
			//ActionLists.Add(new DockActionList(component as Control));
			Debug.DesignTrace.Info("ControlDesigner:Registered the action list");
			if (component is ISupportDesignTimeInitialization)
			{
				(component as ISupportDesignTimeInitialization).DesignTimeInitialize(this, InitializationType.Initialize);
			}
		}

		public override void InitializeExistingComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeExistingComponent(defaultValues);
			if (Component is ISupportDesignTimeInitialization)
			{
				(Component as ISupportDesignTimeInitialization).DesignTimeInitialize(this, InitializationType.InitializeExisting);
			}
		}

		public override SelectionRulesAlias SelectionRules
		{
			get
			{
				var rules = base.SelectionRules;
				if (Component is IModifyDesignTimeBehavior)
					rules = (Component as IModifyDesignTimeBehavior).ModifySelectionRules(rules);
				return rules;
			}
		}


		protected ServiceType GetService<ServiceType>() where ServiceType : class { return base.GetService(typeof(ServiceType)) as ServiceType; }
	}

	class DockActionList : DesignerActionList
	{
		public DockActionList(Control ctl) : base(ctl)
		{
			ctl_ = ctl;
		}
		readonly Control ctl_;
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			var ret = base.GetSortedActionItems();
			ret.Add(new DesignerActionPropertyItem("Dock", "Dock"));
			return ret;
		}

		public DockStyle Dock
		{
			get { return ctl_.Dock; }
			set { ctl_.Dock = value; }
		}
	}
}
