using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Networking
{
	public class DataReceivedEventArgs : EventArgs
	{
		public DataReceivedEventArgs(Collections.Specialized.Bytes data, long timeStamp)
		{
			Data = data;
			Timestamp = timeStamp;
		}
		public Collections.Specialized.Bytes Data { get; private set; }
		public long Timestamp { get; private set; }
	}

	public enum PurgeType { RxBuffer, TxBuffer, RxAndTxBuffer };
	public interface IDataLink
	{
		void Close();
		void Transmit(Megahard.Collections.Specialized.Bytes bytes);
		void Purge(PurgeType pt);
		void BlockIncomingData();
		void SetTimestampGuard();
		bool WaitForLinkToIdle(TimeSpan idleTime, TimeSpan abortTimeout);

		bool IsOpen { get; set; }
		int TransmissionRate { get; set; }
		event EventHandler<DataReceivedEventArgs> DataReceived;
	}
}
