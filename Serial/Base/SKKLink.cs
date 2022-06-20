using System;
using System.Threading;
using SKKLib.Serial.Interface;

namespace SKKLib.Serial.Base
{
    public abstract class SKKLink<R, W> : ISKKLink where R:ISKKReader, new() where W:ISKKWriter, new()
    {
        public SKKLink(ISKKSerialPort port, byte macID)
        {
            MacID = macID;
            SerialPort = port;
        }

        public byte MacID { get; set; }
        private ISKKReader reader_;
        private ISKKWriter writer_;
        private ISKKSerialPort serialPort_;

        private Object myLock = new Object();

        public ISKKSerialPort SerialPort
        {
            get => serialPort_;
            set
            {
                serialPort_ = value;
                if (serialPort_ == null)
                {
                    reader_ = null;
                    writer_ = null;
                }
                else
                {
                    (reader_ = new R()).SerialPort = serialPort_;
                    (writer_ = new W()).SerialPort = serialPort_;
                }
            }
        }

        private void SendData(string msg) => writer_.SendMessage(msg, MacID);

        private string GetData() => reader_.ReadMessage(MacID);

        public string SendCommand(string msg) => SendCommand(msg, 0);

        public string SendCommand(string msg, int delay)
        {
            lock (myLock)
            {
                SendData(msg);
                Thread.Sleep(delay);
                return GetData();
            }
        }
    }
}
