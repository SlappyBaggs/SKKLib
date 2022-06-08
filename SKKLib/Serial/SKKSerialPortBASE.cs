using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SKKLib.Serial.Data;

namespace SKKLib.Serial
{
    public class SKKSerialPortBASE : Interface.ISKKSerialPort
    {
        #region CONSTRUCTOR
        public SKKSerialPortBASE(
            String pn = SerialPortDefaults.PortName,
            BaudRate br = SerialPortDefaults.BaudRate,
            DataBits db = SerialPortDefaults.DataBits,
            Parity p = SerialPortDefaults.Parity,
            StopBits sb = SerialPortDefaults.StopBits
            )
        {
            port_ = new System.IO.Ports.SerialPort(pn);
            
            ReadTimeout = SerialPortDefaults.ReadTimeout;
            
            BaudRate = br;
            DataBits = db;
            Parity = p;
            StopBits = sb;
        
            port_.DataReceived += DataReceivedFromPort;

            Handshake = SerialPortDefaults.Handshake;
            DTREnable = SerialPortDefaults.DTREnable;
            RTSEnable = SerialPortDefaults.RTSEnable;
        }
        #endregion

        #region LOCAL VARIABLES
        private readonly Object lock_ = new Object();

        private System.IO.Ports.SerialPort port_;
        #endregion

        #region EVENTS
        public event SKKPortDataReceived_EH SKKPortDataReceivedEvent;
        private void DataReceivedFromPort(object o, System.IO.Ports.SerialDataReceivedEventArgs args) { if (SKKPortDataReceivedEvent != null) SKKPortDataReceivedEvent(args); }

        public event SKKPortOpenedClosed_EH SKKPortOpenedClosedEvent;
        private void OnPortOpenedClosed(OpenedClosed oc) { if (SKKPortOpenedClosedEvent != null) SKKPortOpenedClosedEvent(oc); }

        public event SKKPortHandshakeChanged_EH SKKPortHandshakeChangedEvent;
        private void OnPortHandshakeChanged(Handshake hs) { if (SKKPortHandshakeChangedEvent != null) SKKPortHandshakeChangedEvent(hs); }

        public event SKKPortDTREnableChanged_EH SKKPortDTREnableChangedEvent;
        private void OnPortDTREnableChanged(bool b) { if (SKKPortDTREnableChangedEvent != null) SKKPortDTREnableChangedEvent(b); }

        public event SKKPortRTSEnableChanged_EH SKKPortRTSEnableChangedEvent;
        private void OnPortRTSEnableChanged(bool b) { if (SKKPortRTSEnableChangedEvent != null) SKKPortRTSEnableChangedEvent(b); }

        public event SKKPortReadTimeoutChanged_EH SKKPortReadTimeoutChangedEvent;
        private void OnPortReadTimeoutChanged(int i) { if (SKKPortReadTimeoutChangedEvent != null) SKKPortReadTimeoutChangedEvent(i); }

        public event SKKPortPortNameChanged_EH SKKPortPortNameChangedEvent;
        private void OnPortPortNameChanged(string n) { if (SKKPortPortNameChangedEvent != null) SKKPortPortNameChangedEvent(n); }

        public event SKKPortBaudRateChanged_EH SKKPortBaudRateChangedEvent;
        private void OnPortBaudRateChanged(BaudRate br) { if (SKKPortBaudRateChangedEvent != null) SKKPortBaudRateChangedEvent(br); }

        public event SKKPortDataBitsChanged_EH SKKPortDataBitsChangedEvent;
        private void OnPortDataBitsChanged(DataBits db) { if (SKKPortDataBitsChangedEvent != null) SKKPortDataBitsChangedEvent(db); }

        public event SKKPortParityChanged_EH SKKPortParityChangedEvent;
        private void OnPortParityChanged(Parity p) { if (SKKPortParityChangedEvent != null) SKKPortParityChangedEvent(p); }

