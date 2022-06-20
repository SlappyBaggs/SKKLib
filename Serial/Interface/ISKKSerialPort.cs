using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SKKLib.Serial.Data;

namespace SKKLib.Serial.Interface
{
    public interface ISKKSerialPort
    {
        #region EVENTS
        event SKKPortDataReceived_EH SKKPortDataReceivedEvent;
        event SKKPortOpenedClosed_EH SKKPortOpenedClosedEvent;
        event SKKPortHandshakeChanged_EH SKKPortHandshakeChangedEvent;
        event SKKPortDTREnableChanged_EH SKKPortDTREnableChangedEvent;
        event SKKPortRTSEnableChanged_EH SKKPortRTSEnableChangedEvent;
        event SKKPortReadTimeoutChanged_EH SKKPortReadTimeoutChangedEvent;
        event SKKPortPortNameChanged_EH SKKPortPortNameChangedEvent;
        event SKKPortBaudRateChanged_EH SKKPortBaudRateChangedEvent;
        event SKKPortDataBitsChanged_EH SKKPortDataBitsChangedEvent;
        event SKKPortParityChanged_EH SKKPortParityChangedEvent;
        event SKKPortStopBitsChanged_EH SKKPortStopBitsChangedEvent;
        #endregion

        #region PROPERTIES
        [DefaultValue(SerialPortDefaults.Handshake)]
        Handshake Handshake { get; set; }

        [DefaultValue(SerialPortDefaults.DTREnable)]
        bool DTREnable { get; set; }

        [DefaultValue(SerialPortDefaults.RTSEnable)]
        bool RTSEnable { get; set; }

        [DefaultValue(SerialPortDefaults.ReadTimeout)]
        int ReadTimeout { get; set; }

        [DefaultValue(SerialPortDefaults.PortName)]
        string PortName { get; set; }

        [DefaultValue(SerialPortDefaults.BaudRate)]
        BaudRate BaudRate { get; set; }

        [DefaultValue(SerialPortDefaults.DataBits)]
        DataBits DataBits { get; set; }

        [DefaultValue(SerialPortDefaults.Parity)]
        Parity Parity { get; set; }

        [DefaultValue(SerialPortDefaults.StopBits)]
        StopBits StopBits { get; set; }

        [DefaultValue(false)]
        bool IsOpen { get; set; }
        
        System.IO.Stream BaseStream { get; }
        #endregion

        #region READ & WRITE FUNCTIONS
        void Open();
        
        void Close();

        string ReadExisting();

        byte[] ReadAll();

        byte[] ReadAllOLD();

        int ReadByte();

        void Write(string s);

        void Write(byte b);

        void Write(byte[] b);

        void Write(byte[] b, int offset, int count);
    }
    #endregion
}
