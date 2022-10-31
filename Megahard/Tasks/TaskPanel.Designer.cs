namespace Megahard.Tasks
{
	partial class TaskPanel
	{
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		private void InitializeComponent()
		{
			this.taskStack_ = new Megahard.Tasks.TaskStack();
			((System.ComponentModel.ISupportInitialize)(this.taskStack_)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// TaskPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "TaskPanel";
			((System.ComponentModel.ISupportInitialize)(this.taskStack_)).EndInit();
			((System.Configuration.IPersistComponentSettings)(this)).LoadComponentSettings();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TaskStack taskStack_;
	}
}
