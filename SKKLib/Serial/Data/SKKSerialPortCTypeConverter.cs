using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.Serial.Data
{
    public class SKKSerialPortCTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Interface.ISKKSerialPortC portC = value as Interface.ISKKSerialPortC;
                if (portC != null) return portC.PortCName;
            }
            return "(none)";
        }
    }
}
