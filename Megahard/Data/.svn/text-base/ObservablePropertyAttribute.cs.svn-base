using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.CodeDom;
using System.ComponentModel.Design;
using Megahard.CodeDom;
using System.Reflection;

namespace Megahard.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ObservablePropertyAttribute : Attribute
	{
		public ObservablePropertyAttribute(bool autoImplement)
		{
			AutoImplement = autoImplement;
		}
		public ObservablePropertyAttribute()
		{
			AutoImplement = false;
		}
		public bool AutoImplement { get; private set; }
	}
}
