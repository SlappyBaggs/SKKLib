using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	partial class CollectionVisualizer
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
			this.headerBottom_ = new KryptonHeader();
			this.addButtonSpec_ = new ButtonSpecAny();
			this.addCmd_ = new KryptonCommand();
			this.buttonSpecAny2 = new ButtonSpecAny();
			this.delCmd_ = new KryptonCommand();
			this.reorderBar_ = new KryptonHeader();
			this.buttonSpecAny3 = new ButtonSpecAny();
			this.moveDownCmd_ = new KryptonCommand();
			this.buttonSpecAny4 = new ButtonSpecAny();
			this.moveUpCmd_ = new KryptonCommand();
			this.listBox_ = new KryptonListBox();
			this.addContextMenu_ = new KryptonContextMenu();
			this.addMenuItems_ = new KryptonContextMenuItems();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// headerBottom_
			// 
			this.headerBottom_.ButtonSpecs.AddRange(new ButtonSpecAny[] {
            this.addButtonSpec_,
            this.buttonSpecAny2});
			this.headerBottom_.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.headerBottom_.Location = new System.Drawing.Point(0, 267);
			this.headerBottom_.Name = "headerBottom_";
			this.headerBottom_.Size = new System.Drawing.Size(260, 28);
			this.headerBottom_.TabIndex = 0;
			this.headerBottom_.Values.Description = "";
			this.headerBottom_.Values.Heading = "";
			this.headerBottom_.Values.Image = null;
			// 
			// addButtonSpec_
			// 
			this.addButtonSpec_.Enabled = ButtonEnabled.True;
			this.addButtonSpec_.KryptonCommand = this.addCmd_;
			this.addButtonSpec_.UniqueName = "EC730CAA7DB44193EC730CAA7DB44193";
			// 
			// addCmd_
			// 
			this.addCmd_.Text = "Add";
			this.addCmd_.Execute += new System.EventHandler(this.addCmd__Execute);
			// 
			// buttonSpecAny2
			// 
			this.buttonSpecAny2.Enabled = ButtonEnabled.True;
			this.buttonSpecAny2.KryptonCommand = this.delCmd_;
			this.buttonSpecAny2.UniqueName = "3E7E45D3E6F543073E7E45D3E6F54307";
			// 
			// delCmd_
			// 
			this.delCmd_.Text = "Delete";
			this.delCmd_.Execute += new System.EventHandler(this.delCmd__Execute);
			// 
			// reorderBar_
			// 
			this.reorderBar_.ButtonSpecs.AddRange(new ButtonSpecAny[] {
            this.buttonSpecAny3,
            this.buttonSpecAny4});
			this.reorderBar_.Dock = System.Windows.Forms.DockStyle.Left;
			this.reorderBar_.Location = new System.Drawing.Point(0, 0);
			this.reorderBar_.Name = "reorderBar_";
			this.reorderBar_.Orientation = VisualOrientation.Left;
			this.reorderBar_.Size = new System.Drawing.Size(26, 267);
			this.reorderBar_.TabIndex = 1;
			this.reorderBar_.Values.Description = "";
			this.reorderBar_.Values.Heading = "";
			this.reorderBar_.Values.Image = null;
			// 
			// buttonSpecAny3
			// 
			this.buttonSpecAny3.Edge = PaletteRelativeEdgeAlign.Near;
			this.buttonSpecAny3.Enabled = ButtonEnabled.True;
			this.buttonSpecAny3.KryptonCommand = this.moveDownCmd_;
			this.buttonSpecAny3.UniqueName = "8E16DAE001A548D68E16DAE001A548D6";
			// 
			// moveDownCmd_
			// 
			//this.moveDownCmd_.ImageSmall = global::Megahard.Properties.Resources.arrow_left;
			this.moveDownCmd_.Execute += new System.EventHandler(this.moveDownCmd__Execute);
			// 
			// buttonSpecAny4
			// 
			this.buttonSpecAny4.Edge = PaletteRelativeEdgeAlign.Far;
			this.buttonSpecAny4.Enabled = ButtonEnabled.True;
			this.buttonSpecAny4.KryptonCommand = this.moveUpCmd_;
			this.buttonSpecAny4.UniqueName = "4C5866AB47FB45BC4C5866AB47FB45BC";
			// 
			// moveUpCmd_
			// 
			//this.moveUpCmd_.ImageSmall = global::Megahard.Properties.Resources.arrow_right;
			this.moveUpCmd_.Execute += new System.EventHandler(this.moveUpCmd__Execute);
			// 
			// listBox_
			// 
			this.listBox_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox_.Location = new System.Drawing.Point(26, 0);
			this.listBox_.Name = "listBox_";
			this.listBox_.Size = new System.Drawing.Size(234, 267);
			this.listBox_.TabIndex = 2;
			this.listBox_.SelectedIndexChanged += new System.EventHandler(this.listBox__SelectedIndexChanged);
			// 
			// addContextMenu_
			// 
			this.addContextMenu_.Items.AddRange(new KryptonContextMenuItemBase[] {
            this.addMenuItems_});
			// 
			// addMenuItems_
			// 
			this.addMenuItems_.ImageColumn = false;
			// 
			// CollectionVisualizer
			// 
			this.Controls.Add(this.listBox_);
			this.Controls.Add(this.reorderBar_);
			this.Controls.Add(this.headerBottom_);
			this.Name = "CollectionVisualizer";
			this.Size = new System.Drawing.Size(260, 295);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private KryptonHeader headerBottom_;
		private KryptonHeader reorderBar_;
		private KryptonListBox listBox_;
		private KryptonCommand addCmd_;
		private KryptonCommand delCmd_;
		private KryptonCommand moveDownCmd_;
		private KryptonCommand moveUpCmd_;
		private ButtonSpecAny addButtonSpec_;
		private ButtonSpecAny buttonSpecAny2;
		private ButtonSpecAny buttonSpecAny3;
		private ButtonSpecAny buttonSpecAny4;
		private KryptonContextMenu addContextMenu_;
		private KryptonContextMenuItems addMenuItems_;
	}
}
