using System.Windows.Forms;
using System;
using System.ComponentModel;
namespace Megahard.Controls
{
	[Designer(typeof(Design.ControlDesigner))]
	public class PropertyTree : TreeView
	{
		public PropertyTree()
		{
			HideSelection = false;
		}

		[DefaultValue(false)]
		public new bool HideSelection
		{
			get	{ return base.HideSelection; }
			set { base.HideSelection = value; }
		}

		#region Property SelectedProperty

		[DefaultValue(null)]
		[Browsable(false)]
		public Data.PropertyPath SelectedProperty
		{
			get
			{
				return GetPropertyName(SelectedNode);
			}
			set
			{
				if (value.IsEmpty)
				{
					SelectedNode = Nodes.Count > 0 ? Nodes[0] : null;
				}
				else
				{
					var nodes = Nodes.Find(value.Path, true);
					if (nodes.Length > 0)
						SelectedNode = nodes[0];
					else
						SelectedNode = null;
				}

				OnSelectedPropertyChanged();
			}
		}
		public event EventHandler SelectedPropertyChanged;
		protected virtual void OnSelectedPropertyChanged()
		{
			if (SelectedPropertyChanged != null)
				SelectedPropertyChanged(this, EventArgs.Empty);
		}

		#endregion

		#region Property SelectedObject
		object propSelectedObject_;
		
		[DefaultValue(null), Browsable(false)]
		public object SelectedObject
		{
			get { return propSelectedObject_; }
			set
			{
				if(propSelectedObject_ == value)
					return;
				propSelectedObject_ = value;
				OnSelectedObjectChanged();
			}
		}
		
		public event EventHandler SelectedObjectChanged;
		protected virtual void OnSelectedObjectChanged()
		{
			PopulateTree();

			if(SelectedObjectChanged != null)
				SelectedObjectChanged(this, EventArgs.Empty);
		}
		
		#endregion

		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			var node = e.Node;
			if (node.Nodes.Count == 1 && node.Nodes[0] is PropNode)
			{
				var pnode = node.Nodes[0] as PropNode;
				node.Nodes.Clear();
				foreach (PropertyDescriptor subProp in pnode.SubProps)
				{
					object ob = null;
					try
					{
						if (pnode.Instance != null)
						{
							ob = pnode.Prop.GetValue(pnode.Instance);
						}
					}
					catch
					{
						ob = null;
					}
					AddProperty(subProp, node, ob);
				}
			}


			base.OnBeforeExpand(e);
		}

		class PropNode : TreeNode
		{
			public PropNode(PropertyDescriptor prop, object instance, PropertyDescriptorCollection subProps)
			{
				Prop = prop;
				Instance = instance;
				SubProps = subProps;
			}

			public PropertyDescriptorCollection SubProps { get; private set; }
			public PropertyDescriptor Prop { get; private set; }
			public object Instance { get; private set; }
		}

		
		private void AddProperty(PropertyDescriptor prop, TreeNode parent, object instance)
		{
			if (!IncludeProperty(prop))
				return;
			TreeNode propNode = new TreeNode(prop.DisplayName);

			propNode.Tag = prop;
			propNode.Name = parent.Tag == null ? prop.Name : parent.Name + "." + prop.Name;
			parent.Nodes.Add(propNode);
			PropertyDescriptorCollection subProps = GetSubProperties(prop, instance);
			if (subProps != null)
			{
				if (subProps.Count > 0)
				{
					propNode.Nodes.Add(new PropNode(prop, instance, subProps));
				}
				return;
				/*
				foreach (PropertyDescriptor subProp in subProps)
				{
					object ob = null;
					try
					{
						if (instance != null)
						{
							ob = prop.GetValue(instance);
						}
					}
					catch
					{
						ob = null;
					}
					AddProperty(subProp, propNode, ob);
				}
				*/
			}

			//if ((!prop.IsReadOnly || ShowReadOnlyProperties) || propNode.Nodes.Count > 0 || typeof(ICollection).IsAssignableFrom(prop.PropertyType))
			
			
			//parent.Nodes.Add(propNode);
		}

		protected virtual PropertyDescriptorCollection GetSubProperties(PropertyDescriptor prop, object instance)
		{
			//if (prop.Converter == null || !prop.Converter.GetPropertiesSupported())
			//	return null;

			try
			{
				object propVal = null;
				if (instance != null)
					propVal = prop.GetValue(instance);

				if (propVal != null && prop.Converter.GetPropertiesSupported())
				{
					return prop.Converter.GetProperties(propVal);
				}
				if (propVal != null)
					return TypeDescriptor.GetProperties(propVal);
			}
			catch
			{
			}

			return TypeDescriptor.GetProperties(prop.PropertyType);
		}

		protected virtual bool IncludeProperty(PropertyDescriptor prop)
		{
			return true;
		}

		private Data.PropertyPath GetPropertyName(TreeNode tn)
		{
			if (tn == null || tn.Tag == s_None)
				return string.Empty;
			return tn.Name.ToString();
		}

		protected virtual bool AllowSelect(PropertyDescriptor prop)
		{
			return true;
		}

		void PopulateTree()
		{
			Nodes.Clear();
			PropertyDescriptorCollection pdc = null;

			if (SelectedObject is ITypedList)
			{
				pdc = (SelectedObject as ITypedList).GetItemProperties(null);
				//SelectedObject = null;
			}
			else
			{
				pdc = TypeDescriptor.GetProperties(SelectedObject);
			}

			var root = new TreeNode();
			Nodes.Add(root);
			foreach (PropertyDescriptor prop in pdc)
			{
				//var catNode = Nodes[prop.Category];
				//if (catNode == null)
				//	catNode = Nodes.Add(prop.Category, prop.Category);
				AddProperty(prop, root, SelectedObject);
			}

			// Don't use a root node if there is only one, just move all the children up a level
			if (Nodes.Count == 1)
			{
				var tn = Nodes[0];
				Nodes.Clear();
				foreach (TreeNode child in tn.Nodes)
					Nodes.Add(child);
			}

			Sort();
			if (Nodes.Count > 0)
			{
				Nodes.Insert(0, new TreeNode("(none)") { Tag = s_None });
			}
			SelectedProperty = "";
		}

		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			base.OnBeforeSelect(e);
			PropertyDescriptor pd = e.Node.Tag as PropertyDescriptor;
			e.Cancel = e.Node.Tag != s_None && (pd == null || !AllowSelect(pd));
		}
		static readonly object s_None = new object();
	}
}
