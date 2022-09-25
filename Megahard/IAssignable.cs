using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megahard
{
	public interface IAssignable
	{
		void Assign(object val);
	}

	public interface IValue
	{
		object Value
		{
			get;
		}
	}

	public interface IValue<T>
	{
		T Value { get; }
	}

	public abstract class AssignableValue : IValue, IAssignable
	{
		public abstract object Value { get; set; }
		void IAssignable.Assign(object val)
		{
			Value = val;
		}
	}
}
