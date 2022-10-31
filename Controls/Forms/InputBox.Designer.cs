using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Forms
{
    partial class InputBox
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
            this.tbInput = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.group1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.butOK = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.butCancel = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            ((System.ComponentModel.ISupportInitialize)(this.group1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.group1.Panel)).BeginInit();
            this.group1.Panel.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(3, 3);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(502, 24);
            this.tbInput.TabIndex = 0;
            this.tbInput.Text = "Enter your input here";
            this.tbInput.MultilineChanged += new System.EventHandler(this.tbInput_MultilineChanged);
            this.tbInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbInput_KeyPress);
            // 
            // group1
            // 
            this.group1.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup[] {
            this.butOK,
            this.butCancel});
            this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.HeaderVisiblePrimary = false;
            this.group1.Location = new System.Drawing.Point(0, 0);
            this.group1.Name = "group1";
            // 
            // group1.Panel
            // 
            this.group1.Panel.Controls.Add(this.tbInput);
            this.group1.Size = new System.Drawing.Size(484, 61);
            this.group1.TabIndex = 2;
            this.group1.ValuesSecondary.Heading = "";
            // 
            // butOK
            // 
            this.butOK.Edge = ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign.Near;
            this.butOK.HeaderLocation = ComponentFactory.Krypton.Toolkit.HeaderLocation.SecondaryHeader;
            this.butOK.Text = "OK";
            this.butOK.UniqueName = "4FFD2C907A2147570F94C228FA639BB0";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Edge = ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign.Far;
            this.butCancel.HeaderLocation = ComponentFactory.Krypton.Toolkit.HeaderLocation.SecondaryHeader;
            this.butCancel.Text = "Cancel";
            this.butCancel.UniqueName = "2D373718DD4B411C3CAE4E5C067B6A10";
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 61);
            this.Controls.Add(this.group1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputBox";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.InputBox_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.group1.Panel)).EndInit();
            this.group1.Panel.ResumeLayout(false);
            this.group1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.group1)).EndInit();
            this.group1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private KryptonTextBox tbInput;
        private KryptonHeaderGroup group1;
        private ButtonSpecHeaderGroup butOK;
        private ButtonSpecHeaderGroup butCancel;
    }
}