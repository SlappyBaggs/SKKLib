namespace SKKLib.Controls.Controls
{
    partial class SKKComboListBox
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
            this.but1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.tb1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.listView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // but1
            // 
            this.but1.Location = new System.Drawing.Point(100, 0);
            this.but1.Name = "but1";
            this.but1.Size = new System.Drawing.Size(23, 23);
            this.but1.TabIndex = 0;
            this.but1.Values.Text = "kryptonButton1";
            this.but1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // tb1
            // 
            this.tb1.Location = new System.Drawing.Point(0, 0);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(100, 23);
            this.tb1.TabIndex = 1;
            // 
            // listView
            // 
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 23);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(123, 96);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // SKKComboListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Controls.Add(this.tb1);
            this.Controls.Add(this.but1);
            this.Name = "SKKComboListBox";
            this.Size = new System.Drawing.Size(123, 119);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton but1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tb1;
        private System.Windows.Forms.ListView listView;
    }
}
