namespace SKKLib.Serial.Interface
{
    public interface ISKKWriter
    {
        ISKKSerialPort SerialPort { get; set; }
        void SendMessage(string cmd, byte macID);
    }
}
