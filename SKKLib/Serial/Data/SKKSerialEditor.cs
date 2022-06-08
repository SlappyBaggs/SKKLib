using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SKKLib.Serial.Data
{
    public class SKKSerialPortCEditor : UITypeEditor
    {
        private IWindowsFormsEditorService svc_;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            svc_ = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            KryptonListBox lb = new KryptonListBox();
            lb.SelectionMode = SelectionMode.One;
            lb.SelectedValueChanged += (o, e) => { svc_.CloseDropDown(); };
            lb.DisplayMember = "PortCName";
            foreach (Interface.ISKKSerialPortC portC in SKKSerialDesignerStorage.SerialPortComponents)
            {
                int index = lb.Items.Add(portC);
                if(portC.Equals(value)) lb.SelectedIndex = index;   
            }
            lb.Height = lb.PreferredSize.Height;
            lb.ListBox.IntegralHeight = true;
            svc_.DropDownControl(lb);
            
            return (lb.SelectedItem == null) ? value : lb.SelectedItem;
        }
    }
}
