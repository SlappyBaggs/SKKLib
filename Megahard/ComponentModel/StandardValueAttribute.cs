using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.ComponentModel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class StandardValueAttribute : Attribute
	{
		public StandardValueAttribute() : this(true) { }
		public StandardValueAttribute(bool isStandardValue)
		{
			propIsStandardValue_ = isStandardValue;
		}

		#region bool IsStandardValue { get; readonly; }
		readonly bool propIsStandardValue_;
		public bool IsStandardValue
		{
			get { return propIsStandardValue_; }
		}
		#endregion
	}
}