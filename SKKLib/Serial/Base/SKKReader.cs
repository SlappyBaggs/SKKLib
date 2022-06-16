using SKKLib.Serial.Interface;

namespace SKKLib.Serial.Base
{
    public abstract class SKKReader : ISKKReader
    {
        public ISKKSerialPort SerialPort { get; set; }

        //public SKKReader() { }

        public abstract string ReadMessage(byte macID);
    }
}
