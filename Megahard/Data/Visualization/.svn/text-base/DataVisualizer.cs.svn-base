using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Megahard.Data.Visualization
{
	[ToolboxItem(false)]
	[Designer(typeof(DataVisualizer.DataVisualizerDocDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
	[TypeDescriptionProvider(typeof(DataVisualizer.dvTypeDescriptor.Provider))]
	public partial class DataVisualizer : Megahard.Controls.UserControlBase, IDataVisualizer
	{
		protected class DataVisualizerDocDesigner : Megahard.Controls.UserControlBase.UserControlDocDesigner
		{
			public DataVisualizerDocDesigner()
			{
			}
			protected override void PostFilterProperties(System.Collections.IDictionary properties)
			{
				base.PostFilterProperties(properties);
				PropertyDescriptor oldDataProp = (PropertyDescriptor)properties["Data"];
				properties.Remove("Data");
				properties.Remove("DataBindings");
				//MessageBox.Show("Post filter properties");
				properties["Data"] = oldDataProp.ChangePropertyType((base.Component as DataVisualizer).VisualizerType ?? typeof(object));
			}
		}
		public DataVisualizer()
		{
		}

		class dvTypeDescriptor : Megahard.ComponentModel.InstanceOfTypeDescriptor<dvTypeDescriptor, DataVisualizer>
		{
			public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
			{
				var props = OriginalTypeDescriptor.GetProperties(attributes);
				PropertyDescriptor[] newProps = new PropertyDescriptor[props.Count];
				for (int i = 0; i < props.Count; ++i)
				{
					var prop = props[i];
					if (prop.Name == "Data" && Instance.VisualizerType != null)
					{
						newProps[i] = prop.ChangePropertyType(Instance.VisualizerType);
					}
					else
					{
						newProps[i] = prop;
					}
				}
				return new PropertyDescriptorCollection(newProps);
			}

		}


		protected object GetDataValue()
		{
			var data = Data;
			if (data == null)
				return null;
			return data.GetValue();
		}

		protected T GetDataValue<T>()
		{
			var data = Data;
			if (data == null)
				return default(T);
			return data.GetValue<T>();
		}

		protected T GetDataValueAs<T>() where T : class
		{
			var data = Data;
			if (data == null)
				return null;
			return data.GetValueAs<T>();
		}

		protected void SetDataValue(object val)
		{
			if (Data == null)
				Data = new DataObject(val);
			else
				Data.SetValue(val);
		}

		public DialogResult ShowAsDialog(IWin32Window owner = null)
		{
			return ShowAsDialog(this, owner);
		}
		public static DialogResult ShowAsDialog(IDataVisualizer vis, IWin32Window owner = null)
		{
			using (var frm = new Design.ModalVisualizerForm(vis))
			{
				return frm.ShowDialog(owner);
			}
		}

		public virtual string GetStringRepresentation()
		{
			try
			{
				var val = GetDataValue();
				return val != null ? val.ToString() : "";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		private Type visualizerType_;
		[DefaultValue(null)]
		[Editor(typeof(Design.TypeBrowserEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public Type VisualizerType
		{
			get
			{
				return visualizerType_;
			}
			set
			{
				if (visualizerType_ == value)
					return;
				visualizerType_ = value;
				TypeDescriptor.Refresh(this);
			}
		}

		//<ObservableProperty Name="Data" Type="DataObject" DefaultValue="null"/>
		//<ObservableProperty Name="AllowEditing" Type="bool" DefaultValue="false"/>
		//<ObservableProperty Name="UpdateMode" Type="UpdateMode" DefaultValue="UpdateMode.Automatic" Category='"Data"'/>
		//<ObservableProperty Name="CommitMode" Type="CommitMode" DefaultValue="CommitMode.Automatic" Category='"Data"'/>

		protected bool AutoCommit
		{
			get { return CommitMode == CommitMode.Automatic; }
		}

		protected bool AutoUpdate
		{
			get { return UpdateMode == UpdateMode.Automatic; }
		}

		#region IDataVisualizer Members

		public virtual void UpdateDisplay()
		{

		}

		public void CommitChanges()
		{
			OnCommitted();
		}

		protected virtual void OnCommitted()
		{
			if (Committed != null)
				Committed(this, EventArgs.Empty);
		}

		public void CancelChanges()
		{
			OnCanceled();
		}

		protected virtual void OnCanceled()
		{
			if (Canceled != null)
				Canceled(this, EventArgs.Empty);
		}

		public event EventHandler Canceled;
		public event EventHandler Committed;

		public event EventHandler<DataErrorEventArgs> DataError;
		protected virtual void OnDataError(DataErrorEventArgs arg)
		{
			var copy = DataError;
			if (copy != null)
				copy(this, arg);
		}

		[Browsable(false)]
		public Control GUIObject { get { return this; } }

		public virtual void InvokePopupEditor()
		{
		}
		#endregion

	}
}
