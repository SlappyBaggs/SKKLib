namespace SKKLib.Serial.Interface
{
    public interface ISKKReader
    {
        ISKKSerialPort SerialPort { get; set; }
        string ReadMessage(byte macID);
    }
}
