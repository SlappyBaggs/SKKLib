namespace Megahard.Controls
{
	partial class ToggleSwitch
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
			this.SuspendLayout();
			// 
			// ToggleSwitch
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Name = "ToggleSwitch";
			this.Size = new System.Drawing.Size(32, 50);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ToggleSwitch_Paint);
			this.Click += new System.EventHandler(this.ToggleSwitch_Click);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
