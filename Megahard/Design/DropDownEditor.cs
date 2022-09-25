using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Megahard.Design
{
		class Container : System.ComponentModel.Container
		{
			public Container(IServiceProvider provider)
			{
				System.Diagnostics.Debug.Assert(provider != null);
				provider_ = provider;
			}
			
			readonly IServiceProvider provider_;

			protected override object GetService(Type service)
			{
				return base.GetService(service) ?? provider_.GetService(service);
			}
		}

	public class DropDownEditor<VisualizerType> : UITypeEditor where VisualizerType : Data.Visualization.IDataVisualizer, new()
	{
		public DropDownEditor() 
		{
			base.EditorStyle = System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}

		public Action<VisualizerType> SetupVisualizer { get; set; }
		protected override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, System.Windows.Forms.Design.IWindowsFormsEditorService formEditSvc, object currentValue)
		{
			using (VisualizerType editor = new VisualizerType())
			{
				if (SetupVisualizer != null)
					SetupVisualizer(editor);

				bool canceled = false;
				bool committed = false;
				EventHandler onCancel = (s, e) =>
					{
						canceled = true;
						formEditSvc.CloseDropDown();
					};
				EventHandler onCommit = (s, e) =>
					{
						committed = true;
						formEditSvc.CloseDropDown();
					};

				editor.Committed += onCommit;
				editor.Canceled += onCancel;

				Container cont = new Container(provider);
				cont.Add(editor.GUIObject);
				editor.Data = new Data.DataObject(currentValue);
				formEditSvc.DropDownControl(editor.GUIObject);
				if(canceled)
					return currentValue;
				if (!committed)
					editor.CommitChanges();
				return editor.Data.GetValue();
			}
		}

	}


	public class ModalVisualizer<VisualizerType> : UITypeEditor where VisualizerType : Control, Data.Visualization.IDataVisualizer, new()
	{
		ModalVisualizer()
		{
			base.EditorStyle = System.Drawing.Design.UITypeEditorEditStyle.Modal;
		}

		public Action<VisualizerType> SetupVisualizer { get; set; }
		protected override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, System.Windows.Forms.Design.IWindowsFormsEditorService formEditSvc, object currentValue)
		{
			using (VisualizerType editor = new VisualizerType())
			{
				if (SetupVisualizer != null)
					SetupVisualizer(editor);
				Container cont = new Container(provider);
				cont.Add(editor);
				editor.Data = new Data.DataObject(currentValue);
				var frm = editor as Form;
				if (frm == null)
				{
					frm = new ModalVisualizerForm(editor);
				}

				bool canceled = false;
				bool committed = false;
				EventHandler onCancel = (s, e) =>
				{
					canceled = true;
					frm.Close();
				};
				EventHandler onCommit = (s, e) =>
				{
					committed = true;
					formEditSvc.CloseDropDown();
					if (context != null && context.OnComponentChanging())
						context.OnComponentChanged();
					frm.Close();	
				};
				editor.Committed += onCommit;
				editor.Canceled += onCancel;

				formEditSvc.ShowDialog(frm);
				if (canceled)
					return currentValue;
				if (!committed)
					editor.CommitChanges();
				return editor.Data.GetValue();
			}
		}
	}
}
