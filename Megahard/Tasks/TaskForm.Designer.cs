namespace Megahard.Tasks
{
	partial class TaskForm
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

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskForm));
			this.toolStripContainer_ = new System.Windows.Forms.ToolStripContainer();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.statusLabel_ = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.mainToolstrip = new System.Windows.Forms.ToolStrip();
			this.closeActiveTaskButton_ = new System.Windows.Forms.ToolStripButton();
			this.toolStripContainer_.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer_.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer_.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.mainToolstrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer_
			// 
			// 
			// toolStripContainer_.BottomToolStripPanel
			// 
			this.toolStripContainer_.BottomToolStripPanel.Controls.Add(this.statusStrip1);
			// 
			// toolStripContainer_.ContentPanel
			// 
			this.toolStripContainer_.ContentPanel.Size = new System.Drawing.Size(774, 495);
			this.toolStripContainer_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer_.LeftToolStripPanelVisible = false;
			this.toolStripContainer_.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer_.Name = "toolStripContainer_";
			this.toolStripContainer_.RightToolStripPanelVisible = false;
			this.toolStripContainer_.Size = new System.Drawing.Size(774, 566);
			this.toolStripContainer_.TabIndex = 1;
			this.toolStripContainer_.Text = "toolStripContainer1";
			// 
			// toolStripContainer_.TopToolStripPanel
			// 
			this.toolStripContainer_.TopToolStripPanel.Controls.Add(this.mainMenu);
			this.toolStripContainer_.TopToolStripPanel.Controls.Add(this.mainToolstrip);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statusLabel_});
			this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.statusStrip1.Location = new System.Drawing.Point(0, 0);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(774, 22);
			this.statusStrip1.TabIndex = 0;
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			this.toolStripProgressBar1.Visible = false;
			// 
			// statusLabel_
			// 
			this.statusLabel_.Name = "statusLabel_";
			this.statusLabel_.Size = new System.Drawing.Size(0, 17);
			// 
			// mainMenu
			// 
			this.mainMenu.Dock = System.Windows.Forms.DockStyle.None;
			this.mainMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(774, 24);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "menuStrip1";
			// 
			// mainToolstrip
			// 
			this.mainToolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.mainToolstrip.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.mainToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeActiveTaskButton_});
			this.mainToolstrip.Location = new System.Drawing.Point(3, 24);
			this.mainToolstrip.Name = "mainToolstrip";
			this.mainToolstrip.Size = new System.Drawing.Size(66, 25);
			this.mainToolstrip.TabIndex = 1;
			this.mainToolstrip.Visible = false;
			// 
			// closeActiveTaskButton_
			// 
			this.closeActiveTaskButton_.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.closeActiveTaskButton_.Image = ((System.Drawing.Image)(resources.GetObject("closeActiveTaskButton_.Image")));
			this.closeActiveTaskButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.closeActiveTaskButton_.Name = "closeActiveTaskButton_";
			this.closeActiveTaskButton_.Size = new System.Drawing.Size(23, 22);
			this.closeActiveTaskButton_.Text = "Back";
			this.closeActiveTaskButton_.ToolTipText = "Close this task and return to previous task";
			this.closeActiveTaskButton_.Visible = false;
			this.closeActiveTaskButton_.Click += new System.EventHandler(this.closeActiveTaskButton__Click);
			// 
			// TaskForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(774, 566);
			this.Controls.Add(this.toolStripContainer_);
			this.Name = "TaskForm";
			this.Text = "TaskForm";
			this.toolStripContainer_.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer_.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer_.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer_.TopToolStripPanel.PerformLayout();
			this.toolStripContainer_.ResumeLayout(false);
			this.toolStripContainer_.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.mainToolstrip.ResumeLayout(false);
			this.mainToolstrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStripContainer_;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel_;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStrip mainToolstrip;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripButton closeActiveTaskButton_;
	}
}