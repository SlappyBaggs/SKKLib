using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Megahard.Collections.Specialized;

namespace Megahard.SerialIO.RS485
{
	/// <summary>
	/// This class is responsible for implementing the IN2 RS485 protocol onto a datastream, decoding data from the stream and dispatching it into the application
	/// as well as encoding new data onto the stream as requested by the application
	/// </summary>
	public class MessageReader
	{
		public MessageReader(System.IO.Stream stream)
		{
			if (!stream.CanRead)
				throw new ArgumentException("MessageReader requires a readable stream", "stream");
			stream_ = stream;
            stream_.ReadTimeout = 1000;
		}

		enum State { Wait4STX, Wait4ETX, MsgRecd }


		const byte STX = 0x02;
		const byte ETX = 0x03;


		System.IO.Stream stream_;

		/// <summary>
		/// Reads one message, blocks if necessary
		/// </summary>
		/// <returns></returns>
		public RawMessage ReadMessage()
		{
			var bb = Bytes.Build(12);
			var state = State.Wait4STX;
			while (state != State.MsgRecd)
			{
				int val = stream_.ReadByte();
				if (val == -1)
					return new RawMessage();

				byte byteVal = (byte)val;
				switch (state)
				{
					case State.Wait4STX:
						if (byteVal == STX)
						{
							bb.Add(STX);
							state = State.Wait4ETX;
						}
						break;
					case State.Wait4ETX:
						bb.Add(byteVal);
						if (byteVal == ETX)
							state = State.MsgRecd;
						break;
					default:
						throw new InvalidOperationException("State enum has invalid value");
				}
			}
			return new RawMessage(bb.ToBytes());
		}
	}
}
