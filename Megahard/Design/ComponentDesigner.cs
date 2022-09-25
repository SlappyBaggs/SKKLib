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
namespace Megahard.Design
{
	public enum InitializationType { Initialize, InitializeNew, InitializeExisting };
	public interface ISupportDesignTimeInitialization
	{
		void DesignTimeInitialize(ITreeDesigner designer, InitializationType initType);
	}
	public interface IModifyDesignTimeBehavior
	{
		SelectionRulesAlias ModifySelectionRules(SelectionRulesAlias currentVal);
	}

	/// <summary>
	/// Common base class for all designers to use, not much in it for now, this is mostly infrastructure
	/// </summary>
	public class ComponentDesigner : ComponentDesignerAlias
	{
		public ComponentDesigner()
		{
		}

		protected ServiceType GetService<ServiceType>() where ServiceType : class { return base.GetService(typeof(ServiceType)) as ServiceType; }
		
		protected IRootDesigner RootDesigner
		{
			get { return GetService<IRootDesigner>(); }
		}

		protected IDesignerHost DesignerHost { get { return GetService<IDesignerHost>(); } }

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			ActionLists.Add(new ActionList(component));
			Debug.DesignTrace.Info("ComponentDesigner:Registered the action list");

			if (component is ISupportDesignTimeInitialization)
			{
				(component as ISupportDesignTimeInitialization).DesignTimeInitialize(this, InitializationType.Initialize);
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

		public override void InitializeExistingComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeExistingComponent(defaultValues);
			if (Component is ISupportDesignTimeInitialization)
			{
				(Component as ISupportDesignTimeInitialization).DesignTimeInitialize(this, InitializationType.InitializeExisting);
			}
		}
	}

	public class ParentControlDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
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
}
