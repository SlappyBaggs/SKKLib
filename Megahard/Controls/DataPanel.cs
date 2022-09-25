using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Controls
{
	[ToolboxItem(true)]
	public partial class DataPanel : Data.Visualization.DataVisualizer
	{
		public DataPanel()
		{
			InitializeComponent();
			flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
		}

		//<ObservableProperty Name="PropertyFilter" Type="Predicate<System.ComponentModel.PropertyDescriptor>" Browsable="false" DesignerSerializationVisibility="DesignerSerializationVisibility.Hidden"/>
		//<ObservableProperty Name="SetupDataBox" Type="Action<Megahard.Data.Controls.DataBox, System.ComponentModel.PropertyDescriptor>" Browsable="false" DesignerSerializationVisibility="DesignerSerializationVisibility.Hidden"/>
		//<ObservableProperty Name="FlowDirection" Type="System.Windows.Forms.FlowDirection" DefaultValue="System.Windows.Forms.FlowDirection.LeftToRight"/>
		//<ObservableProperty Name="VisualizerStyle" Type="Megahard.Data.Visualization.VisualizerStyle" DefaultValue="Megahard.Data.Visualization.VisualizerStyle.Normal"/>
		partial void AfterFlowDirectionChanged(Megahard.Data.ObjectChangedEventArgs<FlowDirection> newVal)
		{
			flowLayoutPanel1.FlowDirection = newVal.NewValue;
		}

		readonly List<string> _propOrder = new List<string>();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<string> PropertyOrder
		{
			get { return _propOrder; }
		}

		protected override void OnObjectChanged(Megahard.Data.ObjectChangedEventArgs args)
		{
			if (args.PropertyName == "Data" || args.PropertyName == "PropertyFilter")
			{
				flowLayoutPanel1.Controls.Clear();
				var val = Data != null ? Data.GetValue() : null;
				AddProps(val);
			}
			base.OnObjectChanged(args);
		}

		void AddProps(object ob)
		{
			if (ob == null)
				return;
			var filter = PropertyFilter ?? (prop=>prop.IsBrowsable);
			var setupdbox = SetupDataBox ?? ((dbox, prop)=>
											{
												if (prop.PropertyType != typeof(bool))
													dbox.Label.Placement = Megahard.Data.Controls.DataBox.DataLabel.LabelPlacement.Top;
												var style = VisualizerStyle;
												if(style == Megahard.Data.Visualization.VisualizerStyle.Normal)
												{
													var visStyle = prop.Attributes.GetAttribute<Megahard.Data.Visualization.VisualizerStyleAttribute>();
													if (visStyle != null)
														style = visStyle.VisualizerStyle;
												}
												dbox.VisualizerStyle = style;
											});

			foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(ob).Sort(PropertyOrder.ToArray()))
			{
				if (filter(prop))
				{
					var db = new Megahard.Data.Controls.DataBox();
					db.AutoSize = true;
					db.AutoSizeMode = AutoSizeMode.GrowAndShrink;
					setupdbox(db, prop);
					db.Data = new Megahard.Data.DataObject(ob, prop.Name);
					
					flowLayoutPanel1.Controls.Add(db);
				}
			}
			
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			if(FlowDirection == FlowDirection.TopDown || FlowDirection == FlowDirection.BottomUp)
			{
				int h = Enumerable.Sum(from Control c in flowLayoutPanel1.Controls select c.GetPreferredSize(Size.Empty).Height + c.Margin.Vertical + 5);
				int w = Enumerable.Max(from Control c in flowLayoutPanel1.Controls select c.GetPreferredSize(Size.Empty).Width + c.Margin.Horizontal + 1);
				proposedSize.Height = h + Margin.Vertical;
				proposedSize.Width = w + Margin.Horizontal;
			}
			else
			{
				int h = Enumerable.Max(from Control c in flowLayoutPanel1.Controls select c.GetPreferredSize(Size.Empty).Height + c.Margin.Vertical + 1);
				int w = Enumerable.Sum(from Control c in flowLayoutPanel1.Controls select c.GetPreferredSize(Size.Empty).Width + c.Margin.Horizontal + 1);
				proposedSize.Height = h + Margin.Vertical;
				proposedSize.Width = w + Margin.Horizontal;
			}
			return proposedSize;
		}
	}
}
