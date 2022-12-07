namespace SKKLib.Controls.Forms
{
    partial class SKKPaddingEdit
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
            this.groupPadding = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.butOK = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.butCancel = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.labLeft = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.numLeft = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.labBottom = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.labRight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.numBottom = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.numRight = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.labTop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.numTop = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.labAll = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.numAll = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.groupPadding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPadding.Panel)).BeginInit();
            this.groupPadding.Panel.SuspendLayout();
            this.groupPadding.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPadding
            // 
            this.groupPadding.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup[] {
            this.butOK,
            this.butCancel});
            this.groupPadding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPadding.HeaderVisiblePrimary = false;
            this.groupPadding.Location = new System.Drawing.Point(0, 0);
            this.groupPadding.Name = "groupPadding";
            // 
            // groupPadding.Panel
            // 
            this.groupPadding.Panel.Controls.Add(this.labLeft);
            this.groupPadding.Panel.Controls.Add(this.numLeft);
            this.groupPadding.Panel.Controls.Add(this.labBottom);
            this.groupPadding.Panel.Controls.Add(this.labRight);
            this.groupPadding.Panel.Controls.Add(this.numBottom);
            this.groupPadding.Panel.Controls.Add(this.numRight);
            this.groupPadding.Panel.Controls.Add(this.labTop);
            this.groupPadding.Panel.Controls.Add(this.numTop);
            this.groupPadding.Panel.Controls.Add(this.labAll);
            this.groupPadding.Panel.Controls.Add(this.numAll);
            this.groupPadding.Size = new System.Drawing.Size(288, 142);
            this.groupPadding.TabIndex = 0;
            this.groupPadding.ValuesSecondary.Heading = "";
            // 
            // butOK
            // 
            this.butOK.HeaderLocation = ComponentFactory.Krypton.Toolkit.HeaderLocation.SecondaryHeader;
            this.butOK.Text = "OK";
            this.butOK.UniqueName = "BC8D2D81F13E4C46D48D2FF4651C668B";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.HeaderLocation = ComponentFactory.Krypton.Toolkit.HeaderLocation.SecondaryHeader;
            this.butCancel.Text = "Cancel";
            this.butCancel.UniqueName = "42C551DE09D740EA1FA25335F84CCD0E";
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // labLeft
            // 
            this.labLeft.Location = new System.Drawing.Point(44, 53);
            this.labLeft.Name = "labLeft";
            this.labLeft.Size = new System.Drawing.Size(31, 20);
            this.labLeft.TabIndex = 7;
            this.labLeft.Values.Text = "Left";
            // 
            // numLeft
            // 
            this.numLeft.Location = new System.Drawing.Point(76, 51);
            this.numLeft.Name = "numLeft";
            this.numLeft.Size = new System.Drawing.Size(51, 22);
            this.numLeft.TabIndex = 6;
            this.numLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labBottom
            // 
            this.labBottom.Location = new System.Drawing.Point(87, 81);
            this.labBottom.Name = "labBottom";
            this.labBottom.Size = new System.Drawing.Size(51, 20);
            this.labBottom.TabIndex = 5;
            this.labBottom.Values.Text = "Bottom";
            // 
            // labRight
            // 
            this.labRight.Location = new System.Drawing.Point(186, 53);
            this.labRight.Name = "labRight";
            this.labRight.Size = new System.Drawing.Size(39, 20);
            this.labRight.TabIndex = 5;
            this.labRight.Values.Text = "Right";
            // 
            // numBottom
            // 
            this.numBottom.Location = new System.Drawing.Point(139, 79);
            this.numBottom.Name = "numBottom";
            this.numBottom.Size = new System.Drawing.Size(51, 22);
            this.numBottom.TabIndex = 4;
            this.numBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numRight
            // 
            this.numRight.Location = new System.Drawing.Point(225, 51);
            this.numRight.Name = "numRight";
            this.numRight.Size = new System.Drawing.Size(51, 22);
            this.numRight.TabIndex = 4;
            this.numRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labTop
            // 
            this.labTop.Location = new System.Drawing.Point(107, 25);
            this.labTop.Name = "labTop";
            this.labTop.Size = new System.Drawing.Size(31, 20);
            this.labTop.TabIndex = 3;
            this.labTop.Values.Text = "Top";
            // 
            // numTop
            // 
            this.numTop.Location = new System.Drawing.Point(139, 23);
            this.numTop.Name = "numTop";
            this.numTop.Size = new System.Drawing.Size(51, 22);
            this.numTop.TabIndex = 2;
            this.numTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labAll
            // 
            this.labAll.Location = new System.Drawing.Point(12, 12);
            this.labAll.Name = "labAll";
            this.labAll.Size = new System.Drawing.Size(24, 20);
            this.labAll.TabIndex = 1;
            this.labAll.Values.Text = "All";
            // 
            // numAll
            // 
            this.numAll.Location = new System.Drawing.Point(42, 10);
            this.numAll.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numAll.Name = "numAll";
            this.numAll.Size = new System.Drawing.Size(51, 22);
            this.numAll.TabIndex = 0;
            this.numAll.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SKKPaddingEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 142);
            this.Controls.Add(this.groupPadding);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SKKPaddingEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Padding";
            ((System.ComponentModel.ISupportInitialize)(this.groupPadding.Panel)).EndInit();
            this.groupPadding.Panel.ResumeLayout(false);
            this.groupPadding.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupPadding)).EndInit();
            this.groupPadding.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup groupPadding;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup butOK;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup butCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel labLeft;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown numLeft;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel labBottom;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel labRight;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown numBottom;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown numRight;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel labTop;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown numTop;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel labAll;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown numAll;
    }
}