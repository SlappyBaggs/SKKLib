using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Megahard.Data
{
	[ProvideProperty("DataBindings", typeof(Component))]
	[ToolboxItem(true)]
	public partial class BindingManager : Megahard.ComponentModel.ComponentBase, IExtenderProvider
	{
		public BindingManager()
		{
			InitializeComponent();
		}

		public BindingManager(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		public void SetDataBindings(Component comp, DataBinderCollection db)
		{
			dict_[comp] = db;
		}

		[DefaultValue(null)]
		public DataBinderCollection GetDataBindings(Component comp)
		{
			DataBinderCollection ret;
			dict_.TryGetValue(comp, out ret);
			return ret;
		}

		readonly Dictionary<Component, DataBinderCollection> dict_ = new Dictionary<Component, DataBinderCollection>();

		bool IExtenderProvider.CanExtend(object extendee)
		{
			return extendee != this;
		}
	}

}
