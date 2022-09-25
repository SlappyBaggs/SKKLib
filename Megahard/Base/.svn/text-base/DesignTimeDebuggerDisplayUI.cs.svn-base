using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Megahard.ExtensionMethods;
using System.ComponentModel.Design;
using System.Collections;
using Megahard.Reflection;
namespace Megahard.Debug
{
	partial class DesignTimeDebuggerDisplayUI : Form
	{
		private readonly DesignTimeDebugger dtd_;
		public DesignTimeDebuggerDisplayUI(DesignTimeDebugger dtd)
		{
			InitializeComponent();
			dtd_ = dtd;
			msgs_.DataSource = dtd_.DesignTraceMessages;
			dataTraceList_.DataSource = dtd_.DesignDataTrace;
			dataTraceList_.DisplayMember = "ShortDescription";
			dataTraceList_.SelectedIndexChanged += new EventHandler(dataTraceList__SelectedIndexChanged);

			PopulateInfoTree();
		}

		void dataTraceList__SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		IDesigner GetDesigner(IComponent comp)
		{
			IDesignerHost host = dtd_.Container as IDesignerHost;
			if (host == null)
				return null;
			return host.GetDesigner(comp);
		}

		T GetService<T>() where T : class
		{
			return dtd_.Site.GetService(typeof(T)) as T;
		}

		void PopulateInfoTree()
		{
			try
			{
				var rootNode = InspectionNode.Create(GetService<IRootDesigner>(), "RootDesigner");
				infoTree_.Nodes.Add(rootNode);

				var hostNode = InspectionNode.Create(GetService<IDesignerHost>(), "DesignerHost");
				infoTree_.Nodes.Add(hostNode);

				infoTree_.Nodes.Add(InspectionNode.Create(GetService<ITypeDiscoveryService>(), "ITypeDiscovery"));

				foreach (IComponent comp in hostNode.Value.Container.Components)
				{
					InspectionNode compNode = new InspectionNode(comp, comp.Site.Name);
					compNode.Nodes.Add(new InspectionNode(hostNode.Value.GetDesigner(comp), "Designer"));
					hostNode.Nodes.Add(compNode);
				}

				var refNode = InspectionNode.Create(GetService<IReferenceService>(), "References");
				foreach (object ob in refNode.Value.GetReferences())
				{
					refNode.Nodes.Add(new InspectionNode(ob, refNode.Value.GetName(ob) ?? "(none)"));
				}
				infoTree_.Nodes.Add(refNode);

				var extenderList = InspectionNode.Create(GetService<IExtenderListService>(), "Extender Providers");
				foreach (IExtenderProvider provider in extenderList.Value.GetExtenderProviders())
				{
					var node = extenderList.AddChildNode(provider.GetType().Name, provider);
					node.Nodes.Add(new InspectionNode(TypeDescriptor.GetAttributes(provider)));
				}
				var behave = InspectionNode.Create(GetService<System.Windows.Forms.Design.Behavior.BehaviorService>(), "Behavior Service");
				var adNode = behave.AddChildNode("Adorners", behave.Value.Adorners);
				foreach (var adorner in behave.Value.Adorners)
				{
					var node = new InspectionNode(adorner, "Adorner");
					adNode.Nodes.Add(node);
					if (adorner.Glyphs != null && adorner.Glyphs.Count > 0)
					{
						var gnode = node.AddChildNode("Glyphs", adorner.Glyphs);
						foreach (var glyph in adorner.Glyphs)
						{
							gnode.Nodes.Add(new InspectionNode(glyph, "Glyph"));
						}
					}
				}
				infoTree_.Nodes.Add(behave);

				infoTree_.Nodes.Add(extenderList);

				//Populate(dtd_.Container);
				infoTree_.Sort();
			}
			catch (Exception ex)
			{
				statusLbl_.Text = ex.Message;
			}
		}

		class InspectionNode : TreeNode
		{
			public InspectionNode()
			{
			}
			public InspectionNode(AttributeCollection attrs) : base("Attributes")
			{
				Tag = attrs;
				foreach (Attribute attr in attrs)
				{
					var node = AddChildNode(attr.GetType().Name, attr);
				}
			}

