using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Controls
{
	partial class ViewPanel
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
			this.panel_ = new KryptonPanel();
			((System.ComponentModel.ISupportInitialize)(this.panel_)).BeginInit();
			// 
			// panel_
			// 
			this.panel_.Location = new System.Drawing.Point(0, 0);
			this.panel_.Name = "panel_";
			this.panel_.PaletteMode = PaletteMode.Global;
			this.panel_.PanelBackStyle = PaletteBackStyle.PanelClient;
			this.panel_.Size = new System.Drawing.Size(100, 100);
			this.panel_.TabIndex = 0;
			((System.ComponentModel.ISupportInitialize)(this.panel_)).EndInit();

		}

		#endregion

		private KryptonPanel panel_;
	}
}
