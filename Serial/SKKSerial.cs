#if WHY_THIS_HERE
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ComponentModel;
using SKKLib.Serial.Data;

namespace SKKLib.Serial
{
    public class SKKSerialPort
    {
        public delegate void SKKSerialDataReceivedEventHandler();
        public event SKKSerialDataReceivedEventHandler SKKSerialDataReceivedEvent;

        private readonly Object lock_ = new Object();

        private System.IO.Ports.SerialPort port_;

        public SKKSerialPort() : this("COM1") { }
        public SKKSerialPort(String pn) : this(pn, Data.BaudRate.Baud9600) { }
        public SKKSerialPort(String pn, BaudRate br) : this(pn, br, DataBits.Eight) { }
        public SKKSerialPort(String pn, BaudRate br, DataBits db) : this(pn, br, db, Parity.None) { }
        public SKKSerialPort(String pn, BaudRate br, DataBits db, Parity p) : this(pn, br, db, p, StopBits.One) { }
        public SKKSerialPort(String pn, BaudRate br, DataBits db, Parity p, StopBits sb)
        {
            port_ = new System.IO.Ports.SerialPort(pn);
            
            ReadTimeout = 2000;
            
            BaudRate = br;
            DataBits = db;
            Parity = p;
            StopBits = sb;
        
            port_.DataReceived += DataReceivedFromPort;

            Handshake = Handshake.None;
            DTREnable = true;
            RTSEnable = true;
        }

        private void DataReceivedFromPort(object o, System.IO.Ports.SerialDataReceivedEventArgs args)
        {
            if (SKKSerialDataReceivedEvent != null)
                SKKSerialDataReceivedEvent();
        }

        public void Open() { IsOpen = true; }
        public void Close() { IsOpen = false; }

        private Handshake handshake_;
        [DefaultValue(Handshake.None)]
        public Handshake Handshake
        {
            get { return handshake_; }
            set 
            {
                if (IsOpen) throw new SKKSerialPortOpen("Can't set Handshake '" + value + "' on SerialPort " + portName_ + ".  Port is already open.");
                lock (lock_)
                {
                    handshake_ = value;
                    switch (handshake_)
                    {
                        case Handshake.None: port_.Handshake = System.IO.Ports.Handshake.None; break;
                        case Handshake.RequestToSend: port_.Handshake = System.IO.Ports.Handshake.RequestToSend; break;
                        case Handshake.RequestToSendXonXoff: port_.Handshake = System.IO.Ports.Handshake.RequestToSendXOnXOff; break;
                        case Handshake.XonXoff: port_.Handshake = System.IO.Ports.Handshake.XOnXOff; break;
                    }
                }
            }
        }

        private bool dtrEnable_;
        [DefaultValue(false)]
        public bool DTREnable
        {
            get { return dtrEnable_; }
            set
            {
                if (IsOpen) throw new SKKSerialPortOpen("Can't set DTREnable '" + (value ? "TRUE" : "FALSE") + "' on SerialPort " + portName_ + ". Port is already open.");
                lock (lock_)
                {
                    dtrEnable_ = value;
                    port_.DtrEnable = dtrEnable_;
                }
            }
        }

        private bool rtsEnable_;
        [DefaultValue(false)]
        public bool RTSEnable
        {
            get { return rtsEnable_; }
            set
            {
                if (IsOpen) throw new SKKSerialPortOpen("Can't set RTSEnable '" + (value ? "TRUE" : "FALSE") + "' on SerialPort " + portName_ + ". Port is already open.");
                rtsEnable_ = value;
                port_.RtsEnable = rtsEnable_;
            }
        }