			public InspectionNode(PropertyDescriptorCollection props)
				: base("Property Descriptors")
			{
				Tag = props;
				foreach (PropertyDescriptor prop in props)
				{
					var node = AddChildNode(prop.Name, prop);
					node.Nodes.Add(new InspectionNode(prop.Attributes));
					node.AddChildNode("Converter", prop.Converter);

				}
			}

			public InspectionNode(EventDescriptorCollection events)
				: base("Event Descriptors")
			{
				Tag = events;
				foreach (EventDescriptor ed in events)
				{
					var node = AddChildNode(ed.Name, ed);
					node.Nodes.Add(new InspectionNode(ed.Attributes));
				}
			}

			public InspectionNode(IContainer cont) : base("Container")
			{
				Tag = cont;
				foreach (IComponent comp in cont.Components)
				{
					base.Nodes.Add(new InspectionNode(comp, comp.Site.Name));
				}
			}

			public InspectionNode(object comp, string name)
			{
				if (comp == null)
				{
					base.Text = "(null)";
				}
				else
				{
					base.Text = name;
					Tag = comp;
					var imap = comp.GetType().GetInterfaces();
					if (imap != null && imap.Length > 0)
					{
						var interfaces = AddChildNode("Interfaces", imap);
						foreach (Type itype in imap)
							interfaces.AddChildNode(itype.Name, itype);
					}

					var typeDesc = AddChildNode("Type Descriptor", TypeDescriptor.GetProvider(comp));
					var defProp = TypeDescriptor.GetDefaultProperty(comp);
					if (defProp != null)
						typeDesc.AddChildNode("Default Property", defProp);

					var defEvent = TypeDescriptor.GetDefaultEvent(comp);
					if (defEvent != null)
						typeDesc.AddChildNode("Default Event", defEvent);

					typeDesc.AddChildNode("Converter", TypeDescriptor.GetConverter(comp));

					var editor = TypeDescriptor.GetEditor(comp, typeof(System.Drawing.Design.UITypeEditor));
					if (editor != null)
						typeDesc.AddChildNode("UITypeEditor", editor);
					typeDesc.AddChildNode("ReflectType", TypeDescriptor.GetReflectionType(comp));
					base.Nodes.Add(new InspectionNode(TypeDescriptor.GetProperties(comp)));
					base.Nodes.Add(new InspectionNode(TypeDescriptor.GetEvents(comp)));
				}
			}

			public InspectionNode AddChildNode(string txt, object tag)
			{
				var node = new InspectionNode() { Text = txt, Tag = tag };
				base.Nodes.Add(node);
				return node;
			}

			public static InspectionNode<T> Create<T>(T ob, string txt) { return new InspectionNode<T>(txt, ob); }
		}

		class InspectionNode<T> : InspectionNode
		{
			public InspectionNode(string txt, T ob)
			{
				base.Text = txt;
				base.Tag = ob;
			}

			public T Value
			{
				get { return (T)base.Tag; }
			}
		}

		private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			statusStrip1.Visible = false;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// TODO: remove the following lines after you know the resource names

			string[] sa = this.GetType().Assembly.GetManifestResourceNames();

			MessageBox.Show(string.Join(Environment.NewLine, sa));
		}


		void AddDetailItem(params string[] txt)
		{
			detailList_.Items.Add(new ListViewItem(txt));
		}

		void ShowDetails(object ob)
		{
			propertyGrid_.SelectedObject = ob;

			try
			{
				detailList_.SuspendLayout();
				detailList_.Items.Clear();
				if (ob == null)
					return;

				AddDetailItem("Type", ob.GetType().Namespace + "." + ob.GetType().Name);
				AddDetailItem("Base", ob.GetType().BaseType.Namespace + "." + ob.GetType().Name);
				AddDetailItem("ComponentName", TypeDescriptor.GetComponentName(ob));
				AddDetailItem("FullComponentName", TypeDescriptor.GetFullComponentName(ob));
				AddDetailItem("ClassName", TypeDescriptor.GetClassName(ob));
				detailList_.ResizeColumnsIntelligent();
			}
			finally
			{
				detailList_.ResumeLayout();
			}
		}

		private void infoTree__NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			ShowDetails(e.Node.Tag);
		}

	}
}
