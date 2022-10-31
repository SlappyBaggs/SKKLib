using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Megahard
{
	public class WeakReference<T> : WeakReference
	{
		public WeakReference(T target) : base(target)
		{
		}
		public WeakReference()
			: base(null)
		{
		}
		
		public new T Target
		{
			get 
			{
				return (T)base.Target;
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