        [DefaultValue(2000)]
        public int ReadTimeout
        {
            get { return port_.ReadTimeout; }
            set { port_.ReadTimeout = value; }
        }

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
                        throw new SKKSerialPortOpen("Cannot open port '" + portName_ + "'.  Port is already open.");
                    }
                    else
                    {
                        port_.Open();
                    }
                }
                else
                {
                    port_.Close();
                }
            }
        }

        private string portName_;
        [DefaultValue("COM1")]
        public string PortName
        {
            get { return portName_; }
            set
            {
                if (IsOpen) throw new SKKSerialPortOpen("Can't set PortName '" + value + "' on SerialPort " + portName_ + ".  Port is already open.");
                lock (lock_)
                {
                    portName_ = value;
                    port_.PortName = portName_;
                }
            }
        }

        private BaudRate baudRate_;
        [DefaultValue(BaudRate.Baud9600)]
        public BaudRate BaudRate
        {
            get { return baudRate_; }
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpen("Can't set BaudRate '" + value.ToString() + "' on SerialPort " + portName_ + ".  Port is already open.");
                lock (lock_)
                {
                    baudRate_ = value;
                    port_.BaudRate = (int)baudRate_;
                }
            }
        }

        private DataBits dataBits_;
        [DefaultValue(DataBits.Eight)]
        public DataBits DataBits
        {
            get { return dataBits_; }
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpen("Can't set DataBits '" + value.ToString() + "' on SerialPort " + portName_ + ".  Port is already open.");
                lock (lock_)
                {
                    dataBits_ = value;
                    port_.DataBits = (int)dataBits_;
                }
            }
        }

        private Parity parity_;
        [DefaultValue(Parity.None)]
        public Parity Parity
        {
            get { return parity_; }
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpen("Can't set Parity '" + value.ToString() + "' on SerialPort " + portName_ + ".  Port is already open.");
                lock (lock_)
                {
                    parity_ = value;
                    switch (parity_)
                    {
                        case Parity.None: port_.Parity = System.IO.Ports.Parity.None; break;
                        case Parity.Odd: port_.Parity = System.IO.Ports.Parity.Odd; break;
                        case Parity.Even: port_.Parity = System.IO.Ports.Parity.Even; break;
                        case Parity.Mark: port_.Parity = System.IO.Ports.Parity.Mark; break;
                        case Parity.Space: port_.Parity = System.IO.Ports.Parity.Space; break;
                    }
                }
            }
        }

        private StopBits stopBits_;
        [DefaultValue(StopBits.One)]
        public StopBits StopBits
        {
            get { return stopBits_; }
            set
            {
                if (port_.IsOpen) throw new SKKSerialPortOpen("Can't set StopBits '" + value.ToString() + "' on SerialPort " + portName_ + ".  Port is already open.");
                lock (lock_)
                {
                    stopBits_ = value;
                    switch (stopBits_)
                    {
                        case StopBits.None: port_.StopBits = System.IO.Ports.StopBits.None; break;
                        case StopBits.One: port_.StopBits = System.IO.Ports.StopBits.One; break;
                        case StopBits.Two: port_.StopBits = System.IO.Ports.StopBits.Two; break;
                        case StopBits.OnePointFive: port_.StopBits = System.IO.Ports.StopBits.OnePointFive; break;
                    }
                }
            }
        }

        public string ReadExisting()
        {
            return port_.ReadExisting();
        }

        public byte[] ReadAll()
        {
            int len = port_.BytesToRead;
            byte[] ba = new byte[len];
            port_.Read(ba, 0, len);
            return ba;
        }

        public byte[] ReadAllOLD()
        {
            if (!IsOpen) throw new SKKSerialPortClosed("Cannot ReadAll on SerialPort '" + portName_ + "'.  Serial port is closed.");
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
                    catch(SKKSerialTimeout)
                    {
                        throw new SKKSerialPartialTimeout(ba);
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
            if (!IsOpen) throw new SKKSerialPortClosed("Cannot ReadByte on SerialPort '" + portName_ + "'.  Serial port is closed.");
            lock(lock_)
            {
                int ret = -1;
                try
                {
                    ret = port_.ReadByte();
                }
                catch (System.TimeoutException to)
                {
                    // Don't re-throw, just let it return -1...
                    //throw new SKKSerialTimeout();
                }
                return ret;
            }
        }

        public System.IO.Stream BaseStream { get { return port_.BaseStream; } }

        public void Write(string s)
        {
            Write(Encoding.ASCII.GetBytes(s));
        }

        public void Write(byte[] b)
        {
            Write(b, 0, b.Length);
        }

        public void Write(byte[] b, int offset, int count)
        {
            if (!IsOpen) 
                throw new SKKSerialPortClosed("Cannot Write on SerialPort '" + portName_ + "'.  Serial port is closed.");
            lock (lock_)
            {
                port_.Write(b, offset, count);
            }
        }
    }
}
#endif