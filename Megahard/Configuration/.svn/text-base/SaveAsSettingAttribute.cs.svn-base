using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Reflection;

namespace Megahard.Configuration
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	public class SaveAsSettingAttribute : Attribute
	{
		public SaveAsSettingAttribute()
		{
			_save = true;
		}

		public SaveAsSettingAttribute(bool save)
		{
			_save = save;
		}

		public bool Save
		{
			get { return _save; }
		}
		readonly bool _save;
	}
}
