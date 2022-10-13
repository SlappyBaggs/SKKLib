using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.Serial.Data
{
    /// <summary>
    /// The s k k serial designer storage.
    /// </summary>
    public static class SKKSerialDesignerStorage
    {
        /// <summary>
        /// Gets or sets the serial port component.
        /// </summary>
        public static Interface.ISKKSerialPortC SerialPortComponent { get; set; }

        /// <summary>
        /// Gets the serial port components.
        /// </summary>
        [Browsable(false)]
        public static List<Interface.ISKKSerialPortC> SerialPortComponents { get; private set; } = new List<Interface.ISKKSerialPortC>();

        /// <summary>
        /// Adds the serial component.
        /// </summary>
        /// <param name="portC">The port c.</param>
        public static void AddSerialComponent(Interface.ISKKSerialPortC portC)
        {
            if (!SerialPortComponents.Contains(portC))
                SerialPortComponents.Add(portC);
        }
    }
}
