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
	public partial class TaskStack : Data.ObservableComponent
	{
		public TaskStack()
		{
		}

		public TaskStack(IContainer cont)
		{
			cont.Add(this);
		}

		public void Push(Task task)
		{
			if (task == null)
				throw new ArgumentNullException("task");
			if (stack_.Contains(task))
				throw new InvalidOperationException("Task is already in this stack");
			stack_.Push(task);
			ActiveTask = task;
		}

		public void Pop()
		{
			if (stack_.Count == 0)
				return;
			stack_.Pop();
			ActiveTask = stack_.Count == 0 ? null : stack_.Peek();
		}

		//<ObservableProperty Name="ActiveTask" Type="Task" SetAccessor="private"/>

		readonly Stack<Task> stack_ = new Stack<Task>();
	}
}
