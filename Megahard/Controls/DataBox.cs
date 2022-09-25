using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection;
using System.Linq;
using ComponentFactory.Krypton.Toolkit;
using Megahard.ExtensionMethods;
using System.Collections.Generic;
using System.ComponentModel.Design;


namespace Megahard.Data.Controls
{
	/// <summary>
	/// Notes on thread safety:
	/// You are not to call any functions on DataBox from a thread other than the thread which create databox.  Typical WinForm control rules here.  There is one exception.
	/// The data property is exception, the dataobject can only be changed from main thread, but the SetValue func on the dataobject can be called from anythread, and
	/// change notification of data subobjects can fire on any thread, whew complex
	
	/// </summary>
    [ToolboxItem(true)]
	[Designer(typeof(DataBoxDesigner))]
	[DefaultProperty("Data")]
	[ToolboxBitmap(typeof(reslocator), "Megahard.Icons.DataBox.ico")]
    public partial class DataBox : Megahard.Controls.UserControlBase
	{
		static DataBox()
		{
			if (TypeDescriptor.GetEditor(typeof(bool), typeof(Data.Visualization.VisualTypeEditor)) == null)
				TypeDescriptor.AddAttributes(typeof(bool), new EditorAttribute(typeof(Visualization.VisualTypeEditor<Visualization.BooleanVisualizer>), typeof(Visualization.VisualTypeEditor)));

			foreach (Type t in Mathematics.NumericInfo.NumericTypes)
			{
				if (TypeDescriptor.GetEditor(t, typeof(Data.Visualization.VisualTypeEditor)) == null)
					TypeDescriptor.AddAttributes(t, new EditorAttribute(typeof(Visualization.VisualTypeEditor<Visualization.NumericUpDownVisualizer>), typeof(Visualization.VisualTypeEditor)));
			}
		}

