using System;
using System.ComponentModel;

namespace Megahard.SerialIO
{
	public enum WriteMode { Sync, Async }
	[TypeConverter(typeof(TypeConverters.EnumDescConverter))]
	public enum BaudRate {

		[Description("1200")]
			BAUD1200 = 1200,
		[Description("2400")]
			BAUD2400 = 2400,
		[Description("4800")]
			BAUD4800 = 4800,
		[Description("9600")]
			BAUD9600 = 9600,
		[Description("19200")]
			BAUD19200 = 19200,
		[Description("38400")]
			BAUD38400 = 38400,
		[Description("57600")]
			BAUD57600 = 57600,
		[Description("115200")]
			BAUD115200 = 115200,
		[Description("128000")]
			BAUD128000 = 128000,
		[TypeConverters.OmitDescription]
		BAUDNonStandard = 0
	}
	
	public enum PortProvider	
	{
		Fax, LAT, Modem, NetworkBridge, ParallelPort, RS232, RS422, RS423,
		RS449, Scanner, TCPTelnet, Unspecified, X25	
	}

	
	public interface ISerialPortProperties
	{
		BaudRate MaxBaud { get; }
		PortProvider PortProvider { get; }
		uint MaxTxQueue { get; }
		uint MaxRxQueue { get; }
		uint CurrentTxQueue { get; }
		uint CurrentRxQueue { get; }
	}

	public class SerialPortEventArgs : EventArgs
	{
		internal SerialPortEventArgs(UInt32 mask, Int64 timestamp)
		{
			mask_ = mask;
			timestamp_ = timestamp;
		}

		public bool DSR
		{
			get { return (mask_ & EV_DSR) != 0; }
		}

		public bool CTS
		{
			get { return (mask_ & EV_CTS) != 0; }
		}
		public bool RLSD
		{
			get { return (mask_ & EV_RLSD) != 0; }
		}
		public bool Error
		{
			get { return (mask_ & EV_ERR) != 0; }
		}
		public bool Ring
		{
			get { return (mask_ & EV_RING) != 0; }
		}

		public bool Break
		{
			get { return (mask_ & EV_BREAK) != 0; }
		}

		public bool ReceivedChar
		{
			get { return (mask_ & EV_RXCHAR) != 0; }
		}

		public Int64 Timestamp { get { return timestamp_; } }

		readonly UInt32 mask_;
		readonly Int64 timestamp_;

		const UInt32 EV_DSR = 0x0010;
		const UInt32 EV_CTS = 0x0008;
		const UInt32 EV_RLSD = 0x0020;
		const UInt32 EV_ERR = 0x0080;
		const UInt32 EV_RING = 0x0100;
		const UInt32 EV_BREAK = 0x0040;
		const UInt32 EV_RXCHAR = 0x0001;
	}
	public enum Parity
	{
		None,
		Odd,
		Even,
		Mark,
		Space
	}
	
	public enum DataBits
	{
		Five = 5, Six = 6, Seven = 7, Eight = 8
	}

	public enum StopBits
	{
		One,
		OnePointFive,
		Two
	}

	public class PortNameConverter : TypeConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(System.IO.Ports.SerialPort.GetPortNames());
		}
	}

	public class SerialIOException : Exception
	{
		public SerialIOException() : base("SerialIOException") { }
		public SerialIOException(string s) : base(s) { }
	}
}
