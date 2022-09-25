using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.IO
{
	public static class mhStreamExtender
	{
		/// <summary>
		/// Reads exactly bytesToRead bytes from stream and returns as an array, if stream times out or end is reached, an exception is thrown
		/// </summary>
		public static byte[] ReadExactly(this Stream stream, int bytesToRead)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (bytesToRead < 0)
				throw new ArgumentException("bytesToRead must be >= 0");
			if (bytesToRead == 0)
				return new byte[0];
			byte[] ret = new byte[bytesToRead];
			
			int index = 0;
			while (index < bytesToRead)
			{
				//int read = stream.ReadByte();
				//if(read == -1)
				//	throw new EndOfStreamException(string.Format("End of stream reached, {0} bytes read, {1} bytes remained", index, bytesToRead - index));
				//ret[index] = (byte)read;
				//index++;
				int read = stream.Read(ret, index, bytesToRead - index);
				if (read == 0)
					throw new EndOfStreamException(string.Format("End of stream reached, {0} bytes read, {1} bytes remained", index, bytesToRead - index));
				index += read;
			}

			return ret;
		}
	}

	public static class mhTextReaderExtender
	{
		public static IEnumerable<string> ReadLines(this TextReader rdr)
		{
			string line;
			while((line = rdr.ReadLine()) != null)
				yield return line;
		}
	}
}
