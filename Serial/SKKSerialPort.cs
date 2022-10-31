using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using SKKLib.Serial.Data;

namespace SKKLib.Serial
{
    public class SKKSerialPort : Interface.ISKKSerialPort
    {
        #region CONSTRUCTOR
        public SKKSerialPort(
            String pn = SerialPortDefaults.PortName,
            BaudRate br = SerialPortDefaults.BaudRate,
            DataBits db = SerialPortDefaults.DataBits,
            Parity p = SerialPortDefaults.Parity,
            StopBits sb = SerialPortDefaults.StopBits
            )
        {
            // Create serial port base
            port_ = new SKKSerialPortBASE(pn, br, db, p, sb);
            
            // Hook our events into base's events
            port_.SKKPortDataReceivedEvent += DataReceivedFromPort;
            port_.SKKPortOpenedClosedEvent += OnPortOpenedClosed;
            port_.SKKPortHandshakeChangedEvent += OnPortHandshakeChanged;
            port_.SKKPortDTREnableChangedEvent += OnPortDTREnableChanged;
            port_.SKKPortRTSEnableChangedEvent += OnPortRTSEnableChanged;
            port_.SKKPortReadTimeoutChangedEvent += OnPortReadTimeoutChanged;
            port_.SKKPortPortNameChangedEvent += OnPortPortNameChanged;
            port_.SKKPortBaudRateChangedEvent += OnPortBaudRateChanged;
            port_.SKKPortDataBitsChangedEvent += OnPortDataBitsChanged;
            port_.SKKPortParityChangedEvent += OnPortParityChanged;
            port_.SKKPortStopBitsChangedEvent += OnPortStopBitsChanged;
        }
        #endregion

        #region LOCAL VARIABLES
        //private System.IO.Ports.SerialPort port_;
        private Interface.ISKKSerialPort port_;
        #endregion

        #region EVENTS
        public event SKKPortDataReceived_EH SKKPortDataReceivedEvent = delegate { };
        private void DataReceivedFromPort(System.IO.Ports.SerialDataReceivedEventArgs args) => SKKPortDataReceivedEvent(args);

        public event SKKPortOpenedClosed_EH SKKPortOpenedClosedEvent = delegate { };
        private void OnPortOpenedClosed(OpenedClosed oc) => SKKPortOpenedClosedEvent(oc);

        public event SKKPortHandshakeChanged_EH SKKPortHandshakeChangedEvent = delegate { };
        private void OnPortHandshakeChanged(Handshake hs) { SKKPortHandshakeChangedEvent(hs); }

        public event SKKPortDTREnableChanged_EH SKKPortDTREnableChangedEvent = delegate { };
        private void OnPortDTREnableChanged(bool b) { SKKPortDTREnableChangedEvent(b); }

        public event SKKPortRTSEnableChanged_EH SKKPortRTSEnableChangedEvent = delegate { };
        private void OnPortRTSEnableChanged(bool b) { SKKPortRTSEnableChangedEvent(b); }

        public event SKKPortReadTimeoutChanged_EH SKKPortReadTimeoutChangedEvent = delegate { };
        private void OnPortReadTimeoutChanged(int i) { SKKPortReadTimeoutChangedEvent(i); }

        public event SKKPortPortNameChanged_EH SKKPortPortNameChangedEvent = delegate { };
        private void OnPortPortNameChanged(string n) { SKKPortPortNameChangedEvent(n); }

        public event SKKPortBaudRateChanged_EH SKKPortBaudRateChangedEvent = delegate { };
        private void OnPortBaudRateChanged(BaudRate br) { SKKPortBaudRateChangedEvent(br); }

        public event SKKPortDataBitsChanged_EH SKKPortDataBitsChangedEvent = delegate { };
        private void OnPortDataBitsChanged(DataBits db) { SKKPortDataBitsChangedEvent(db); }

        public event SKKPortParityChanged_EH SKKPortParityChangedEvent = delegate { };
        private void OnPortParityChanged(Parity p) { SKKPortParityChangedEvent(p); }

        public event SKKPortStopBitsChanged_EH SKKPortStopBitsChangedEvent = delegate { };
        private void OnPortStopBitsChanged(StopBits sb) { SKKPortStopBitsChangedEvent(sb); }
        #endregion

        #region PROPERTIES
        [DefaultValue(SerialPortDefaults.Handshake)]
        public Handshake Handshake
        {
            get => port_.Handshake;
            set => port_.Handshake = value;
        }
        [DefaultValue(SerialPortDefaults.DTREnable)]
        public bool DTREnable
        {
            get => port_.DTREnable;
            set => port_.DTREnable = value;
        }

        [DefaultValue(SerialPortDefaults.RTSEnable)]
        public bool RTSEnable
        {
            get => port_.RTSEnable;
            set => port_.RTSEnable = value;
        }

        [DefaultValue(SerialPortDefaults.ReadTimeout)]
        public int ReadTimeout
        {
            get => port_.ReadTimeout;
            set => port_.ReadTimeout = value;
       }

        [DefaultValue(SerialPortDefaults.PortName)]
        public string PortName
        {
            get => port_.PortName;
            set => port_.PortName = value;
        }

        [DefaultValue(SerialPortDefaults.BaudRate)]
        public BaudRate BaudRate
        {
            get => port_.BaudRate;
            set => port_.BaudRate = value;
        }

        [DefaultValue(SerialPortDefaults.DataBits)]
        public DataBits DataBits
        {
            get => port_.DataBits;
            set => port_.DataBits = value;
        }

        [DefaultValue(SerialPortDefaults.Parity)]
        public Parity Parity
        {
            get => port_.Parity;
            set => port_.Parity = value;
        }

        [DefaultValue(SerialPortDefaults.StopBits)]
        public StopBits StopBits
        {
            get => port_.StopBits;
            set => port_.StopBits = value;
        }

        [DefaultValue(SerialPortDefaults.IsOpen)]
        public bool IsOpen
        {
            get => port_.IsOpen;
            set => port_.IsOpen = value;
        }

        [Browsable(false)]
        public System.IO.Stream BaseStream { get => port_.BaseStream; }
        #endregion

        #region READ & WRITE FUNCTIONS
        public void Open() => port_.IsOpen = true;

        public void Close() => port_.IsOpen = false;

        public string ReadExisting() => port_.ReadExisting();

        public byte[] ReadAll() => port_.ReadAll();

        public byte[] ReadAllOLD() => port_.ReadAllOLD();

        public int ReadByte() => port_.ReadByte();

        public void Write(string s) => port_.Write(s);

        public void Write(byte b) => port_.Write(b);

        public void Write(byte[] b) => port_.Write(b);

        public void Write(byte[] b, int offset, int count) => port_.Write(b, offset, count);
    }
    #endregion
}
