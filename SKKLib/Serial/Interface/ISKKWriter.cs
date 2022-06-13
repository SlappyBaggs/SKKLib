namespace SKKLib.Serial.Interface
{
    public interface ISKKWriter
    {
        void SendMessage(string cmd, byte macID);
    }
}
