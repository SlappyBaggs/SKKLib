using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Megahard.Controls
{
	public partial class LEDControl
	{
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LEDControl));
			this.imageList_ = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageList_
			// 
			this.imageList_.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_.ImageStream")));
			this.imageList_.TransparentColor = System.Drawing.Color.Magenta;
			this.imageList_.Images.SetKeyName(0, "leds.bmp");
			// 
			// LEDControl
			// 
			this.Size = new System.Drawing.Size(16, 16);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.LEDControl_Paint);
			this.EnabledChanged += new System.EventHandler(this.LEDControl_EnabledChanged);
			this.ResumeLayout(false);

		}

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

		private ImageList imageList_;
		private IContainer components;

	}

}
