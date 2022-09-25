using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Design
{
	partial class ModalVisualizerForm
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
			this.kryptonSplitContainer1 = new KryptonSplitContainer();
			this.cancelButton_ = new KryptonButton();
			this.okButton_ = new KryptonButton();
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
			this.kryptonSplitContainer1.Panel2.SuspendLayout();
			this.kryptonSplitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// kryptonSplitContainer1
			// 
			this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
			this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kryptonSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.kryptonSplitContainer1.IsSplitterFixed = true;
			this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
			this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
			this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// kryptonSplitContainer1.Panel2
			// 
			this.kryptonSplitContainer1.Panel2.Controls.Add(this.cancelButton_);
			this.kryptonSplitContainer1.Panel2.Controls.Add(this.okButton_);
			this.kryptonSplitContainer1.Size = new System.Drawing.Size(292, 266);
			this.kryptonSplitContainer1.SplitterDistance = 228;
			this.kryptonSplitContainer1.TabIndex = 0;
			// 
			// cancelButton_
			// 
			this.cancelButton_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton_.Location = new System.Drawing.Point(210, 4);
			this.cancelButton_.Name = "cancelButton_";
			this.cancelButton_.Size = new System.Drawing.Size(79, 26);
			this.cancelButton_.TabIndex = 1;
			this.cancelButton_.Values.Text = "Cancel";
			this.cancelButton_.Click += new System.EventHandler(this.cancelButton__Click);
			// 
			// okButton_
			// 
			this.okButton_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton_.Location = new System.Drawing.Point(125, 4);
			this.okButton_.Name = "okButton_";
			this.okButton_.Size = new System.Drawing.Size(79, 26);
			this.okButton_.TabIndex = 0;
			this.okButton_.Values.Text = "OK";
			this.okButton_.Click += new System.EventHandler(this.okButton__Click);
			// 
			// ModalVisualizerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.kryptonSplitContainer1);
			this.Name = "ModalVisualizerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Data Editor";
			this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
			this.kryptonSplitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private KryptonSplitContainer kryptonSplitContainer1;
		private KryptonButton cancelButton_;
		private KryptonButton okButton_;
	}
}

