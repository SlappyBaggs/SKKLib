using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Megahard.Collections.Specialized;

namespace Megahard.SerialIO.RS485
{
	public enum MessageType : byte
	{
		PointToPointCommand = 0, 
		PointToPointQuery = 1,
		BroadCastCommand = 0xfe,
		BroadCastQuery = 0xff
	}

	public class Message
	{
		public Message(byte src, byte dest, MessageType msgType, string data)
		{
			sourceAddress_ = src;
			destinationAddress_ = dest;
			messageType_ = msgType;
			data_ = data ?? string.Empty;
		}

		readonly byte sourceAddress_;
		readonly byte destinationAddress_;
		readonly MessageType messageType_;
		readonly string data_ = string.Empty;

		public byte SourceAddress
		{
			get
			{
				return sourceAddress_;
			}
		}
		public byte DestinationAddress
		{
			get
			{
				return destinationAddress_;
			}
		}

		public MessageType MessageType
		{
			get
			{
				return messageType_;
			}
		}


		/// <summary>
		/// will never be null
		/// </summary>
		public string Data
		{
			get
			{
				return data_;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3}", SourceAddress, DestinationAddress, MessageType, Data);
		}
	}

	public struct RawMessage
	{
		internal RawMessage(Bytes b)
			: this()
		{
			Bytes = b ?? Bytes.Empty;
			if (b != null)
			{
				Valid = true;
			}
		}
		public Bytes Bytes { get; private set; }
		public bool Valid { get; private set; }
	}

}
