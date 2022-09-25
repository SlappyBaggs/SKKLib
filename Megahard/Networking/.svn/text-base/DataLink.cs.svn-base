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

		/// <summary>
		/// The timestamp at which the data was received (as returned from System.Diagnostics.StopWatch.GetTimestamp()
		/// </summary>
		public long Timestamp { get; private set; }
	}

	public enum PurgeType { RxBuffer, TxBuffer, RxAndTxBuffer };
	public interface IDataLink
	{
		/// <summary>
		/// Same as setting IsOpen = false
		/// </summary>
		void Close();
		void Transmit(Megahard.Collections.Specialized.Bytes bytes);
		void Purge(PurgeType pt);

		/// <summary>
		/// Prevents incoming data from firing the DataReceived event
		/// Use SetTimestampGuard to re-enable incoming data 
		/// </summary>
		void BlockIncomingData();

		/// <summary>
		/// Any incoming data with a timestamp less that the timestamp at the time of this call 
		/// will be blocked from triggering data received event
		/// </summary>
		void SetTimestampGuard();
	
		/// <summary>
		/// Blocks until there has been no send or receive activity for a given period of time
		/// </summary>
		/// <param name="idleTime">Amount of time the line must idle before this func returns</param>
		/// <param name="abortTimeout">Amount of time that must elapse before the func returns regardless of line idle state</param>
		/// <returns>True if the line idle time condition was satisfied, false if the function returned because abortTime was reached</returns>
		bool WaitForLinkToIdle(TimeSpan idleTime, TimeSpan abortTimeout);

		bool IsOpen { get; set; }

		/// <summary>
		/// This is the transmission speed we are requesting from the DataLink (units are bps)
		/// If the datalink ignore this setting it should throw a NotSupportedException
		/// If it supports it but does not support the supplied value, it should throw an InvalidOperationException
		/// </summary>
		int TransmissionRate { get; set; }

		/// <summary>
		/// Fired when data is received on the link, guaranteed to be serialized
		/// </summary>
		event EventHandler<DataReceivedEventArgs> DataReceived;
	}
}
