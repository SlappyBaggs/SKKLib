using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Forms
{
    partial class MessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBox));
            this.tbMessage = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.butOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
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
            this.tbMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SKKMessageBox_KeyPress);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(110, 125);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(50, 25);
            this.butOK.TabIndex = 1;
            this.butOK.Values.Text = "OK";
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            this.butOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SKKMessageBox_KeyPress);
            // 
            // MessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 162);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.tbMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SKKMessageBox";
            this.Load += new System.EventHandler(this.SKKMessageBox_Load);
            this.SizeChanged += new System.EventHandler(this.MessageBox_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SKKMessageBox_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonTextBox tbMessage;
        private KryptonButton butOK;
    }
}
