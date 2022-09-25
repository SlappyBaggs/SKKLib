using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Megahard.SerialIO.RS485
{
	public class MessageWriter
	{
		public MessageWriter(Stream stream)
		{
			if (!stream.CanWrite)
				throw new ArgumentException("MessageWriter requires a writeable stream", "stream");

			stream_ = stream;
		}
		const byte STX = 0x02;
		const byte ETX = 0x03;

		public void WriteMessage(Message msg)
		{
			var dataBytes = Encoding.UTF8.GetBytes(msg.Data);
			List<byte> bytes = new List<byte>(12 + dataBytes.Length);

			bytes.Add(STX);
			bytes.AddRange(Encoding.UTF8.GetBytes(msg.SourceAddress.ToString("X2")));
			bytes.AddRange(Encoding.UTF8.GetBytes(msg.DestinationAddress.ToString("X2")));
			bytes.AddRange(Encoding.UTF8.GetBytes(((byte)msg.MessageType).ToString("X2")));
			bytes.AddRange(Encoding.UTF8.GetBytes(dataBytes.Length.ToString("X2")));
			bytes.AddRange(dataBytes);

			bytes.AddRange(Encoding.UTF8.GetBytes(CalculateChkSum(bytes).ToString("X2")));
			bytes.Add(ETX);
			stream_.Write(bytes.ToArray(), 0, bytes.Count);
		}

		byte CalculateChkSum(IEnumerable<byte> bytes)
		{
			byte sum = 0;
			foreach (byte val in bytes)
				sum += val;
			return sum;
		}

		readonly Stream stream_;
	}
}
