using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Megahard.ExtensionMethods;
using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Tasks
{
	public partial class TaskForm : KryptonForm, ITaskContainer
	{

		private UITask activeTask;
	
		public TaskForm()
		{
			InitializeComponent();

		}

		private ToolStripContentPanel TaskPanel
		{
			get
			{
			   return toolStripContainer_.ContentPanel;
			}
		}

		[Browsable(false)]
		protected StatusStrip StatusStrip
		{
			get
			{
				return this.statusStrip1;
			}
		}

		protected string TitlePrefix { get; set; }
		protected string TitleSuffix { get; set; }
		string origTitle_;
 
		private Stack<UITask> taskStack_ = new Stack<UITask>();
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UITask ActiveTask
		{
			get
			{
				return activeTask;
			}
			set
			{
				if (activeTask == value)
				{
					return;
				}

				if (activeTask != null)
				{
					TaskPanel.Controls.Remove(activeTask);
					TaskDeactivated(activeTask, value != null);
				}

				if (value != null)
				{
					value.SetTaskContainer(this);
					TaskPanel.Controls.Add(value);
					taskStack_.Push(value);
					TaskActivated(value);
				}
				else
				{
					taskStack_.Pop();
					if (taskStack_.Count > 0)
					{
						ActiveTask = taskStack_.Pop();
					}
				}

				if (activeTask == null)
					Text = origTitle_;
			}
		}

		public void ReplaceActiveTask(UITask newTask)
		{
			if (newTask == null)
			{
				ActiveTask = null;
				return;
			}
			if (activeTask == newTask)
			{
				return;
			}

			if (activeTask != null)
			{
				TaskPanel.Controls.Remove(activeTask);
				TaskDeactivated(activeTask, false);
				taskStack_.Pop();
			}

			newTask.SetTaskContainer(this);
			TaskPanel.Controls.Add(newTask);
			taskStack_.Push(newTask);
			TaskActivated(newTask);
		}


		private void TaskActivated(UITask task)
		{
			activeTask = task;
			task.Dock = toolStripContainer_.Dock;
			if (task.Menu != null)
			{
				MergeMenus(task.Menu, mainMenu);
			}
			if (task.Tools != null)
			{
				MergeToolStrips(task.Tools, mainToolstrip);
			}
			task.ObjectChanged += task_ObjectChanged;
			task.Focus();
			task.SetActivated(true);
			if (origTitle_ == null)
				origTitle_ = Text;
			UpdateTitle(task.TaskName);
			closeActiveTaskButton_.Visible = task.ShowCloseTaskButton;
			mainToolstrip.Visible = mainToolstrip.Items.Count > 1 || closeActiveTaskButton_.Visible;
			mainMenu.Visible = mainMenu.Items.Count != 0;
		}

		private void UpdateTitle(string s)
		{
			Text = TitlePrefix + " " + s + " " + TitleSuffix;
		}

		void task_ObjectChanged(object sender, Data.ObjectChangedEventArgs e)
		{
			if (sender == ActiveTask && e.PropertyName == "TaskName")
			{
				UpdateTitle(((UITask)sender).TaskName);
			}

		}

		private void TaskDeactivated(UITask task, bool stillinStack)
		{
			activeTask = null;
			if (task.Menu != null && (!stillinStack || !task.MenuSticky))
			{
				ToolStripManager.RevertMerge(mainMenu, task.Menu);
			}
			if (task.Tools != null && (!stillinStack || !task.ToolsSticky))
			{
				ToolStripManager.RevertMerge(mainToolstrip, task.Tools);
			}
			mainToolstrip.Visible = mainToolstrip.Items.Count != 0;

			if (!stillinStack)
				task.SetTaskContainer(null);
			task.SetActivated(false);
			task.ObjectChanged -= task_ObjectChanged;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string StatusText
		{
			get { return statusLabel_.Text; }
			set 
			{
				this.AutoBeginInvoke(() => 
					{
						statusLabel_.Visible = true;
						statusLabel_.Text = value;
					}
				);
			}
		}

		public void ShowStatus(string txt, TimeSpan displayTime, bool clearProgBar)
		{
			StatusText = txt;
			Threading.DelayedMethodCall.Call(displayTime, ()=>
				{
					this.BeginInvoke((MethodInvoker)
						delegate()
						{
							if (statusLabel_.Text == txt)
							{
								statusLabel_.Text = "";
								if (clearProgBar)
								{
									toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
									toolStripProgressBar1.Visible = false;
								}
							}
						});
				});
		}

		public ProgressReporter StartProgressReportableOperation(int totalSteps, string statusTxt)
		{
			SetProgress(0, totalSteps, statusTxt);
			ProgressReporter pr = new ProgressReporter(
				amt=> this.AutoBeginInvoke(()=>toolStripProgressBar1.Increment(amt)),
				stxt=> this.AutoBeginInvoke(()=>StatusText = stxt),
				(dtxt)=>this.AutoBeginInvoke(()=>
					{
						toolStripProgressBar1.Value = toolStripProgressBar1.Maximum;
						ShowStatus(dtxt, TimeSpan.FromSeconds(5), true);
					}),
				(ctxt)=>this.AutoBeginInvoke(()=>
					{
						toolStripProgressBar1.Value = 0;
						ShowStatus(ctxt, TimeSpan.FromSeconds(5), true);
					})
			);

			return pr;
		}

		public void HideProgress()
		{
			this.AutoBeginInvoke(() =>
				{
					toolStripProgressBar1.Visible = false;
				}
			);
		}

		public void SetProgress(int currentStep, int totSteps)
		{
			this.AutoBeginInvoke(() =>
				{
					toolStripProgressBar1.Visible = true;
					toolStripProgressBar1.Minimum = System.Math.Min(0, currentStep);
					toolStripProgressBar1.Maximum = System.Math.Max(totSteps, currentStep);
					toolStripProgressBar1.Value = currentStep;
				}
			);
		}
		public void SetProgress(int currentStep, int totSteps, string status)
		{
			this.AutoBeginInvoke(() =>
				{
					toolStripProgressBar1.Visible = true;
					toolStripProgressBar1.Minimum = System.Math.Min(1, currentStep);
					toolStripProgressBar1.Maximum = System.Math.Max(totSteps, currentStep);
					toolStripProgressBar1.Value = currentStep;
					StatusText = status;
				}
			);
		}

		// Merge m1 into m2
		internal static void MergeMenus(MenuStrip m1, MenuStrip m2)
		{
			_MergeMenus(m1.Items, m2.Items, true);
			ToolStripManager.Merge(m1, m2);
		}

		internal static void MergeToolStrips(ToolStrip t1, ToolStrip t2)
		{
			_MergeMenus(t1.Items, t2.Items, true);
			ToolStripManager.Merge(t1, t2);
		}

		// Recursive function to auto-set MergeActions and MergeIndices in MenuStrips...
		// At the top level, new menu items are appended AFTER existing items...
		// Starting at sub menus, new menu items are inserted BEFORE existing items...
		// To count as a match, the Text fields of two ToolStripMenuItems are compared...
		// This requires exact match... ex: Exit does NOT match E&xit, so be aware of ALT/underscore notation...
		
		// Menu1 has a 'Test' MenuItem...  Menu2 has a 'Test' MenuItem
		// Menu2 'Test' has an OnCLick event...
		// When Menu2 is merged INTO Menu1, the on-click event for Menu2 'Test' is lost IF Menu1 'Test' has sub-menus...
		private static void _MergeMenus(ToolStripItemCollection c1, ToolStripItemCollection c2, bool first)
		{
			int _count = (first ? c2.Count : 0);
			bool found;
			foreach (Object ob1 in c1)
			{
				if (ob1 is ToolStripMenuItem)
				{
					ToolStripMenuItem mi1 = (ToolStripMenuItem)ob1;
					found = false;
					foreach (Object ob2 in c2)
					{
						if (ob2 is ToolStripMenuItem)
						{
							ToolStripMenuItem mi2 = (ToolStripMenuItem)ob2;
							if (mi2.Text == mi1.Text)
							{
								mi1.MergeAction = (mi2.DropDownItems.Count > 0) ? MergeAction.MatchOnly : MergeAction.Replace;
								found = true;
								_MergeMenus(mi1.DropDownItems, mi2.DropDownItems, false);
								continue;
							}
						}
					}
					if (!found)
					{
						mi1.MergeAction = MergeAction.Insert;
						mi1.MergeIndex = _count++;
					}
				}
			}
		}

		private void closeActiveTaskButton__Click(object sender, EventArgs e)
		{
			ActiveTask = null;
		}
	}

	//[TypeDescriptionProvider(typeof(Megahard.Controls.ForwardToolboxBitmapAttribute<KryptonBreadCrumb, ToolStripNumericUpDown>))]
	public class ToolStripBreadCrumb : Megahard.Controls.ToolStripControlHost<KryptonBreadCrumb>
	{
	}

}