		public DataBox()
		{
			base.CreateHandle();
			InitializeComponent();
			if (components == null)
				components = new Container();
			propLabel_ = new DataLabel(this);
			base.RegisterChildObservable("Label");
		}
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if(components != null)
					components.Dispose();
				if (_contextMenu != null)
				{
					_contextMenu.Dispose();
					_contextMenu = null;
				}
			}
			base.Dispose(disposing);
		}

		//<ObservableProperty Name="VisualizerStyle" Type="Megahard.Data.Visualization.VisualizerStyle" DefaultValue="Megahard.Data.Visualization.VisualizerStyle.Normal" Category='"Appearance"'/>

		KryptonContextMenu _contextMenu;

		void CreateContextMenu()
		{
			if (_contextMenu != null)
				return;
			_contextMenu = new KryptonContextMenu();
			var items = new KryptonContextMenuItems() { ImageColumn = false };
			var reset = new KryptonContextMenuItem("Reset");
			reset.Click += delegate
			{
				if (Data != null)
					Data.Reset();
			};
			items.Items.Add(reset);
			_contextMenu.Items.Add(items);
		}

		private bool readOnly_;
		[DefaultValue(false)]
		[Category("Data")]
		public bool ReadOnly
		{
			get
			{
				return readOnly_;
			}
			set
			{
				readOnly_ = value;
				if (activeVisualizer_ != null)
					activeVisualizer_.AllowEditing = !value;
			}
		}

		/// <summary>
		/// Indicates if there is an error with the bound data
		/// </summary>
		/// 
		[Browsable(false)]
		public bool Error
		{
			get { return error_; }
		}
		bool error_;

		public event EventHandler<Megahard.Data.Visualization.DataErrorEventArgs> DataError;
		protected virtual void OnDataError(Megahard.Data.Visualization.DataErrorEventArgs arg)
		{
			var copy = DataError;
			if (copy != null)
				copy(this, arg);
		}

		void EnterErrorMode()
		{
			error_ = true;
			object ob;
			PropertyPath prop;
			Data.GetBindingDetails(out ob, out prop);
			INotifyPropertyChanged propChg = ob as INotifyPropertyChanged;
			if (propChg != null)
				propChg.PropertyChanged += ErrorMode_MonitorObject;
		}

		void ExitErrorMode()
		{
			error_ = false;
			if (Data == null)
				return;
			object ob;
			PropertyPath prop;
			Data.GetBindingDetails(out ob, out prop);
			INotifyPropertyChanged propChg = ob as INotifyPropertyChanged;
			if (propChg != null)
				propChg.PropertyChanged -= ErrorMode_MonitorObject;
		}
		void ErrorMode_MonitorObject(object ob, EventArgs args)
		{
			ReloadVisualizer();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			if (activeVisualizer_ == null)
				return proposedSize;
			if (Label.Size.IsHidden)
				return activeVisualizer_.GUIObject.GetPreferredSize(proposedSize);
			var lblSize = LabelControl.GetPreferredSize(proposedSize);
			if (Label.Placement == DataLabel.LabelPlacement.Left)
			{
				if(proposedSize.Width > 0)
					proposedSize.Width -= lblSize.Width;
				var ps = activeVisualizer_.GUIObject.GetPreferredSize(proposedSize);
				ps.Width += lblSize.Width;
				ps.Height = Math.Max(ps.Height, lblSize.Height);
				return ps;
			}
			else
			{
				if(proposedSize.Height > 0)
					proposedSize.Height -= lblSize.Height;
				var ps = activeVisualizer_.GUIObject.GetPreferredSize(proposedSize);
				ps.Width = Math.Max(lblSize.Width, ps.Width);
				ps.Height += lblSize.Height;
				return ps;
			}
		}


		#region Label Property
		readonly DataLabel propLabel_;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(Megahard.Design.ModalVisualizer<DataBoxLabelEditor>), typeof(System.Drawing.Design.UITypeEditor))]
		[Megahard.Design.ShowInSmartPanel]
		[Category("Data")]
		[ObservableProperty]
		public DataLabel Label
		{
			get { return propLabel_; }
		}
		#endregion

		Control LabelControl { get; set; }
	
		DataBoxLayoutEngine layout_;
		public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
		{
			get
			{
				if (layout_ == null)
					layout_ = new DataBoxLayoutEngine(base.LayoutEngine);
				return layout_;
			}
		}

		Control EditorControl
		{
			get { return activeVisualizer_ == null ? null : activeVisualizer_.GUIObject; }
		}

		[Browsable(false)]
		public Visualization.IDataVisualizer Visualizer
		{
			get { return activeVisualizer_; }
		}

		Data.Visualization.IDataVisualizer activeVisualizer_;

		#region Data Property
		Megahard.Data.DataObject propData_;
		[Editor(typeof(Design.DropDownEditor<DataObjectEditor>), typeof(System.Drawing.Design.UITypeEditor))]
		[TypeConverter(typeof(DataBox.DataConverter))]
		[DefaultValue(null)]
		[Design.ShowInSmartPanel]
		[Category("Data")]
		[DisplayName("Bound Data")]
		[Description("This is the data which is to be shown in the DataBox")]
		public DataObject Data
		{
			get { return propData_; }
			set
			{
				if (Data == value)
					return;
				var oldVal = Data;
				RaiseObjectChanging(new ObjectChangingEventArgs("Data", value));
				propData_ = value;
				RaiseObjectChanged(new ObjectChangedEventArgs("Data", oldVal, value));
			}
		}
		#endregion

		void ActiveVisualizer_DataError(object sender, Visualization.DataErrorEventArgs args)
		{
			OnDataError(args);
		}

		protected virtual void HandleDataChanged()
		{
			//System.Diagnostics.Trace.Write("HandleDataChanged");
			System.Diagnostics.Debug.Assert(!InvokeRequired);
			object oldVisualizer = Visualizer;
			//base.OnObjectChanging(new Megahard.Data.ObjectChangingEventArgs("Visualizer", null));
			if (Error)
				ExitErrorMode();

			if (activeVisualizer_ != null)
			{
				var copy = activeVisualizer_;
				activeVisualizer_ = null;
				copy.Dispose();
			}

			if (Data != null && Data.BindingEnabled)
			{
				try
				{
					var ed = TypeDescriptor.GetEditor(Data, typeof(Visualization.VisualTypeEditor)) as Visualization.VisualTypeEditor;
					if (ed != null)
						activeVisualizer_ = ed.CreateVisualizer(VisualizerStyle);
					else
						activeVisualizer_ = Visualization.VisualUITypeEditor.CreateVisualizer();
					activeVisualizer_.AllowEditing = !ReadOnly;
					activeVisualizer_.Data = Data;
					activeVisualizer_.DataError += ActiveVisualizer_DataError;
				}
				catch (Exception ex)
				{
					if (activeVisualizer_ != null)
						activeVisualizer_.Dispose();

					activeVisualizer_ = new Visualization.ExceptionVisualizer() { Data = new DataObject(ex) };
					activeVisualizer_.DataError += ActiveVisualizer_DataError;
					EnterErrorMode();
				}
				Controls.Add(activeVisualizer_.GUIObject);
				UpdateLabelControl();
			}
			else
			{
			}
			//base.OnObjectChanged(new Megahard.Data.ObjectChangedEventArgs("Visualizer", oldVisualizer, Visualizer));
		}

		protected override void OnInitialized()
		{
			if (Data != null)
			{
				HandleDataChanged();
			}

			base.OnInitialized();
		}

		class DataConverter : TypeConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string) && context != null)
				{
					if (value is DataObject)
					{
						IReferenceService refServ = context.GetService(typeof(IReferenceService)) as IReferenceService;
						if (refServ != null)
						{
							var data = value as DataObject;
							object instance;
							Megahard.Data.PropertyPath prop;
							data.GetBindingDetails(out instance, out prop);
							if(prop != null)
							{
								return string.Format("{0}.{1}", refServ.GetName(instance), prop);
							}
							return refServ.GetName(instance);
						}
					}
					else if (value == null)
					{
						return "(unbound)";
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

		}


		class DataBoxLayoutEngine : System.Windows.Forms.Layout.LayoutEngine
		{
			readonly System.Windows.Forms.Layout.LayoutEngine baseLayout_;
			public DataBoxLayoutEngine(System.Windows.Forms.Layout.LayoutEngine baseLayout)
			{
				baseLayout_ = baseLayout;
			}
			public override bool Layout(object container, System.Windows.Forms.LayoutEventArgs layoutEventArgs)
			{
				if(baseLayout_ != null)
					baseLayout_.Layout(container, layoutEventArgs);
				DataBox dbox = container as DataBox;
				if(dbox == null)
					return base.Layout(container, layoutEventArgs);

				int edX = 0;
				int edY = 0;
				int edW = dbox.Width;
				int edH = dbox.Height;

				if (dbox.LabelControl != null)
				{
					dbox.LabelControl.Left = 0;
					dbox.LabelControl.Top = 0;

					var lblSize = dbox.Label.Size;

					if (dbox.Label.Placement == DataLabel.LabelPlacement.Left)
					{
						if (dbox.Height < dbox.LabelControl.Height)
							dbox.Height = dbox.LabelControl.Height;

						int lblWidth = dbox.Label.Size.CalculatePixelSize(dbox.Width);
						var minSize = dbox.LabelControl.MinimumSize;
						var maxSize = dbox.LabelControl.MaximumSize;
						if (dbox.Label.Size != DataLabel.LabelSize.Auto)
							minSize.Width = lblWidth;
						else
							minSize.Width = 0;
						maxSize.Width = lblWidth;
						dbox.LabelControl.MinimumSize = minSize;
						dbox.LabelControl.MaximumSize = maxSize;
						dbox.LabelControl.Size = dbox.LabelControl.PreferredSize;

						edX = dbox.LabelControl.Width;
						edY = 0;
						edH = dbox.Height;
						edW = dbox.Width - dbox.LabelControl.Width;

					}
					else if (dbox.Label.Placement == DataLabel.LabelPlacement.Top)
					{
						if (dbox.Height < dbox.LabelControl.Height * 2)
							dbox.Height = dbox.LabelControl.Height * 2;

						edY = dbox.LabelControl.Height;
						edX = 0;
						edW = dbox.Width;
						dbox.LabelControl.Width = dbox.Width;
						edH = Math.Max(dbox.Height - dbox.LabelControl.Height, dbox.LabelControl.Height);
					}
				}


				if (dbox.EditorControl != null)
				{
					dbox.EditorControl.Left = edX;
					dbox.EditorControl.Top = edY;
					dbox.EditorControl.Width = edW;
					dbox.EditorControl.Height = edH;
					//if (dbox.EditorControl.Height < edH)
					//	dbox.Height = edY + dbox.EditorControl.Height;
				}

				return true;
			}

		}

		protected override void OnObjectChanged(ObjectChangedEventArgs args)
		{
			var prop = args.PropertyName;
			if ((prop.Root == "Data" && (Error || args.IsObjectReset)) || (prop == "Data" && (args.TypeChanged || !args.IsChild || args.IsObjectReset)) || args.PropertyName == "Data.BindingEnabled")
			{
				this.AutoBeginInvoke(HandleDataChanged);
			}

			if (prop.Root == "Label")
			{
				UpdateLabelControl();
			}
			if (prop == "Label.Placement" || prop == "Label.Size")
				PerformLayout();

			base.OnObjectChanged(args);
		}

		/// <summary>
		/// Forces DataBox to re-examine the Data object and create a new visualizer
		/// </summary>
		public void ReloadVisualizer()
		{
			this.AutoBeginInvoke(HandleDataChanged);
		}

		void UpdateLabelControl()
		{
			if (Label.Visible)
			{
				if (LabelControl == null)
				{
					LabelControl = new KryptonWrapLabel() { AutoSize = false };
					//LabelControl = new Megahard.Controls.MegaLabel() { AutoSize = false};
					LabelControl.MouseClick += DataBox_MouseClick;
					Controls.Add(LabelControl);
				}
				LabelControl.Text = Label.GetParsedText();
			}
			else
			{
				if (LabelControl != null)
				{
					var ctl = LabelControl;
					LabelControl = null;
					ctl.MouseClick -= DataBox_MouseClick;
					ctl.Dispose();
				}
			}
		}
		
		class ErrorIndicator : KryptonLinkLabel
		{
			public ErrorIndicator(Exception e, DataBox db)
			{
				ex_ = e;
				db_ = db;
				base.Text = "data error";
			}

			DataBox db_;
			readonly Exception ex_;
			protected override void OnLinkClicked(LinkClickedEventArgs e)
			{
				if (db_ != null)
				{
					db_.ReloadVisualizer();
					if (!db_.Error)
					{
						db_ = null;
						return;
					}
				}

				base.OnLinkClicked(e);
				MessageBox.Show(ex_.Message + Environment.NewLine + ex_.TargetSite.DeclaringType + "." + ex_.TargetSite.Name, ex_.GetType().Name);
			}
		}

		private void DataBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && Data != null && !Data.IsDefault)
			{
				CreateContextMenu();
				if (_contextMenu != null)
					_contextMenu.Show(this);
			}
		}
	}
}

