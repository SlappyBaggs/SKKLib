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
	public static class StandardValues
	{
		static readonly object s_UnavailableValue = new object();
		public static object UnavailableValue
		{
			get
			{
				return s_UnavailableValue;
			}
		}
	}
}
