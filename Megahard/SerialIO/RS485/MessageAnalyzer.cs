using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Megahard.Collections.Specialized;

namespace Megahard.SerialIO.RS485
{
	/// <summary>
	/// Analyzes raw byte messages and turns them into instances of the Message class if they are valid
	/// </summary>
	public class MessageAnalyzer
	{
		const string pattern = @"([0-9|A-F][0-9|A-F])([0-9|A-F][0-9|A-F])([0-9|A-F][0-9|A-F])([0-9|A-F][0-9|A-F])(.*)";

		static readonly Regex regExMsg = new Regex(pattern);

		/// <summary>
		/// returns an instance of Message if the rawMsg is valid, otherwise null
		/// </summary>
		public Message Analyze(RawMessage rawMsg)
		{
			if (!rawMsg.Valid || rawMsg.Bytes.Length < 12)
			{
				OnInvalidMessage(rawMsg);
				return null;
			}
			if (!VerifyCheckSum(rawMsg.Bytes))
			{
				OnChkSumFailed(rawMsg);
				return null;
			}
			try
			{
				var match = regExMsg.Match(rawMsg.Bytes.ToUTF8String(1, rawMsg.Bytes.Length - 4));
				if (match.Success)
				{
					byte srcAddr = byte.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
					byte destAddr = byte.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
					var cmd = (MessageType)byte.Parse(match.Groups[3].Value, System.Globalization.NumberStyles.HexNumber);
					byte dataLen = byte.Parse(match.Groups[4].Value, System.Globalization.NumberStyles.HexNumber);
					string data = match.Groups[5].Value;
					return new Message(srcAddr, destAddr, cmd, data);
				}
			}
			catch
			{

			}
			OnInvalidMessage(rawMsg);
			return null;
		}

		void OnInvalidMessage(RawMessage msg)
		{
		}

		void OnChkSumFailed(RawMessage msg)
		{
		}

		static bool VerifyCheckSum(Bytes bytes)
		{
			byte sum = 0;
			foreach (byte b in bytes.Slice(0, bytes.Length - 3))
				sum += b;

			int suppliedChkSum = int.Parse(bytes.ToUTF8String(bytes.Length - 3, 2), System.Globalization.NumberStyles.HexNumber);
			return sum == suppliedChkSum;
		}
	}

}
