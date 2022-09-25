using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Megahard.Collections
{
	public interface IReadIndexable<ValType, KeyType> : System.Collections.Generic.IEnumerable<ValType>
	{
		ValType this[KeyType key] { get; }
		KeyType IndexOf(ValType value);
		int Length { get; }
	}

	public interface IIndexable<ValType, KeyType> : IReadIndexable<ValType, KeyType>
	{
		new ValType this[KeyType key] { get; set; }
	}

	public static class ReadIndexableAdapter
	{
		sealed class Adapter<T> : ReadOnlyCollection<T>
		{
			public Adapter(IList<T> list) : base(list) { }
		}

		public static IReadIndexable<T, int> AsReadIndexable<T>(this IList<T> list)
		{
			return new Adapter<T>(list);
		}
	}
}