using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Forms
{
    partial class QuestionBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestionBox));
            this.tbMessage = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.butYes = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.butNo = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(12, 12);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.Size = new System.Drawing.Size(260, 107);
            this.tbMessage.TabIndex = 0;
            // 
            // butYes
            // 
            this.butYes.Location = new System.Drawing.Point(83, 125);
            this.butYes.Name = "butYes";
            this.butYes.Size = new System.Drawing.Size(50, 25);
            this.butYes.TabIndex = 1;
            this.butYes.Values.Text = "Yes";
            this.butYes.Click += new System.EventHandler(this.butYES_Click);
            // 
            // butNo
            // 
            this.butNo.Location = new System.Drawing.Point(139, 125);
            this.butNo.Name = "butNo";
            this.butNo.Size = new System.Drawing.Size(50, 25);
            this.butNo.TabIndex = 2;
            this.butNo.Values.Text = "No";
            this.butNo.Click += new System.EventHandler(this.butNo_Click);
            // 
            // QuestionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 162);
            this.Controls.Add(this.butNo);
            this.Controls.Add(this.butYes);
            this.Controls.Add(this.tbMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuestionBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SKKQuestionBox";
            this.Load += new System.EventHandler(this.STECMessageBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonTextBox tbMessage;
        private KryptonButton butYes;
        private KryptonButton butNo;
    }
}
