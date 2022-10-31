using System;
using System.Windows.Forms;

namespace Megahard.Tasks
{
	public partial class Task : Data.ObservableComponent
	{
		public Task()
		{
			State = TaskState.Incomplete;
		}

		protected void ActivateChildTask()
		{
		}

		protected virtual Control GetTaskUI()
		{
			return null;
		}

		public void ActivateGUI(Action<Control> addCtl)
		{
			if (addCtl == null)
				throw new ArgumentNullException("addCtl");

			var gui = GetTaskUI();
			if (gui == null)
				throw new InvalidOperationException("Task cannot be activated, null gui");
			if (Activated)
				throw new InvalidOperationException("Task is already activated");
			Activated = true;
			addCtl(gui);
		}

		public void DeactivateGUI()
		{
			if (!Activated)
				return;
			var gui = GetTaskUI();
			if (gui != null)
				gui.Parent = null;
		}


		//<ObservableProperty Name="Activated" Type="bool" SetAccessor="private" DefaultValue="false"/>
		//<ObservableProperty Name="TaskName" Type="string" SetAccessor="protected" DefaultValue='""'/>

		protected virtual void OnCompleted()
		{
		}

		protected virtual void OnActivated()
		{
		}

		protected virtual void OnDeactivated()
		{
		}

		protected virtual void OnCanceled()
		{
		}
		protected void IndicateCompleted()
		{
			State = TaskState.Complete;
			OnCompleted();
		}

		public void CancelTask()
		{
			State = TaskState.Canceled;
			OnCanceled();
		}

		//<ObservableProperty Name="State" Type="TaskState" SetAccessor="private" DefaultValue="TaskState.Incomplete"/>

		partial void AfterStateChanged(Data.ObjectChangedEventArgs<TaskState> chgArgs)
		{
			if (chgArgs.NewValue == TaskState.Canceled)
				OnCanceled();
			else if (chgArgs.NewValue == TaskState.Complete)
				OnCompleted();
		}

		partial void AfterActivatedChanged(Data.ObjectChangedEventArgs<bool> chgArgs)
		{
			if (chgArgs.NewValue)
				OnActivated();
			else
				OnDeactivated();
		}
	}

	public enum TaskState
	{
		Incomplete, Complete, Canceled,
	}
}