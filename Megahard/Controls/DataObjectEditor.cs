using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace Megahard.Data
{
	partial class DataObjectEditor : Data.Visualization.DataVisualizer
	{
		public DataObjectEditor()
		{
			InitializeComponent();
		}

		void OnDataChanged()
		{
			componentList_.DataSource = null;
			propertyTree_.SelectedObject = null;
			if (Data == null)
				return;


			var host = GetService<IDesignerHost>();
			if (host == null)
				return;
			if(host.Container != null)
			{
				System.Collections.ArrayList components = new System.Collections.ArrayList();
				components.Add(null);
				components.AddRange(host.Container.Components);
				var comps = (from IComponent c in components select new { Name = (c == null ? "(none)" : TypeDescriptor.GetComponentName(c)), Component = c });

				var sortedcomps = from c in comps orderby c.Name select c;
				componentList_.DisplayMember = "Name";
				componentList_.ValueMember = "Component";
				componentList_.DataSource = sortedcomps.ToList();
			}

			DataObject realData = (DataObject)Data.GetValue();
			if (realData != null)
			{
				object instance;
				PropertyPath prop;

				realData.GetBindingDetails(out instance, out prop);
				componentList_.SelectedValue = instance;
				propertyTree_.SelectedProperty = prop;
			}
		}

		protected override void OnCanceled()
		{
			componentList_.SelectedIndex = -1;
			propertyTree_.SelectedProperty = null;

			base.OnCanceled();
		}

		bool committing_ = false;

		protected override void OnCommitted()
		{
			try
			{
				committing_ = true;
				var comp = componentList_.SelectedValue;
				if (comp == null)
				{
					Data.SetValue(null);
				}
				else
				{
					var prop = propertyTree_.SelectedProperty;
					var dob = new DataObject(comp, prop);
					Data.SetValue(dob);
				}

				base.OnCommitted();
			}
			finally
			{
				committing_ = false;
			}
		}

		private void okCmd__Execute(object sender, EventArgs e)
		{
			CommitChanges();
		}

		private void cancelCmd__Execute(object sender, EventArgs e)
		{
			CancelChanges();
		}

		private void componentList__SelectedValueChanged(object sender, EventArgs e)
		{
			propertyTree_.SelectedObject = componentList_.SelectedValue;
		}

		private void DataBoxDataEditor_ObjectChanged(object sender, Megahard.Data.ObjectChangedEventArgs e)
		{
			if (!committing_ && e.PropertyName == "Data")
				OnDataChanged();
		}
	}
}
