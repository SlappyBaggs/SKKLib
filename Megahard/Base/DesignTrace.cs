using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Megahard.Debug
{
	public static class DesignTrace
	{
		static DesignTrace()
		{
			Enabled = false;
		}

		static bool Enabled
		{
			get;
			set;
		}
		internal static void RegisterListener(TraceListener tl)
		{
			if (!Enabled) return;

			if (!traceSrc_.Listeners.Contains(tl))
				traceSrc_.Listeners.Add(tl);
		}

		internal static void UnRegisterListener(TraceListener tl)
		{
			if (!Enabled) return;
			traceSrc_.Listeners.Remove(tl);
		}

		public static void Info(object o)
		{
			Info(SmartConvert.ConvertTo<string>(o));
		}

		public static void Info(string msg)
		{
			if (!Enabled) return;
			traceSrc_.TraceInformation(msg);
		}

		public static void Data(params object[] argsobject)
		{
			if (!Enabled) return;
			traceSrc_.TraceData(TraceEventType.Information, 0, argsobject);
		}


		private static readonly TraceSource traceSrc_ = new TraceSource("DesignTrace", SourceLevels.All);
	}
}
