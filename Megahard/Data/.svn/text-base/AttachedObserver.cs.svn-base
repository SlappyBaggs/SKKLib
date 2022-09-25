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
	public class AttachedObserver
	{
		internal AttachedObserver(Action detatch)
		{
			_detatch = detatch;
		}
		readonly Action _detatch;
		public void Detatch()
		{
			_detatch();
		}
	}
}
