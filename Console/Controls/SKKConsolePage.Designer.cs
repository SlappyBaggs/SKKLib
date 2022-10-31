namespace SKKLib.Console.Controls
{
    partial class SKKConsolePage
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
            this.tbRich = new ComponentFactory.Krypton.Toolkit.KryptonRichTextBox();
            this.groupPage = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.butClear = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            ((System.ComponentModel.ISupportInitialize)(this.groupPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPage.Panel)).BeginInit();
            this.groupPage.Panel.SuspendLayout();
            this.groupPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbRich
            // 
            this.tbRich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRich.Location = new System.Drawing.Point(0, 0);
            this.tbRich.Name = "tbRich";
            this.tbRich.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.tbRich.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbRich.Size = new System.Drawing.Size(894, 459);
            this.tbRich.StateCommon.Back.Color1 = System.Drawing.Color.Black;
            this.tbRich.TabIndex = 0;
            this.tbRich.Text = "";
            this.tbRich.TextChanged += new System.EventHandler(this.tbRich_TextChanged);
            this.tbRich.SystemColorsChanged += new System.EventHandler(this.tbRich_SystemColorsChanged);
            // 
            // groupPage
            // 
            this.groupPage.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup[] {
            this.butClear});
            this.groupPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPage.HeaderVisiblePrimary = false;
            this.groupPage.Location = new System.Drawing.Point(0, 0);
            this.groupPage.Name = "groupPage";
            this.groupPage.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            // 
            // groupPage.Panel
            // 
            this.groupPage.Panel.Controls.Add(this.tbRich);
            this.groupPage.Size = new System.Drawing.Size(896, 488);
            this.groupPage.TabIndex = 1;
            this.groupPage.ValuesSecondary.Heading = "";
            // 
            // butClear
            // 
            this.butClear.HeaderLocation = ComponentFactory.Krypton.Toolkit.HeaderLocation.SecondaryHeader;
            this.butClear.Text = "Clear";
            this.butClear.UniqueName = "F5D8967F3F114FB27DA96978CA25C3AB";
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // SKKConsolePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupPage);
            this.Name = "SKKConsolePage";
            this.Size = new System.Drawing.Size(896, 488);
            ((System.ComponentModel.ISupportInitialize)(this.groupPage.Panel)).EndInit();
            this.groupPage.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupPage)).EndInit();
            this.groupPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal ComponentFactory.Krypton.Toolkit.KryptonRichTextBox tbRich;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup groupPage;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup butClear;
    }
}
