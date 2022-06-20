using SKKLib.Serial.Interface;

namespace SKKLib.Serial.Base
{
    public abstract class SKKWriter : ISKKWriter
    {
        public ISKKSerialPort SerialPort { get; set; }

        public abstract void SendMessage(string cmd, byte macID);
    }
}
