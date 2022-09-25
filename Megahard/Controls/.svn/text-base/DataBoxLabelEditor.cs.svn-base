using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Data.Controls
{
	public partial class DataBoxLabelEditor : Visualization.DataVisualizer
	{
		public DataBoxLabelEditor()
		{
			InitializeComponent();
			//visible_.Data = new DataObject(this, "Data.Visible");
			base.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(DataBoxLabelEditor_ObjectChanged);
		}

		void DataBoxLabelEditor_ObjectChanged(object sender, ObjectChangedEventArgs e)
		{
			if(e.PropertyName == "Data")
				propertyGrid1.SelectedObject = Data;
		}
	}
}
