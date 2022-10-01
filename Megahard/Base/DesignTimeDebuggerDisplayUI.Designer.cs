﻿using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Debug
{
	partial class DesignTimeDebuggerDisplayUI
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.msgs_ = new System.Windows.Forms.ListBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dataTraceTree_ = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.dataTraceList_ = new System.Windows.Forms.ListBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.kryptonSplitContainer1 = new KryptonSplitContainer();
			this.infoTree_ = new System.Windows.Forms.TreeView();
			this.kryptonSplitContainer2 = new KryptonSplitContainer();
			this.propertyGrid_ = new System.Windows.Forms.PropertyGrid();
			this.detailList_ = new System.Windows.Forms.ListView();
			this.NameColumn = new System.Windows.Forms.ColumnHeader();
			this.ValueColumn = new System.Windows.Forms.ColumnHeader();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusLbl_ = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
			this.kryptonSplitContainer1.Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
			this.kryptonSplitContainer1.Panel2.SuspendLayout();
			this.kryptonSplitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel1)).BeginInit();
			this.kryptonSplitContainer2.Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel2)).BeginInit();
			this.kryptonSplitContainer2.Panel2.SuspendLayout();
			this.kryptonSplitContainer2.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// msgs_
			// 
			this.msgs_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.msgs_.FormattingEnabled = true;
			this.msgs_.Location = new System.Drawing.Point(3, 3);
			this.msgs_.Name = "msgs_";
			this.msgs_.Size = new System.Drawing.Size(623, 381);
			this.msgs_.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(637, 415);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.msgs_);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(629, 389);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Info Trace";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(103, 288);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dataTraceTree_);
			this.tabPage2.Controls.Add(this.splitter1);
			this.tabPage2.Controls.Add(this.dataTraceList_);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(629, 389);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Data Trace";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// dataTraceTree_
			// 
			this.dataTraceTree_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataTraceTree_.Location = new System.Drawing.Point(196, 3);
			this.dataTraceTree_.Name = "dataTraceTree_";
			this.dataTraceTree_.Size = new System.Drawing.Size(430, 383);
			this.dataTraceTree_.TabIndex = 2;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(193, 3);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 383);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// dataTraceList_
			// 
			this.dataTraceList_.Dock = System.Windows.Forms.DockStyle.Left;
			this.dataTraceList_.FormattingEnabled = true;
			this.dataTraceList_.Location = new System.Drawing.Point(3, 3);
			this.dataTraceList_.Name = "dataTraceList_";
			this.dataTraceList_.Size = new System.Drawing.Size(190, 381);
			this.dataTraceList_.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.kryptonSplitContainer1);
			this.tabPage3.Controls.Add(this.splitter2);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(629, 389);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Inspection";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// kryptonSplitContainer1
			// 
			this.kryptonSplitContainer1.ContainerBackStyle = PaletteBackStyle.PanelClient;
			this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
			this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.kryptonSplitContainer1.Location = new System.Drawing.Point(3, 0);
			this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
			this.kryptonSplitContainer1.PaletteMode = PaletteMode.Global;
			// 
			// kryptonSplitContainer1.Panel1
			// 
			this.kryptonSplitContainer1.Panel1.Controls.Add(this.infoTree_);
			this.kryptonSplitContainer1.Panel1.PaletteMode = PaletteMode.Global;
			this.kryptonSplitContainer1.Panel1.PanelBackStyle = PaletteBackStyle.PanelClient;
			// 
			// kryptonSplitContainer1.Panel2
			// 
			this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonSplitContainer2);
			this.kryptonSplitContainer1.Panel2.PaletteMode = PaletteMode.Global;
			this.kryptonSplitContainer1.Panel2.PanelBackStyle = PaletteBackStyle.PanelClient;
			this.kryptonSplitContainer1.SeparatorStyle = SeparatorStyle.LowProfile;
			this.kryptonSplitContainer1.Size = new System.Drawing.Size(626, 389);
			this.kryptonSplitContainer1.SplitterDistance = 389;
			this.kryptonSplitContainer1.TabIndex = 4;
			// 
			// infoTree_
			// 
			this.infoTree_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoTree_.Location = new System.Drawing.Point(0, 0);
			this.infoTree_.Name = "infoTree_";
			this.infoTree_.Size = new System.Drawing.Size(389, 389);
			this.infoTree_.TabIndex = 3;
			this.infoTree_.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.infoTree__NodeMouseClick);
			// 
			// kryptonSplitContainer2
			// 
			this.kryptonSplitContainer2.ContainerBackStyle = PaletteBackStyle.PanelClient;
			this.kryptonSplitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
			this.kryptonSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonSplitContainer2.Location = new System.Drawing.Point(0, 0);
			this.kryptonSplitContainer2.Name = "kryptonSplitContainer2";
			this.kryptonSplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.kryptonSplitContainer2.PaletteMode = PaletteMode.Global;
			// 
			// kryptonSplitContainer2.Panel1
			// 
			this.kryptonSplitContainer2.Panel1.Controls.Add(this.propertyGrid_);
			this.kryptonSplitContainer2.Panel1.PaletteMode = PaletteMode.Global;
			this.kryptonSplitContainer2.Panel1.PanelBackStyle = PaletteBackStyle.PanelClient;
			// 
			// kryptonSplitContainer2.Panel2
			// 
			this.kryptonSplitContainer2.Panel2.Controls.Add(this.detailList_);
			this.kryptonSplitContainer2.Panel2.PaletteMode = PaletteMode.Global;
			this.kryptonSplitContainer2.Panel2.PanelBackStyle =PaletteBackStyle.PanelClient;
			this.kryptonSplitContainer2.SeparatorStyle = SeparatorStyle.LowProfile;
			this.kryptonSplitContainer2.Size = new System.Drawing.Size(232, 389);
			this.kryptonSplitContainer2.SplitterDistance = 196;
			this.kryptonSplitContainer2.TabIndex = 1;
			// 
			// propertyGrid_
			// 
			this.propertyGrid_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid_.HelpVisible = false;
			this.propertyGrid_.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid_.Name = "propertyGrid_";
			this.propertyGrid_.Size = new System.Drawing.Size(232, 196);
			this.propertyGrid_.TabIndex = 0;
			// 
			// detailList_
			// 
			this.detailList_.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.ValueColumn});
			this.detailList_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.detailList_.Location = new System.Drawing.Point(0, 0);
			this.detailList_.Name = "detailList_";
			this.detailList_.Size = new System.Drawing.Size(232, 188);
			this.detailList_.TabIndex = 0;
			this.detailList_.UseCompatibleStateImageBehavior = false;
			this.detailList_.View = System.Windows.Forms.View.Details;
			// 
			// NameColumn
			// 
			this.NameColumn.Text = "Name";
			// 
			// ValueColumn
			// 
			this.ValueColumn.Text = "Value";
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(0, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 389);
			this.splitter2.TabIndex = 1;
			this.splitter2.TabStop = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLbl_});
			this.statusStrip1.Location = new System.Drawing.Point(0, 393);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(637, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusLbl_
			// 
			this.statusLbl_.Name = "statusLbl_";
			this.statusLbl_.Size = new System.Drawing.Size(0, 17);
			// 
			// DesignTimeDebuggerDisplayUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(637, 415);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tabControl1);
			this.Name = "DesignTimeDebuggerDisplayUI";
			this.Text = "Design Time Debug Info";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
			this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
			this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
			this.kryptonSplitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel1)).EndInit();
			this.kryptonSplitContainer2.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel2)).EndInit();
			this.kryptonSplitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).EndInit();
			this.kryptonSplitContainer2.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox msgs_;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ListBox dataTraceList_;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel statusLbl_;
		private System.Windows.Forms.TreeView infoTree_;
		private System.Windows.Forms.TreeView dataTraceTree_;
		private System.Windows.Forms.Button button1;
		private KryptonSplitContainer kryptonSplitContainer1;
		private KryptonSplitContainer kryptonSplitContainer2;
		private System.Windows.Forms.PropertyGrid propertyGrid_;
		private System.Windows.Forms.ListView detailList_;
		private System.Windows.Forms.ColumnHeader NameColumn;
		private System.Windows.Forms.ColumnHeader ValueColumn;
	}
}