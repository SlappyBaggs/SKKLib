using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Megahard.Tasks
{
	public partial class UITask : Megahard.Controls.UserControlBase
	{
		public UITask()
		{
			InitializeComponent();
			ShowCloseTaskButton = true;
		}
		//<SyncEvent Name="TaskDeactivated" NoArgs="1"/>
		//<SyncEvent Name="TaskActivated" NoArgs="1"/>


		//<ObservableProperty Name="TaskName" Type="string">
		//<Attribute Val='DefaultValue("")'/>
		//</ObservableProperty>

		internal void SetActivated(bool b)
		{
			if (b)
				RaiseTaskActivated(EventArgs.Empty);
			else
				RaiseTaskDeactivated(EventArgs.Empty);
		}
		
		[Browsable(true)]
		[Category("Task")]
		[Description("If true, this tasks menu stays on the main window as long as it is in the task stack")]
		[DefaultValue(false)]
		public bool MenuSticky { get; set; }

		[Browsable(true)]
		[Category("Task")]
		[Description("If true, this tasks menu stays on the main window as long as it is in the task stack")]
		[DefaultValue(false)]
		public bool ToolsSticky { get; set; }

		internal void SetTaskContainer(ITaskContainer cont)
		{
			taskCont_ = cont;
		}

		ITaskContainer taskCont_;
		protected ITaskContainer TaskContainer
		{
			get { return taskCont_; }
		}
		
		protected TaskForm TaskForm
		{
			get
			{
				return (TaskForm)TaskContainer;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Megahard.Tasks.UITask NextTask
		{
			get;
			set;
		}

		protected void GoToNext()
		{
			if (NextTask != null)
				TaskForm.ReplaceActiveTask(NextTask);
		}

		protected void CloseThisTask()
		{
			if (TaskContainer != null && TaskContainer.ActiveTask == this)
				TaskContainer.ActiveTask = null;
		}

		MenuStrip menuStripBASE;
		[Browsable(false)]
		virtual public MenuStrip Menu
		{
			get { return menuStripBASE; }
		}

		ToolStrip toolsBASE;
		[Browsable(true)]
		virtual public ToolStrip Tools
		{
			get { return toolsBASE; }
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			if (!DesignMode)
			{
				if (e.Control is MenuStrip)
				{
					MenuStrip ms = (MenuStrip)e.Control;
					if (menuStripBASE == null)
						menuStripBASE = ms;
					else
						TaskForm.MergeMenus(ms, menuStripBASE);
					ms.Visible = false;
				}
				else if (e.Control is ToolStrip)
				{
					ToolStrip ts = (ToolStrip)e.Control;
					if (toolsBASE == null)
						toolsBASE = ts;
					else
						TaskForm.MergeToolStrips(ts, toolsBASE);
					ts.Visible = false;
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool ShowCloseTaskButton
		{
			get;
			protected set;
		}
	}
}
