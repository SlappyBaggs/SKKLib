using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UITypeEditorEditStyleAlias = System.Drawing.Design.UITypeEditorEditStyle;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Megahard.Design
{
	/// <summary>
	/// All the Megahard.Design TypeEditors derive from this instead of System.Drawing.Design.UITypeEditor
	/// As this provides easier shared commonality of all the Megahard TypeEditors
	/// </summary>
	public class UITypeEditor : System.Drawing.Design.UITypeEditor
	{
		private UITypeEditorEditStyleAlias editorStyle_ = UITypeEditorEditStyleAlias.DropDown;
		protected virtual UITypeEditorEditStyleAlias EditorStyle
		{
			get
			{
				return editorStyle_;
			}
			set
			{
				if (editorStyle_ == value)
					return;
				editorStyle_ = value;
			}
		}
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return EditorStyle;
		}
		public sealed override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			try
			{
				if (provider == null)
					return value;

				IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (edSvc == null)
					return value;

				return EditValue(context, provider, edSvc, value);
			}
			catch (Exception ex)
			{
				if (MessageBox.Show(ex.Message + Environment.NewLine + "Do you wish to Debug?", this.GetType().FullName, MessageBoxButtons.YesNo) == DialogResult.Yes)
					System.Diagnostics.Debugger.Break();
				return value;
			}
		}

		protected virtual object EditValue(ITypeDescriptorContext context, IServiceProvider provider, IWindowsFormsEditorService formEditSvc, object currentValue)
		{
			return currentValue;
		}
	}
}

