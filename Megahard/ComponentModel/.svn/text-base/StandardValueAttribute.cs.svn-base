using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Megahard.ComponentModel
{
	/// <summary>
	/// For usage with the StandardValuesConverter.  Apply this attribute to static properties that
	/// you wish to be included as standard values
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class StandardValueAttribute : Attribute
	{
		/// <summary>
		/// Default IsStandardValue = true
		/// </summary>
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