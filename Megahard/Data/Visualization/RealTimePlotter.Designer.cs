namespace Megahard.Data.Visualization
{
	partial class RealTimePlotter
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
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// RealTimePlotter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "RealTimePlotter";
			this.Size = new System.Drawing.Size(304, 150);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.RealTimePlotter_Paint);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timer1;

	}
}
