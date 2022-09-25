using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{	
	public static class BinaryReaderExtender
	{
		static int GetByteSize(TypeCode tc)
		{
			switch (tc)
			{
				case TypeCode.Boolean:
					return sizeof(bool);
				case TypeCode.Byte:
					return sizeof(byte);
				case TypeCode.Int16:
					return sizeof(short);
				case TypeCode.Int32:
					return sizeof(int);
				case TypeCode.UInt16:
					return sizeof(ushort);
				case TypeCode.UInt32:
					return sizeof(uint);
				case TypeCode.Single:
					return sizeof(float);
				case TypeCode.Double:
					return sizeof(double);
				default:
					throw new Exception("GetByteSize does not support " + tc.ToString());
			}
		}
		public static IConvertible Read(this BinaryReader reader, TypeCode tc)
		{
			switch (tc)
			{
				case TypeCode.Boolean:
					return reader.ReadBoolean();
				case TypeCode.Byte:
					return reader.ReadByte();
				case TypeCode.SByte:
					return reader.ReadSByte();
				case TypeCode.Int16:
					return reader.ReadInt16();
				case TypeCode.Int32:
					return reader.ReadInt32();
				case TypeCode.UInt16:
					return reader.ReadUInt16();
				case TypeCode.UInt32:
					return reader.ReadUInt32();
				case TypeCode.Single:
					return reader.ReadSingle();
				case TypeCode.Double:
					return reader.ReadDouble();
				case TypeCode.String:
					return reader.ReadString();
				default:
					throw new InvalidOperationException("BinaryReaderExtender.Read does not support TypeCode " + tc.ToString());
			}
		}

		public static T Read<T>(this BinaryReader reader) where T : IConvertible
		{
			return (T)Read(reader, Type.GetTypeCode(typeof(T)));
		}

		public static T[] ReadArray<T>(this BinaryReader reader) where T : IConvertible
		{
			int elemSize = GetByteSize(Type.GetTypeCode(typeof(T)));
			int remainingBytes = (int)(reader.BaseStream.Length - reader.BaseStream.Position);

			int totElems = remainingBytes / elemSize;
			T[] ret = new T[totElems];
			try
			{
				int pos = 0;
				while (true)
				{
					ret[pos++] = reader.Read<T>();
				}
			}
			catch (EndOfStreamException)
			{
				return ret;
			}
		}
	}
}
