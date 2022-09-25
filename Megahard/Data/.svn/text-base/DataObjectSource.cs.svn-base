using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace Megahard.Data
{
	[ToolboxItem(true)]
	[Designer(typeof(DataObjectSource.Designer))]
	public partial class DataObjectSource : ObservableComponent
	{
		public DataObjectSource()
		{
			InitializeComponent();
		}

		public DataObjectSource(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
				components = null;
			}
			base.Dispose(disposing);
		}

		private Type dataType_;
		[DefaultValue(null)]
		[Category("Data")]
		[Editor(typeof(Design.TypeBrowserEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public Type DataType
		{
			get
			{
				return dataType_;
			}
			set
			{
				if (dataType_ == value)
					return;
				dataType_ = value;
				TypeDescriptor.Refresh(this);
			}
		}

		//<ObservableProperty Type="object" Name="Data" DefaultValue="null" Category='"Data"'/>

		class Designer : Design.ComponentDesigner
		{
			protected override void PostFilterProperties(System.Collections.IDictionary properties)
			{
				base.PostFilterProperties(properties);
				properties.Remove("Data");
				properties.Remove("DataBindings");
				//MessageBox.Show("Post filter properties");
				properties["Data"] = TypeDescriptor.CreateProperty(typeof(DataObjectSource), "Data",
													(base.Component as DataObjectSource).DataType ?? typeof(object),
													new CategoryAttribute("Data"), new BrowsableAttribute(false));
			}
		}

	}

}
