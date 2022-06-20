using System.ComponentModel;

namespace SKKLib.Serial.Interface
{
    [TypeConverter(typeof(Data.SKKSerialPortCTypeConverter))]
    public interface ISKKSerialPortC : ISKKSerialPort
    {
        string PortCName { get; }
    }
}