        public event SKKPortStopBitsChanged_EH SKKPortStopBitsChangedEvent;
        private void OnPortStopBitsChanged(StopBits sb) { if (SKKPortStopBitsChangedEvent != null) SKKPortStopBitsChangedEvent(sb); }
        #endregion

        #region PROPERTIES
        [DefaultValue(SerialPortDefaults.Handshake)]
        public Handshake Handshake
        {
            get => (Handshake)port_.Handshake;
            set
            {
                if (IsOpen) throw new SKKSerialPortOpenException("Can't set Handshake '" + value + "' on SerialPort " + PortName + ".  Port is already open.");
                lock (lock_)
                {
                    if (port_.Handshake != (System.IO.Ports.Handshake)value)
                    {
                        port_.Handshake = (System.IO.Ports.Handshake)value;
                        OnPortHandshakeChanged(value);
                    }
                }
            }
        }

        [DefaultValue(SerialPortDefaults.DTREnable)]
        public bool DTREnable
        {
            get => port_.DtrEnable;
            set
            {
                if (IsOpen) throw new SKKSerialPortOpenException("Can't set DTREnable '" + (value ? "TRUE" : "FALSE") + "' on SerialPort " + PortName + ". Port is already open.");
                lock (lock_)
                {
                    if (port_.DtrEnable != value)
                    {
                        port_.DtrEnable = value;
                        OnPortDTREnableChanged(value);
                    }
                }
            }
        }

        [DefaultValue(SerialPortDefaults.RTSEnable)]
        public bool RTSEnable
        {
            get => port_.RtsEnable;
            set
            {
                if (IsOpen) throw new SKKSerialPortOpenException("Can't set RTSEnable '" + (value ? "TRUE" : "FALSE") + "' on SerialPort " + PortName + ". Port is already open.");
                if (port_.RtsEnable != value)
                {
                    port_.RtsEnable = value;
                    OnPortRTSEnableChanged(value);
                }
            }
        }

        [DefaultValue(SerialPortDefaults.ReadTimeout)]
        public int ReadTimeout
        {
            get => port_.ReadTimeout;
            set
            {
                if (port_.ReadTimeout != value)
                {
                    port_.ReadTimeout = value;
                    OnPortReadTimeoutChanged(port_.ReadTimeout);
                }
            }
        }

        [DefaultValue(SerialPortDefaults.PortName)]
        public string PortName
        {
            get => port_.PortName;
            set
            {
                if (IsOpen) throw new SKKSerialPortOpenException("Can't set PortName '" + value + "' on SerialPort " + PortName + ".  Port is already open.");
                lock (lock_)
                {
                    if (port_.PortName != value)
                    {
                        port_.PortName = value;
                        OnPortPortNameChanged(port_.PortName);
                    }
                }
            }
        }

        [DefaultValue(SerialPortDefaults.BaudRate)]
        public BaudRate BaudRate
        {
            get => (BaudRate)(port_.BaudRate);
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpenException("Can't set BaudRate '" + value.ToString() + "' on SerialPort " + PortName + ".  Port is already open.");
                lock (lock_)
                {
                    if (port_.BaudRate != (int)value)
                    {
                        port_.BaudRate = (int)value;
                        OnPortBaudRateChanged(value);
                    }
                }
            }
        }

