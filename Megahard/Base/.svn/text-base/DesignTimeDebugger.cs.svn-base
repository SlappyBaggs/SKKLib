using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Megahard.Debug
{
	[Designer(typeof(DesignTimeDebuggerDesigner), typeof(System.ComponentModel.Design.IDesigner))]
	public partial class DesignTimeDebugger : ComponentModel.ComponentBase
	{
		public DesignTimeDebugger()
		{
			InitializeComponent();
			listener_ = new DesignTraceListener(this);
			SetupListener();
			Disposed += (s, e) => DesignTrace.UnRegisterListener(listener_);
		}

		public DesignTimeDebugger(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
			listener_ = new DesignTraceListener(this);
			SetupListener();
			Disposed += (s, e) => DesignTrace.UnRegisterListener(listener_);
		}

		public Form CreateDesignTimeViewingForm()
		{
			return new DesignTimeDebuggerDisplayUI(this);
		}

		void SetupListener()
		{
			DesignTrace.RegisterListener(listener_);
		}
		private readonly DesignTraceListener listener_;

		[Browsable(false)]
		public IList<string> DesignTraceMessages
		{
			get
			{
				return msgs_;
			}
		}

		[Browsable(false)]
		public IList<DataTraceEntry> DesignDataTrace
		{
			get { return dataTrace_; }
		}

		readonly BindingList<string> msgs_ = new BindingList<string>();

		public class DataTraceEntry
		{
			[TypeConverter(typeof(ExpandableObjectConverter))]
			public TraceEventCache TraceEventCache { get; set; }
			public string Source { get; set; }
			public object[] Data { get; set; }
			
			[Browsable(false)]
			public string ShortDescription
			{
				get
                {
					if (Data.Length > 0)
					{
						return Data[0].ToString();
					}
					else
					{
						return TraceEventCache.DateTime.ToString();
					}
                }
			}
		}
		readonly BindingList<DataTraceEntry> dataTrace_ = new BindingList<DataTraceEntry>();
		void Write(string msg)
		{
			msgs_.Add(msg);
		}

		void WriteLine(string msg)
		{
			msgs_.Add(msg);
		}

		void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			dataTrace_.Add(new DataTraceEntry() { TraceEventCache = eventCache, Source = source, Data = data });
		}

		class DesignTraceListener : TraceListener
		{
			private readonly DesignTimeDebugger dtd_;
			public DesignTraceListener(DesignTimeDebugger dtd)
			{
				dtd_ = dtd;
			}
			public override void Write(string message)
			{
				dtd_.Write(message);
			}

			public override void WriteLine(string message)
			{
				dtd_.WriteLine(message);
			}

			public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
			{

				dtd_.TraceData(eventCache, source, eventType, id, data);
			}
		}
	}

	class DesignTimeDebuggerDesigner : Design.ComponentDesigner
	{
		private Form form_;

		[Design.ShowInSmartPanel(AutoFireComponentChange = false)]
		void ShowTraceMessages()
		{
			if(form_ == null || form_.IsDisposed)
				form_ = new DesignTimeDebuggerDisplayUI(base.Component as DesignTimeDebugger);
			form_.Show();
		}

		public override void DoDefaultAction()
		{
			ShowTraceMessages();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && form_ != null && !form_.IsDisposed)
			{
				form_.Dispose();
				form_ = null;
			}

			base.Dispose(disposing);
		}
	}

}