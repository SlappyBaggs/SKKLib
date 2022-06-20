using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using SKKLib.Serial.Interface;

namespace SKKLib.Serial.Data
{
    public class SKKSerialPortCDesigner : ComponentDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            SKKSerialDesignerStorage.AddSerialComponent(component as Interface.ISKKSerialPortC);
        }
    }
}