        [DefaultValue(SerialPortDefaults.DataBits)]
        public DataBits DataBits
        {
            get => (DataBits)port_.DataBits;
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpenException("Can't set DataBits '" + value.ToString() + "' on SerialPort " + PortName + ".  Port is already open.");
                lock (lock_)
                {
                    if (port_.DataBits != (int)value)
                    {
                        port_.DataBits = (int)value;
                        OnPortDataBitsChanged(value);
                    }
                }
            }
        }

        [DefaultValue(SerialPortDefaults.Parity)]
        public Parity Parity
        {
            get => (Parity)port_.Parity;
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpenException("Can't set Parity '" + value.ToString() + "' on SerialPort " + PortName + ".  Port is already open.");
                lock (lock_)
                {
                    if (port_.Parity != (System.IO.Ports.Parity)value)
                    {
                        port_.Parity = (System.IO.Ports.Parity)value;
                        OnPortParityChanged(value);
                    }
                }
            }
        }

        [DefaultValue(SerialPortDefaults.StopBits)]
        public StopBits StopBits
        {
            get => (StopBits)port_.StopBits;
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpenException("Can't set StopBits '" + value.ToString() + "' on SerialPort " + PortName + ".  Port is already open.");
                lock (lock_)
                {
                    if (port_.StopBits != (System.IO.Ports.StopBits)value)
                    {
                        port_.StopBits = (System.IO.Ports.StopBits)value;
                        OnPortStopBitsChanged(value);
                    }
                }
            }
        }
        #endregion

        #region READ & WRITE FUNCTIONS
        public void Open() => IsOpen = true;
        public void Close() => IsOpen = false;

        [DefaultValue(false)]
        public bool IsOpen
        {
            get
            {
                return port_.IsOpen;
            }
            set
            {
                if (value)
                {
                    if (IsOpen)
                    {
                        // I figure throw an exception, because you should be keeping up with the open status of this, and something is amiss    // User can always ignore the throw...
                        throw new SKKSerialPortOpenException($"Cannot open port '{PortName}'.  Port is already open.");
                    }
                    else
                    {
                        try
                        {
                            port_.Open();
                            OnPortOpenedClosed(OpenedClosed.Opened);
                        }
                        catch(Exception e)
                        {
                            throw new SKKSerialException($"Cannot access port '{PortName}': {e.Message}");
                        }
                    }
                }
                else
                {
                    port_.Close();
                    OnPortOpenedClosed(OpenedClosed.Closed);
                }
            }
        }

        public string ReadExisting() => port_.ReadExisting();

        public byte[] ReadAll()
        {
            int len = port_.BytesToRead;
            byte[] ba = new byte[len];
            port_.Read(ba, 0, len);
            return ba;
        }

        public byte[] ReadAllOLD()
        {
            if (!IsOpen) throw new SKKSerialPortClosedException("Cannot ReadAll on SerialPort '" + PortName + "'.  Serial port is closed.");
            lock (lock_)
            {
                int b = -1;
                List<byte> ba = new List<byte>();
                while (true)
                {
                    try
                    {
                        b = ReadByte();
                    }
                    catch (SKKSerialTimeoutException)
                    {
                        throw new SKKSerialPartialTimeoutException(ba);
                    }

                    if (b == -1)
                        break;

                    ba.Add((byte)b);
                }

                return ba.ToArray();
            }
        }

        public int ReadByte()
        {
            if (!IsOpen) throw new SKKSerialPortClosedException("Cannot ReadByte on SerialPort '" + PortName + "'.  Serial port is closed.");
            lock (lock_)
            {
                int ret = -1;
                try
                {
                    ret = port_.ReadByte();
                }
                catch (System.TimeoutException)
                {
                    // Don't re-throw, just let it return -1...
                    // MUST re-throw or shit breaks...
                    throw new SKKSerialTimeoutException();
                }
                return ret;
            }
        }

        [Browsable(false)]
        public System.IO.Stream BaseStream { get => port_.IsOpen ? port_.BaseStream : null; }

        public void Write(string s) => Write(Encoding.ASCII.GetBytes(s));
        
        public void Write(byte b) => Write(new byte[] { b }, 0, 1);

        public void Write(byte[] b) => Write(b, 0, b.Length);

        public void Write(byte[] b, int offset, int count)
        {
            if (!IsOpen)
                throw new SKKSerialPortClosedException("Cannot Write on SerialPort '" + PortName + "'.  Serial port is closed.");
            lock (lock_)
            {
                try
                {
                    port_.Write(b, offset, count);
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine("Caught an exception in BASE");
                }
            }
        }
    }
    #endregion
}
