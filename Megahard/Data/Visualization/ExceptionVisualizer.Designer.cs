using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Data.Visualization
{
	partial class ExceptionVisualizer
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionVisualizer));
			this.hdr_ = new KryptonHeader();
			this.dropButtonSpec_ = new ButtonSpecAny();
			this.dropDownControl_ = new Megahard.Controls.DropDownControl(this.components);
			this.msgLong_ = new KryptonTextBox();
			this.stackTrace_ = new KryptonTextBox();
			this.dropPanel_ = new KryptonPanel();
			((System.ComponentModel.ISupportInitialize)(this.dropPanel_)).BeginInit();
			this.dropPanel_.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// hdr_
			// 
			this.hdr_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.hdr_.AutoSize = false;
			this.hdr_.ButtonSpecs.AddRange(new ButtonSpecAny[] {
            this.dropButtonSpec_});
			this.hdr_.HeaderStyle = HeaderStyle.Secondary;
			this.hdr_.Location = new System.Drawing.Point(0, 0);
			this.hdr_.MinimumSize = new System.Drawing.Size(100, 30);
			this.hdr_.Name = "hdr_";
			this.hdr_.PaletteMode = PaletteMode.Global;
			this.hdr_.Size = new System.Drawing.Size(295, 30);
			this.hdr_.TabIndex = 0;
			this.hdr_.Text = "Exception";
			this.hdr_.Values.Description = "";
			this.hdr_.Values.Heading = "Exception";
			this.hdr_.Values.Image = ((System.Drawing.Image)(resources.GetObject("hdr_.Values.Image")));
			// 
			// dropButtonSpec_
			// 
			this.dropButtonSpec_.Edge = PaletteRelativeEdgeAlign.Inherit;
			this.dropButtonSpec_.ExtraText = "";
			this.dropButtonSpec_.Image = null;
			this.dropButtonSpec_.Orientation = PaletteButtonOrientation.Inherit;
			this.dropButtonSpec_.Text = "";
			this.dropButtonSpec_.Type = PaletteButtonSpecStyle.DropDown;
			this.dropButtonSpec_.UniqueName = "8A6567E7CAFE4BBA8A6567E7CAFE4BBA";
			this.dropButtonSpec_.Click += new System.EventHandler(this.dropButtonSpec__Click);
			// 
			// msgLong_
			// 
			this.msgLong_.Dock = System.Windows.Forms.DockStyle.Top;
			this.msgLong_.InputControlStyle = InputControlStyle.Standalone;
			this.msgLong_.Location = new System.Drawing.Point(0, 0);
			this.msgLong_.Multiline = true;
			this.msgLong_.Name = "msgLong_";
			this.msgLong_.PaletteMode = PaletteMode.Global;
			this.msgLong_.ReadOnly = true;
			this.msgLong_.Size = new System.Drawing.Size(289, 29);
			this.msgLong_.TabIndex = 0;
			// 
			// stackTrace_
			// 
			this.stackTrace_.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stackTrace_.InputControlStyle = InputControlStyle.Standalone;
			this.stackTrace_.Location = new System.Drawing.Point(0, 29);
			this.stackTrace_.Multiline = true;
			this.stackTrace_.Name = "stackTrace_";
			this.stackTrace_.PaletteMode = PaletteMode.Global;
			this.stackTrace_.ReadOnly = true;
			this.stackTrace_.Size = new System.Drawing.Size(289, 204);
			this.stackTrace_.TabIndex = 1;
			// 
			// dropPanel_
			// 
			this.dropPanel_.Controls.Add(this.stackTrace_);
			this.dropPanel_.Controls.Add(this.msgLong_);
			this.dropPanel_.Location = new System.Drawing.Point(3, 36);
			this.dropPanel_.Name = "dropPanel_";
			this.dropPanel_.PaletteMode = PaletteMode.Global;
			this.dropPanel_.PanelBackStyle = PaletteBackStyle.PanelClient;
			this.dropPanel_.Size = new System.Drawing.Size(289, 233);
			this.dropPanel_.TabIndex = 2;
			// 
			// ExceptionVisualizer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.dropPanel_);
			this.Controls.Add(this.hdr_);
			this.Name = "ExceptionVisualizer";
			this.Size = new System.Drawing.Size(295, 272);
			((System.ComponentModel.ISupportInitialize)(this.dropPanel_)).EndInit();
			this.dropPanel_.ResumeLayout(false);
			this.dropPanel_.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private KryptonHeader hdr_;
		private Megahard.Controls.DropDownControl dropDownControl_;
		private ButtonSpecAny dropButtonSpec_;
		private KryptonTextBox msgLong_;
		private KryptonTextBox stackTrace_;
		private KryptonPanel dropPanel_;


	}
}
