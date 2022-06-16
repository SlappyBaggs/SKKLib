using SKKLib.Serial.Interface;

namespace SKKLib.Serial.Base
{
    public abstract class SKKReader : ISKKReader
    {
        public ISKKSerialPort SerialPort { get; set; }

        public abstract string ReadMessage(byte macID);
    }
}
