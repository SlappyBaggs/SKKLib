namespace SKKLib.Serial.Interface
{
    public interface ISKKLink
    {
        byte MacID { get; set; }
        string SendCommand(string msg);
        string SendCommand(string msg, int delay);
        //ISKKReader GetReader { get; }
        //ISKKWriter GetWriter { get; }
    }
}
