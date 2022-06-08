using SKKLib.Serial.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.Serial.Controls
{
    [DesignerAttribute(typeof(SKKSerialPortCDesigner), typeof(IDesigner))]
    public partial class SKKSerialPortC : Component, Interface.ISKKSerialPort, Interface.ISKKSerialPortC
    {
        #region CONSTRUCTORS
        /*
         * Call this(SerialDefaults.PortName) to force the component constructor implementations to call our base constructor
         */
        public SKKSerialPortC() : this(SerialPortDefaults.PortName)
        {
            InitializeComponent();
        }

        public string PortCName { get => TypeDescriptor.GetComponentName(this); }

        public SKKSerialPortC(IContainer container) : this(SerialPortDefaults.PortName)
        {
            container.Add(this);
            InitializeComponent();
        }

        //public override string PortCName { get => this.Name

        public SKKSerialPortC(
            String pn = SerialPortDefaults.PortName,
            BaudRate br = SerialPortDefaults.BaudRate,
            DataBits db = SerialPortDefaults.DataBits,
            Parity p = SerialPortDefaults.Parity,
            StopBits sb = SerialPortDefaults.StopBits
            )
        {
            // Create port and pass in our constructor parameters
            port_ = new SKKSerialPort(pn, br, db, p, sb);

            // Forward all port_'s events to our events
            port_.SKKPortDataReceivedEvent += DataReceivedFromPort;
            port_.SKKPortOpenedClosedEvent += OnSerialPortOpenedClosed;
            port_.SKKPortHandshakeChangedEvent += OnSerialPortHandshakeChanged;
            port_.SKKPortDTREnableChangedEvent += OnSerialPortDTREnableChanged;
            port_.SKKPortRTSEnableChangedEvent += OnSerialPortRTSEnableChanged;
            port_.SKKPortReadTimeoutChangedEvent += OnSerialPortReadTimeoutChanged;
            port_.SKKPortPortNameChangedEvent += OnSerialPortPortNameChanged;
            port_.SKKPortBaudRateChangedEvent += OnSerialPortBaudRateChanged;
            port_.SKKPortDataBitsChangedEvent += OnSerialPortDataBitsChanged;
            port_.SKKPortParityChangedEvent += OnSerialPortParityChanged;
            port_.SKKPortStopBitsChangedEvent += OnSerialPortStopBitsChanged;
        }
        #endregion

        #region LOCAL VARIABLES
        //private System.IO.Ports.SerialPort port_;
        private Interface.ISKKSerialPort port_;
        #endregion

        #region FORWARDED/SHADOWED EVENTS
        public event SKKPortDataReceived_EH SKKPortDataReceivedEvent = delegate { };
        private void DataReceivedFromPort(System.IO.Ports.SerialDataReceivedEventArgs args) => SKKPortDataReceivedEvent(args);

        public event SKKPortOpenedClosed_EH SKKPortOpenedClosedEvent = delegate { };
        private void OnSerialPortOpenedClosed(OpenedClosed oc) => SKKPortOpenedClosedEvent(oc);

        public event SKKPortHandshakeChanged_EH SKKPortHandshakeChangedEvent = delegate { };
        private void OnSerialPortHandshakeChanged(Handshake hs) => SKKPortHandshakeChangedEvent(hs);

        public event SKKPortDTREnableChanged_EH SKKPortDTREnableChangedEvent = delegate { };
        private void OnSerialPortDTREnableChanged(bool b) => SKKPortDTREnableChangedEvent(b);

        public event SKKPortRTSEnableChanged_EH SKKPortRTSEnableChangedEvent = delegate { };
        private void OnSerialPortRTSEnableChanged(bool b) => SKKPortRTSEnableChangedEvent(b);

        public event SKKPortReadTimeoutChanged_EH SKKPortReadTimeoutChangedEvent = delegate { };
        private void OnSerialPortReadTimeoutChanged(int i) => SKKPortReadTimeoutChangedEvent(i);

        public event SKKPortPortNameChanged_EH SKKPortPortNameChangedEvent = delegate { };
        private void OnSerialPortPortNameChanged(string n) => SKKPortPortNameChangedEvent(n);

        public event SKKPortBaudRateChanged_EH SKKPortBaudRateChangedEvent = delegate { };
        private void OnSerialPortBaudRateChanged(BaudRate br) => SKKPortBaudRateChangedEvent(br);

        public event SKKPortDataBitsChanged_EH SKKPortDataBitsChangedEvent = delegate { };
        private void OnSerialPortDataBitsChanged(DataBits db) => SKKPortDataBitsChangedEvent(db);

        public event SKKPortParityChanged_EH SKKPortParityChangedEvent = delegate { };
        private void OnSerialPortParityChanged(Parity p) => SKKPortParityChangedEvent(p);

        public event SKKPortStopBitsChanged_EH SKKPortStopBitsChangedEvent = delegate { };
        private void OnSerialPortStopBitsChanged(StopBits sb) => SKKPortStopBitsChangedEvent(sb);
        #endregion

        #region FORWARDED PROPERTIES
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

        #region FORWARDED FUNCTIONS
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
