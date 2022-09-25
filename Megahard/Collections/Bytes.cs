using System;
using System.Linq;
using System.Text;
using Megahard.ExtensionMethods;
using System.IO;
namespace Megahard.Collections.Specialized
{
	/// <summary>
	/// Bytes class is intended as an immuttable array of bytes.  The array passed in is copied
	/// and it provides no external access to that array, thus it is guaranteed that nobody can alter the array
	/// data once this class is instatiated
	/// </summary>
	public partial class Bytes : Collections.ImmutableArray<byte>, IConvertible
	{
        Bytes()
        {
        }
        public Bytes(System.Collections.Generic.IEnumerable<byte> data)
            : base(data, true)
        {
			byteArray_ = Items as byte[];
			System.Diagnostics.Debug.Assert(byteArray_ != null);
        }

		readonly byte[] byteArray_;

		public BinaryReader CreateReader()
		{
			return new BinaryReader(CreateStream());
		}
		public MemoryStream CreateStream()
		{
			return CreateStream(0, Length);
		}
		public MemoryStream CreateStream(int index, int count)
		{
			return new MemoryStream(byteArray_, index, count, false, false);
		}

		static readonly Bytes s_Empty = new Bytes();
		public static new Bytes Empty { get { return s_Empty; } }

		public static implicit operator Bytes(byte[] array)
		{
            return new Bytes(array);
		}

		public static implicit operator Bytes(System.Collections.ObjectModel.ReadOnlyCollection<byte> bytes)
		{
			return new Bytes(bytes);
		}

		public static implicit operator Bytes(ListSlice<byte> bytes)
		{
			return new Bytes(bytes);
		}

		public static implicit operator Bytes(byte b)
		{
			return Bytes.ToBytes(b);
		}

		public void MarshalToNativeArray(IntPtr nativeArray)
		{
			System.Runtime.InteropServices.Marshal.Copy(byteArray_, 0, nativeArray, Length);
		}

		public static Bytes MarshalFromNativeArray(IntPtr nativeArray, int arrLen)
		{
 			byte[] data = new byte[arrLen];
 			System.Runtime.InteropServices.Marshal.Copy(nativeArray, data, 0, arrLen);
 			return new Bytes(data);
		}

		public new Bytes Slice(int start, int length)
		{
			return new Bytes(base.SliceCore(start, length));
		}

		protected override System.Collections.Generic.IList<byte> SliceCore(int start, int length)
		{
			return this.Slice(start, length);
		}

		public override string ToString()
		{
            if (IsEmpty)
                return string.Empty;
            StringBuilder sb = new StringBuilder(Length * 3);
            byte b = this[0];
            sb.Append(b < 16 ? "0" + b.ToString("X") : b.ToString("X"));

			int MAX = Math.Min(Length, 80);
			for(int i = 1; i < MAX; ++i)
			{
                b = this[i];
                sb.Append(' ').Append(b < 16 ? "0" + b.ToString("X") : b.ToString("X"));
			}
			if (Length > MAX)
				sb.Append("...");
            return sb.ToString();
		}

		public static Bytes ToBytes(TypeCode tc, object o)
		{
			switch (tc)
			{
				case TypeCode.Boolean:
					return BitConverter.GetBytes((bool)o);
				case TypeCode.Byte:
					return new byte[] { (byte)o };
				case TypeCode.SByte:
					return new byte[] { (byte)((sbyte)o) };
				case TypeCode.Int16:
					return BitConverter.GetBytes((Int16)o);
				case TypeCode.Int32:
					return BitConverter.GetBytes((Int32)o);
				case TypeCode.UInt16:
					return BitConverter.GetBytes((UInt16)o);
				case TypeCode.UInt32:
					return BitConverter.GetBytes((UInt32)o);
				case TypeCode.Single:
					return BitConverter.GetBytes((Single)o);
				case TypeCode.Double:
					return BitConverter.GetBytes((Double)o);
				case TypeCode.String:
					return ToBytes(o.ToString());
				default:
					throw new InvalidCastException("Megahard.Bytes cannot convert to typecode " + tc);
			}
		}

		public static Bytes ToBytes(byte b) { return new byte[] { b }; }
		public static Bytes ToBytes(sbyte b) { return new byte[] { (byte)b }; }
		public static Bytes ToBytes(Int16 v) { return BitConverter.GetBytes(v);	}
		public static Bytes ToBytes(Int32 v) { return BitConverter.GetBytes(v); }
		public static Bytes ToBytes(UInt16 v) { return BitConverter.GetBytes(v); }
		public static Bytes ToBytes(UInt32 v) { return BitConverter.GetBytes(v); }
		public static Bytes ToBytes(float v) { return BitConverter.GetBytes(v); }
		public static Bytes ToBytes(double v) { return BitConverter.GetBytes(v); }
		public static Bytes ToBytes(string s) { return System.Text.Encoding.UTF8.GetBytes(s); }

