namespace SKKLib.Controls.Forms
{
    partial class _TestForm
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
            this.kryptonTextBox1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.skkCheckBox1 = new SKKLib.Controls.Controls.SKKCheckBox();
            this.SuspendLayout();
            // 
            // kryptonTextBox1
            // 
            this.kryptonTextBox1.Location = new System.Drawing.Point(264, 200);
            this.kryptonTextBox1.Name = "kryptonTextBox1";
            this.kryptonTextBox1.Size = new System.Drawing.Size(100, 24);
            this.kryptonTextBox1.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)(((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)));
            this.kryptonTextBox1.StateNormal.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            this.kryptonTextBox1.TabIndex = 0;
            this.kryptonTextBox1.Text = "kryptonTextBox1";
            // 
            // skkCheckBox1
            // 
            this.skkCheckBox1.CheckSize = 15;
            this.skkCheckBox1.Location = new System.Drawing.Point(486, 185);
            this.skkCheckBox1.Name = "skkCheckBox1";
            this.skkCheckBox1.Padding = new System.Windows.Forms.Padding(3);
            this.skkCheckBox1.Size = new System.Drawing.Size(80, 25);
            this.skkCheckBox1.TabIndex = 1;
            this.skkCheckBox1.CheckBox_ClickedEvent += new System.EventHandler<SKKLib.Controls.Data.CheckBoxEventArgs>(this.skkCheckBox1_CheckBox_ClickedEvent);
            // 
            // _TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.skkCheckBox1);
            this.Controls.Add(this.kryptonTextBox1);
            this.Name = "_TestForm";
            this.Text = "_TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonTextBox kryptonTextBox1;
        private Controls.SKKCheckBox skkCheckBox1;
    }
}