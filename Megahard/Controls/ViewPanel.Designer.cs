using ComponentFactory.Krypton.Toolkit;

namespace Megahard.Controls
{
	partial class ViewPanel
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
