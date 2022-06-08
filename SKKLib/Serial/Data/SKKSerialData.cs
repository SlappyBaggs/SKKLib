namespace SKKLib.Serial.Data
{
    #region ENUMS
    public enum BaudRate
    {
        Baud110 = 110,
        Baud300 = 300,
        Baud600 = 600,
        Baud1200 = 1200,
        Baud2400 = 2400,
        Baud4800 = 4800,
        Baud9600 = 9600,
        Baud14400 = 14400,
        Baud19200 = 19200,
        Baud28800 = 28800,
        Baud38400 = 38400,
        Baud56000 = 56000,
        Baud57600 = 57600,
        Baud115200 = 115200
    }

    public enum Parity
    {
        None = System.IO.Ports.Parity.None,
        Odd = System.IO.Ports.Parity.Odd,
        Even = System.IO.Ports.Parity.Even,
        Mark = System.IO.Ports.Parity.Mark,
        Space = System.IO.Ports.Parity.Space
    }

    public enum DataBits
    {
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8
    }

    public enum StopBits
    {
        None = System.IO.Ports.StopBits.None,
        One = System.IO.Ports.StopBits.One,
        Two = System.IO.Ports.StopBits.Two,
        OnePointFive = System.IO.Ports.StopBits.OnePointFive
    }

    public enum Handshake
    {
        None = System.IO.Ports.Handshake.None,
        RequestToSend = System.IO.Ports.Handshake.RequestToSend,
        XonXoff = System.IO.Ports.Handshake.XOnXOff,
        RequestToSendXonXoff = System.IO.Ports.Handshake.RequestToSendXOnXOff
    }
    
    public enum Channel
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    public enum OpenedClosed
    {
        Closed = 0,
        Opened = 1
    }
    #endregion

    #region DATA CLASSES & STRUCTS
    public static class SerialPortDefaults
    {
        public const BaudRate BaudRate = Data.BaudRate.Baud9600;
        public const DataBits DataBits = Data.DataBits.Eight;
        public const Parity Parity = Data.Parity.None;
        public const StopBits StopBits = Data.StopBits.One;

        public const int ReadTimeout = 2000;
        public const Handshake Handshake = Data.Handshake.None;
        public const bool DTREnable = true;
        public const bool RTSEnable = true;
        public const bool IsOpen = false;

        public const string PortName = "COM1";

        public static readonly string[] PortNames = System.IO.Ports.SerialPort.GetPortNames();
    }
    #endregion

    #region EVENT DELEGATES
    public delegate void SKKPortDataReceived_EH(System.IO.Ports.SerialDataReceivedEventArgs args);
    public delegate void SKKPortOpenedClosed_EH(OpenedClosed oc);
    public delegate void SKKPortHandshakeChanged_EH(Handshake hs);
    public delegate void SKKPortDTREnableChanged_EH(bool b);
    public delegate void SKKPortRTSEnableChanged_EH(bool b);
    public delegate void SKKPortReadTimeoutChanged_EH(int i);
    public delegate void SKKPortPortNameChanged_EH(string n);
    public delegate void SKKPortBaudRateChanged_EH(BaudRate br);
    public delegate void SKKPortDataBitsChanged_EH(DataBits db);
    public delegate void SKKPortParityChanged_EH(Parity p);
    public delegate void SKKPortStopBitsChanged_EH(StopBits sb);
    #endregion
}
