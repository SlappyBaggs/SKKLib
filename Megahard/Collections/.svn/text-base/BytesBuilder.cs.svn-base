using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
namespace Megahard.Collections.Specialized
{
    public partial class Bytes
    {
        public new class Builder : IReadIndexable<byte, int>
        {
			BinaryWriter writer_;
			BuilderStream stream_;
			ImmutableArray<byte>.Builder builder_;
            internal Builder(int initialCapacity)
            {
				builder_ = ImmutableArray<byte>.Build(initialCapacity);
				stream_ = new BuilderStream(builder_);
				writer_ = new BinaryWriter(stream_);
            }

            public Builder Add<T>(T val) where T : IValue<byte>
            {
                Add(val.Value);
				return this;
            }

			public Builder Add(byte[] bytes)
			{
				writer_.Write(bytes);
				return this;
			}
			public Builder Add(string s)
			{
				return Add(System.Text.Encoding.UTF8.GetBytes(s));
			}

			public Builder Add(byte b)
			{
				writer_.Write(b);
				return this;
			}
			public Builder Add(sbyte b)
			{
				writer_.Write(b);
				return this;
			}

            public Builder Add(Bytes b)
            {
				writer_.Write(b.byteArray_);
				return this;
            }

            public Builder Add(short s)
            {
				writer_.Write(s);
	            return this;
			}

            public Builder Add(UInt16 s)
            {
				writer_.Write(s);
				return this;
			}

            public Builder Add(float f)
            {
				writer_.Write(f);
				return this;
			}

            public Builder Add(double d)
            {
				writer_.Write(d);
				return this;
			}

            public Builder Add(int i)
            {
				writer_.Write(i);
				return this;
			}

            public Builder Add(uint i)
            {
				writer_.Write(i);
				return this;
			}

            public Builder Add(long l)
            {
				writer_.Write(l);
				return this;
			}

            public Builder Add(ulong l)
            {
				writer_.Write(l);
				return this;
			}


            public Builder Add(IEnumerable<byte> blist)
            {
				writer_.Write(blist.ToArray());
				return this;
			}

            public Bytes ToBytes()
            {
				return new Bytes(builder_.ToArray());
            }

            public byte this[int i]
            {
				get 
				{
					return builder_[i];
				}
            }

            public int Count
            {
				get { return builder_.Count; }
            }

			class BuilderStream : Stream
			{
				readonly ImmutableArray<byte>.Builder builder_;
				public BuilderStream(ImmutableArray<byte>.Builder builder)
				{
					builder_ = builder;
				}

				public override bool CanRead
				{
					get { return false; }
				}

				public override bool CanSeek
				{
					get { return false; }
				}

				public override bool CanWrite
				{
					get { return true; }
				}

				public override void Flush()
				{
					
				}

				public override long Length
				{
					get { return builder_.Count; }
				}

				public override long Position
				{
					get
					{
						return builder_.Count;
					}
					set
					{
						throw new NotSupportedException();
					}
				}

				public override int Read(byte[] buffer, int offset, int count)
				{
					throw new NotSupportedException();
				}

				public override long Seek(long offset, SeekOrigin origin)
				{
					throw new NotSupportedException();
				}

				public override void SetLength(long value)
				{
					throw new NotSupportedException();
				}

				public override void Write(byte[] buffer, int offset, int count)
				{
					builder_.AddRange(buffer, offset, count);
				}
				public override void WriteByte(byte value)
				{
					builder_.Add(value);
				}
			}


			#region IReadIndexable<byte,int> Members


			public int IndexOf(byte value)
			{
				return builder_.IndexOf(value);
			}

			int IReadIndexable<byte, int>.Length
			{
				get { return builder_.Count; }
			}

			#endregion

			#region IEnumerable<byte> Members

			public IEnumerator<byte> GetEnumerator()
			{
				return builder_.GetEnumerator();
			}

			#endregion

			#region IEnumerable Members

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			#endregion
		}

    }
}
