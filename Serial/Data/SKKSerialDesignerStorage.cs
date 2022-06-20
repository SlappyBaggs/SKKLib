using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.Serial.Data
{
    public static class SKKSerialDesignerStorage
    {
        public static Interface.ISKKSerialPortC SerialPortComponent { get; set; }

        [Browsable(false)]
        public static List<Interface.ISKKSerialPortC> SerialPortComponents { get; private set; } = new List<Interface.ISKKSerialPortC>();

        public static void AddSerialComponent(Interface.ISKKSerialPortC portC)
        {
            if (!SerialPortComponents.Contains(portC))
                SerialPortComponents.Add(portC);
        }
    }
}
