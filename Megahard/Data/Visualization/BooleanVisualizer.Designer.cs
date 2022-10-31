using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	partial class BooleanVisualizer
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
			this.chkBox_ = new KryptonCheckBox();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// chkBox_
			// 
			this.chkBox_.Location = new System.Drawing.Point(0, 0);
			this.chkBox_.Name = "chkBox_";
			this.chkBox_.Size = new System.Drawing.Size(19, 13);
			this.chkBox_.TabIndex = 0;
			this.chkBox_.Values.ExtraText = "";
			this.chkBox_.Values.Image = null;
			this.chkBox_.Values.Text = "";
			this.chkBox_.CheckedChanged += new System.EventHandler(this.chkBox__CheckedChanged);
			// 
			// BooleanVisualizer
			// 
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.chkBox_);
			this.Name = "BooleanVisualizer";
			this.Size = new System.Drawing.Size(22, 16);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private KryptonCheckBox chkBox_;
	}
}
