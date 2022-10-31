﻿using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data
{
	partial class DataObjectEditor
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
			this.kryptonSplitContainer1 = new KryptonSplitContainer();
			this.kryptonHeaderGroup1 = new KryptonHeaderGroup();
			this.componentList_ = new KryptonListBox();
			this.kryptonHeaderGroup2 = new KryptonHeaderGroup();
			this.propertyTree_ = new Megahard.Controls.PropertyTree();
			this.kryptonHeaderGroup3 = new KryptonHeaderGroup();
			this.buttonSpecHeaderGroup1 = new ButtonSpecHeaderGroup();
			this.okCmd_ = new KryptonCommand();
			this.buttonSpecHeaderGroup2 = new ButtonSpecHeaderGroup();
			this.cancelCmd_ = new KryptonCommand();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
			this.kryptonSplitContainer1.Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
			this.kryptonSplitContainer1.Panel2.SuspendLayout();
			this.kryptonSplitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
			this.kryptonHeaderGroup1.Panel.SuspendLayout();
			this.kryptonHeaderGroup1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2.Panel)).BeginInit();
			this.kryptonHeaderGroup2.Panel.SuspendLayout();
			this.kryptonHeaderGroup2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup3.Panel)).BeginInit();
			this.kryptonHeaderGroup3.Panel.SuspendLayout();
			this.kryptonHeaderGroup3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// kryptonSplitContainer1
			// 
			this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
			this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
			this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
			// 
			// kryptonSplitContainer1.Panel1
			// 
			this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonHeaderGroup1);
			// 
			// kryptonSplitContainer1.Panel2
			// 
			this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonHeaderGroup2);
			this.kryptonSplitContainer1.Size = new System.Drawing.Size(329, 219);
			this.kryptonSplitContainer1.SplitterDistance = 151;
			this.kryptonSplitContainer1.TabIndex = 1;
			// 
			// kryptonHeaderGroup1
			// 
			this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonHeaderGroup1.HeaderVisibleSecondary = false;
			this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
			this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
			// 
			// kryptonHeaderGroup1.Panel
			// 
			this.kryptonHeaderGroup1.Panel.Controls.Add(this.componentList_);
			this.kryptonHeaderGroup1.Size = new System.Drawing.Size(151, 219);
			this.kryptonHeaderGroup1.TabIndex = 0;
			this.kryptonHeaderGroup1.Text = "Data Source";
			this.kryptonHeaderGroup1.ValuesPrimary.Description = "";
			this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Data Source";
			this.kryptonHeaderGroup1.ValuesPrimary.Image = null;
			this.kryptonHeaderGroup1.ValuesSecondary.Description = "";
			this.kryptonHeaderGroup1.ValuesSecondary.Heading = "Description";
			this.kryptonHeaderGroup1.ValuesSecondary.Image = null;
			// 
			// componentList_
			// 
			this.componentList_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.componentList_.Location = new System.Drawing.Point(0, 0);
			this.componentList_.Name = "componentList_";
			this.componentList_.Size = new System.Drawing.Size(149, 187);
			this.componentList_.TabIndex = 0;
			this.componentList_.SelectedValueChanged += new System.EventHandler(this.componentList__SelectedValueChanged);
			// 
			// kryptonHeaderGroup2
			// 
			this.kryptonHeaderGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonHeaderGroup2.HeaderVisibleSecondary = false;
			this.kryptonHeaderGroup2.Location = new System.Drawing.Point(0, 0);
			this.kryptonHeaderGroup2.Name = "kryptonHeaderGroup2";
			// 
			// kryptonHeaderGroup2.Panel
			// 
			this.kryptonHeaderGroup2.Panel.Controls.Add(this.propertyTree_);
			this.kryptonHeaderGroup2.Size = new System.Drawing.Size(173, 219);
			this.kryptonHeaderGroup2.TabIndex = 0;
			this.kryptonHeaderGroup2.Text = "Bound Property";
			this.kryptonHeaderGroup2.ValuesPrimary.Description = "";
			this.kryptonHeaderGroup2.ValuesPrimary.Heading = "Bound Property";
			this.kryptonHeaderGroup2.ValuesPrimary.Image = null;
			this.kryptonHeaderGroup2.ValuesSecondary.Description = "";
			this.kryptonHeaderGroup2.ValuesSecondary.Heading = "Description";
			this.kryptonHeaderGroup2.ValuesSecondary.Image = null;
			// 
			// propertyTree_
			// 
			this.propertyTree_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyTree_.Location = new System.Drawing.Point(0, 0);
			this.propertyTree_.Name = "propertyTree_";
			this.propertyTree_.Size = new System.Drawing.Size(171, 187);
			this.propertyTree_.TabIndex = 0;
			// 
			// kryptonHeaderGroup3
			// 
			this.kryptonHeaderGroup3.ButtonSpecs.AddRange(new ButtonSpecHeaderGroup[] {
            this.buttonSpecHeaderGroup1,
            this.buttonSpecHeaderGroup2});
			this.kryptonHeaderGroup3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonHeaderGroup3.HeaderVisiblePrimary = false;
			this.kryptonHeaderGroup3.Location = new System.Drawing.Point(0, 0);
			this.kryptonHeaderGroup3.Name = "kryptonHeaderGroup3";
			// 
			// kryptonHeaderGroup3.Panel
			// 
			this.kryptonHeaderGroup3.Panel.Controls.Add(this.kryptonSplitContainer1);
			this.kryptonHeaderGroup3.Size = new System.Drawing.Size(331, 250);
			this.kryptonHeaderGroup3.TabIndex = 2;
			this.kryptonHeaderGroup3.Text = "Heading";
			this.kryptonHeaderGroup3.ValuesPrimary.Description = "";
			this.kryptonHeaderGroup3.ValuesPrimary.Heading = "Heading";
			this.kryptonHeaderGroup3.ValuesPrimary.Image = null;
			this.kryptonHeaderGroup3.ValuesSecondary.Description = "";
			this.kryptonHeaderGroup3.ValuesSecondary.Heading = "";
			this.kryptonHeaderGroup3.ValuesSecondary.Image = null;
			// 
			// buttonSpecHeaderGroup1
			// 
			this.buttonSpecHeaderGroup1.Enabled = ButtonEnabled.True;
			this.buttonSpecHeaderGroup1.ExtraText = "";
			this.buttonSpecHeaderGroup1.HeaderLocation = HeaderLocation.SecondaryHeader;
			this.buttonSpecHeaderGroup1.Image = null;
			this.buttonSpecHeaderGroup1.KryptonCommand = this.okCmd_;
			this.buttonSpecHeaderGroup1.Style = PaletteButtonStyle.Standalone;
			this.buttonSpecHeaderGroup1.Text = "OK";
			this.buttonSpecHeaderGroup1.UniqueName = "32AD5B9728514C1232AD5B9728514C12";
			// 
			// okCmd_
			// 
			this.okCmd_.ExtraText = "";
			this.okCmd_.ImageLarge = null;
			this.okCmd_.ImageSmall = null;
			this.okCmd_.Text = "OK";
			this.okCmd_.TextLine1 = "";
			this.okCmd_.TextLine2 = "";
			this.okCmd_.Execute += new System.EventHandler(this.okCmd__Execute);
			// 
			// buttonSpecHeaderGroup2
			// 
			this.buttonSpecHeaderGroup2.Enabled = ButtonEnabled.True;
			this.buttonSpecHeaderGroup2.ExtraText = "";
			this.buttonSpecHeaderGroup2.HeaderLocation = HeaderLocation.SecondaryHeader;
			this.buttonSpecHeaderGroup2.Image = null;
			this.buttonSpecHeaderGroup2.KryptonCommand = this.cancelCmd_;
			this.buttonSpecHeaderGroup2.Style = PaletteButtonStyle.Standalone;
			this.buttonSpecHeaderGroup2.Text = "Cancel";
			this.buttonSpecHeaderGroup2.UniqueName = "BC75F56D6B584837BC75F56D6B584837";
			// 
			// cancelCmd_
			// 
			this.cancelCmd_.ExtraText = "";
			this.cancelCmd_.ImageLarge = null;
			this.cancelCmd_.ImageSmall = null;
			this.cancelCmd_.Text = "Cancel";
			this.cancelCmd_.TextLine1 = "";
			this.cancelCmd_.TextLine2 = "";
			this.cancelCmd_.Execute += new System.EventHandler(this.cancelCmd__Execute);
			// 
			// DataObjectEditor
			// 
			this.Controls.Add(this.kryptonHeaderGroup3);
			this.Name = "DataObjectEditor";
			this.Size = new System.Drawing.Size(331, 250);
			this.ObjectChanged += new System.EventHandler<Megahard.Data.ObjectChangedEventArgs>(this.DataBoxDataEditor_ObjectChanged);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
			this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
			this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
			this.kryptonSplitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
			this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
			this.kryptonHeaderGroup1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2.Panel)).EndInit();
			this.kryptonHeaderGroup2.Panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2)).EndInit();
			this.kryptonHeaderGroup2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup3.Panel)).EndInit();
			this.kryptonHeaderGroup3.Panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup3)).EndInit();
			this.kryptonHeaderGroup3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private KryptonSplitContainer kryptonSplitContainer1;
		private KryptonHeaderGroup kryptonHeaderGroup1;
		private KryptonListBox componentList_;
		private KryptonHeaderGroup kryptonHeaderGroup2;
		private Megahard.Controls.PropertyTree propertyTree_;
		private KryptonHeaderGroup kryptonHeaderGroup3;
		private ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
		private ButtonSpecHeaderGroup buttonSpecHeaderGroup2;
		private KryptonCommand okCmd_;
		private KryptonCommand cancelCmd_;



	}
}
