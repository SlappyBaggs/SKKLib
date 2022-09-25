using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Controls
{
	partial class CollectionBrowser
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainer_ = new KryptonSplitContainer();
			this.catList_ = new KryptonListBox();
			this.catContext_ = new KryptonContextMenu();
			this.contextMenuItems_ = new KryptonContextMenuItems();
			this.contextMenuItem_ = new KryptonContextMenuItem();
			this.contextItemSelectNoCats_ = new KryptonContextMenuItem();
			this.header_ = new KryptonHeader();
			this.memberList_ = new KryptonListBox();
			this.memberContext_ = new KryptonContextMenu();
			this.contextMenuItems1_ = new KryptonContextMenuItems();
			this.contextMenuItem1_ = new KryptonContextMenuItem();
			this.contextItemSelectNoObs_ = new KryptonContextMenuItem();
			this.header1_ = new KryptonHeader();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_.Panel1)).BeginInit();
			this.splitContainer_.Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_.Panel2)).BeginInit();
			this.splitContainer_.Panel2.SuspendLayout();
			this.splitContainer_.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer_
			// 
			this.splitContainer_.Cursor = System.Windows.Forms.Cursors.Default;
			this.splitContainer_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer_.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer_.Location = new System.Drawing.Point(0, 0);
			this.splitContainer_.Name = "splitContainer_";
			// 
			// splitContainer_.Panel1
			// 
			this.splitContainer_.Panel1.Controls.Add(this.catList_);
			this.splitContainer_.Panel1.Controls.Add(this.header_);
			// 
			// splitContainer_.Panel2
			// 
			this.splitContainer_.Panel2.Controls.Add(this.memberList_);
			this.splitContainer_.Panel2.Controls.Add(this.header1_);
			this.splitContainer_.Size = new System.Drawing.Size(466, 351);
			this.splitContainer_.SplitterDistance = 155;
			this.splitContainer_.TabIndex = 0;
			this.splitContainer_.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer__SplitterMoved);
			// 
			// catList_
			// 
			this.catList_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.catList_.KryptonContextMenu = this.catContext_;
			this.catList_.Location = new System.Drawing.Point(0, 31);
			this.catList_.Name = "catList_";
			this.catList_.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.catList_.Size = new System.Drawing.Size(155, 320);
			this.catList_.TabIndex = 1;
			this.catList_.SelectedValueChanged += new System.EventHandler(this.catList__SelectedValueChanged);
			// 
			// catContext_
			// 
			this.catContext_.Items.AddRange(new KryptonContextMenuItemBase[] {
            this.contextMenuItems_});
			// 
			// contextMenuItems_
			// 
			this.contextMenuItems_.ImageColumn = false;
			this.contextMenuItems_.Items.AddRange(new KryptonContextMenuItemBase[] {
            this.contextMenuItem_,
            this.contextItemSelectNoCats_});
			// 
			// contextMenuItem_
			// 
			this.contextMenuItem_.Text = "Select All";
			this.contextMenuItem_.Click += new System.EventHandler(this.contextMenuItem__Click);
			// 
			// contextItemSelectNoCats_
			// 
			this.contextItemSelectNoCats_.Text = "Select None";
			this.contextItemSelectNoCats_.Click += new System.EventHandler(this.contextItemSelectNoCats__Click);
			// 
			// header_
			// 
			this.header_.Dock = System.Windows.Forms.DockStyle.Top;
			this.header_.Location = new System.Drawing.Point(0, 0);
			this.header_.Name = "header_";
			this.header_.Size = new System.Drawing.Size(155, 31);
			this.header_.TabIndex = 0;
			this.header_.Values.Description = "";
			this.header_.Values.Heading = "Categories";
			this.header_.Values.Image = null;
			// 
			// memberList_
			// 
			this.memberList_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memberList_.KryptonContextMenu = this.memberContext_;
			this.memberList_.Location = new System.Drawing.Point(0, 31);
			this.memberList_.Name = "memberList_";
			this.memberList_.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.memberList_.Size = new System.Drawing.Size(306, 320);
			this.memberList_.TabIndex = 1;
			this.memberList_.SelectedValueChanged += new System.EventHandler(this.memberList__SelectedValueChanged);
			// 
			// memberContext_
			// 
			this.memberContext_.Items.AddRange(new KryptonContextMenuItemBase[] {
            this.contextMenuItems1_});
			// 
			// contextMenuItems1_
			// 
			this.contextMenuItems1_.ImageColumn = false;
			this.contextMenuItems1_.Items.AddRange(new KryptonContextMenuItemBase[] {
            this.contextMenuItem1_,
            this.contextItemSelectNoObs_});
			// 
			// contextMenuItem1_
			// 
			this.contextMenuItem1_.Text = "Select All";
			this.contextMenuItem1_.Click += new System.EventHandler(this.contextMenuItem1__Click);
			// 
			// contextItemSelectNoObs_
			// 
			this.contextItemSelectNoObs_.Text = "Select None";
			this.contextItemSelectNoObs_.Click += new System.EventHandler(this.contextItemSelectNoObs__Click);
			// 
			// header1_
			// 
			this.header1_.Dock = System.Windows.Forms.DockStyle.Top;
			this.header1_.Location = new System.Drawing.Point(0, 0);
			this.header1_.Name = "header1_";
			this.header1_.Size = new System.Drawing.Size(306, 31);
			this.header1_.TabIndex = 0;
			this.header1_.Values.Description = "";
			this.header1_.Values.Heading = "Members";
			this.header1_.Values.Image = null;
			// 
			// CollectionBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer_);
			this.Name = "CollectionBrowser";
			this.Size = new System.Drawing.Size(466, 351);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_.Panel1)).EndInit();
			this.splitContainer_.Panel1.ResumeLayout(false);
			this.splitContainer_.Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_.Panel2)).EndInit();
			this.splitContainer_.Panel2.ResumeLayout(false);
			this.splitContainer_.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer_)).EndInit();
			this.splitContainer_.ResumeLayout(false);
			((System.Configuration.IPersistComponentSettings)(this)).LoadComponentSettings();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private KryptonSplitContainer splitContainer_;
		private KryptonListBox catList_;
		private KryptonHeader header_;
		private KryptonListBox memberList_;
		private KryptonHeader header1_;
		private KryptonContextMenu catContext_;
		private KryptonContextMenuItems contextMenuItems_;
		private KryptonContextMenuItem contextMenuItem_;
		private KryptonContextMenu memberContext_;
		private KryptonContextMenuItems contextMenuItems1_;
		private KryptonContextMenuItem contextMenuItem1_;
		private KryptonContextMenuItem contextItemSelectNoCats_;
		private KryptonContextMenuItem contextItemSelectNoObs_;
	}
}