		public static Bytes ToBytes<T>(T val) where T : IConvertible
		{
			TypeCode tc = val.GetTypeCode();
			switch (tc)
			{
				case TypeCode.Boolean:
					return BitConverter.GetBytes(val.ToBoolean(null));
				case TypeCode.Byte:
					return new byte[] { val.ToByte(null) };
				case TypeCode.SByte:
					return new byte[] { (byte)val.ToSByte(null) };
				case TypeCode.Int16:
					return BitConverter.GetBytes(val.ToInt16(null));
				case TypeCode.Int32:
					return BitConverter.GetBytes(val.ToInt32(null));
				case TypeCode.UInt16:
					return BitConverter.GetBytes(val.ToUInt16(null));
				case TypeCode.UInt32:
					return BitConverter.GetBytes(val.ToUInt32(null));
				case TypeCode.Single:
					return BitConverter.GetBytes(val.ToSingle(null));
				case TypeCode.Double:
					return BitConverter.GetBytes(val.ToDouble(null));
				case TypeCode.String:
					return ToBytes(val.ToString());
				default:
					throw new InvalidCastException("Megahard.Bytes cannot convert to typecode " + tc);
			}
		}

		public object ConvertTo(int startIndex, TypeCode tc)
		{
			switch(tc)
			{
				case TypeCode.Boolean:
					return ToBool(startIndex);
				case TypeCode.SByte:
					return (SByte)(this[startIndex]);
				case TypeCode.Byte:
					return this[startIndex];
				case TypeCode.Int16:
					return ToInt16(startIndex);
				case TypeCode.Int32:
					return ToInt32(startIndex);
				case TypeCode.UInt16:
					return ToUInt16(startIndex);
				case TypeCode.UInt32:
					return ToUInt32(startIndex);
				case TypeCode.Single:
					return ToSingle(startIndex);
				case TypeCode.Double:
					return ToDouble(startIndex);
				default:
					throw new InvalidCastException("Megahard.Bytes cannot convert to typecode " + tc);
			}
		}

		public T ConvertTo<T>(int startIndex) where T : IConvertible
		{
			return (T)ConvertTo(startIndex, Type.GetTypeCode(typeof(T)));
		}

		// BitConversion operations
		// These are forwarded off to the BitConverter class
		public int ToInt32(int startIndex)
		{
			return BitConverter.ToInt32(byteArray_, startIndex);
		}
		public short ToInt16(int startIndex)
		{
			return BitConverter.ToInt16(byteArray_, startIndex);
		}

		public bool ToBool(int startIndex)
		{
			return byteArray_[startIndex] != 0;
		}
		public uint ToUInt32(int startIndex)
		{
			return BitConverter.ToUInt32(byteArray_, startIndex);
		}
		public ushort ToUInt16(int startIndex)
		{
			return BitConverter.ToUInt16(byteArray_, startIndex);
		}
		public float ToSingle(int startIndex)
		{
			return BitConverter.ToSingle(byteArray_, startIndex);
		}
		public double ToDouble(int startIndex)
		{
			return BitConverter.ToDouble(byteArray_, startIndex);
		}

		public string ToUTF8String()
		{
			return System.Text.Encoding.UTF8.GetString(byteArray_);
		}
		public string ToUTF8String(int index, int count)
		{
			return System.Text.Encoding.UTF8.GetString(byteArray_, index, count);
		}


		/// <summary>
		/// Utility class for building up an array to then create a Bytes instance
		/// usage of the Builder saves on an extra data copy that would otherwise be performed
		/// </summary>
		/// <returns></returns>
		public new static Builder Build() { return new Builder(0); }
		public new static Builder Build(int capacity) { return new Builder(capacity); }

		public static Bytes Concat(Bytes bytes1, Bytes bytes2)
		{
			if(Bytes.IsNullOrEmpty(bytes1))
				return bytes2 ?? s_Empty;

			if (Bytes.IsNullOrEmpty(bytes2))
				return bytes1 ?? s_Empty;
			// Todo: There is some room to optimize here, but alas for another day
			var bb = Build(bytes1.Length + bytes2.Length);
			bb.Add(bytes1).Add(bytes2);
			return bb.ToBytes();
		}

		#region IConvertible Members

		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return ToBool(0);
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return byteArray_[0];
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Bytes cannot convert to Char");
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Bytes cannot convert to DateTime");
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException("Bytes cannot convert to decimal");
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return ToDouble(0);
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return ToInt16(0);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return ToInt32(0);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return BitConverter.ToInt64(byteArray_, 0);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)byteArray_[0];
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return ToSingle(0);
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return ToString();
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(this, conversionType);
			throw new NotImplementedException();
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return ToUInt16(0);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return ToUInt32(0);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return BitConverter.ToUInt64(byteArray_, 0);
		}

		#endregion
	}

}
