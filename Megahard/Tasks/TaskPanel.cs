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
	public partial class TaskPanel : Controls.UserControlBase
	{
		public TaskPanel()
		{
			InitializeComponent();
			taskStack_.AttachObserver<Task>("ActiveTask", ActiveTaskChanged);
		}

		void ActiveTaskChanged(Data.ObjectChangedEventArgs<Task> chgArgs)
		{
			if(chgArgs.OldValue != null)
				chgArgs.OldValue.DeactivateGUI();
			if(chgArgs.NewValue != null)
				chgArgs.NewValue.ActivateGUI(ctl => Controls.Add(ctl));
		}
		
		public void SetTask(Task task)
		{
			if (taskStack_.ActiveTask != null)
				throw new InvalidOperationException("A task is already set to the task panel");
			taskStack_.Push(task);
		}

	}
}
