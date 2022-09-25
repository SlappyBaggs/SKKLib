using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard.Networking
{
	public class DataLinkStream : System.IO.Stream
	{
		public DataLinkStream(IDataLink dl)
		{
			buf_ = new byte[BUFSIZE];
			dataLink_ = dl;
			bufReadPos_ = 0;
			bufWritePos_ = 0;
			readTimeout_ = 0;
			canRead_ = new System.Threading.ManualResetEvent(false);

			dl.DataReceived += ReceiveData;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
				dataLink_.DataReceived -= ReceiveData;
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			 get { return true; }
		}

		public override bool CanSeek
		{
			 get { return false; }
		}

		public override bool CanTimeout
		{
			 get { return true; }
		}


		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}

			set
			{
				throw new NotSupportedException();
			}
		}

		public override int ReadTimeout
		{
			 get { return readTimeout_; }
			 set { readTimeout_ = value; }
		}

		public override void Flush()
		{
			using(lockOb_.Lock())
			{
				bufReadPos_ = bufWritePos_;
			}
		}

		void WaitForData()
		{
			if (readTimeout_ == 0)
			{
				canRead_.WaitOne();
			}
			else
			{
				if (!canRead_.WaitOne( readTimeout_, false))
					throw new TimeoutException();
			}
		}

		public override int ReadByte()
		{
			if (!dataLink_.IsOpen)
				return -1;

			WaitForData();
            try
            {
                using (lockOb_.Lock())
                {
                    byte ret = buf_[bufReadPos_++];
                    if (bufReadPos_ == BUFSIZE)
                        bufReadPos_ = 0;

                    if (bufReadPos_ == bufWritePos_)
                        canRead_.Reset();
                    return ret;

                }
            }
            catch (System.Exception ception)
            {
                return -1;
            }
        }


		public override int Read(byte[] buffer, int offset, int count)
		{
			if(!dataLink_.IsOpen)
				return 0;

			WaitForData();
            try
            {
                using (lockOb_.Lock())
                {
                    int pos = offset;
                    for (int i = 0; i < count; ++i)
                    {
                        if (bufReadPos_ == bufWritePos_)
                        {
                            canRead_.Reset();
                            return i;
                        }
                        buffer[pos++] = buf_[bufReadPos_++];
                        if (bufReadPos_ == BUFSIZE)
                            bufReadPos_ = 0;
                    }
                    if (bufReadPos_ == bufWritePos_)
                        canRead_.Reset();
                    return count;
                }
            }
            catch (System.Exception ception)
            {
                return 0;
            }
        }

		public override void Write(byte[] buffer, int offset, int count)
		{
			dataLink_.Transmit(new Collections.Specialized.Bytes(buffer).Slice(offset, count));
		}

		public override long Seek(long l, System.IO.SeekOrigin so)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long l)
		{
			throw new NotSupportedException();
		}

        void ReceiveData(object sender, Networking.DataReceivedEventArgs args)
        {
            try
            {

                var data = args.Data;
                using (lockOb_.Lock())
                {
                    int diff = bufWritePos_ - bufReadPos_;

                    foreach (byte b in data)
                    {
                        buf_[bufWritePos_++] = b;
                        if (bufWritePos_ == BUFSIZE)
                        {
                            bufWritePos_ = 0;
                        }
                        if (bufWritePos_ == bufReadPos_)
                        {
                            bufReadPos_ = bufWritePos_ + 1;
                        }
                    }
                    canRead_.Set();
                }
            }
            catch (System.Exception ception)
            {
            
            }
        }


		readonly IDataLink dataLink_;
		readonly System.Threading.ManualResetEvent canRead_;
		readonly byte[] buf_;
		const string LockName = "DataLinkStream";
		readonly Megahard.Threading.SyncLock lockOb_ = new Megahard.Threading.SyncLock(LockName);
		const int BUFSIZE = 65536;
		int bufReadPos_;
		int bufWritePos_;
		int readTimeout_;
	}

}
