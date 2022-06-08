using ComponentFactory.Krypton.Toolkit;
using System.Windows.Forms;

namespace SKKLib.Serial.Controls
{
    partial class SKKSerialSelectBox
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
            this.cbPortName = new KryptonComboBox();
            this.labPortName = new KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.cbPortName)).BeginInit();
            this.SuspendLayout();
            // 
            // cbPortName
            // 
            this.cbPortName.DropDownWidth = 72;
            this.cbPortName.FormattingEnabled = true;
            this.cbPortName.Location = new System.Drawing.Point(53, 0);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new System.Drawing.Size(72, 21);
            this.cbPortName.TabIndex = 0;
            this.cbPortName.Text = "COM1";
            this.cbPortName.SelectedIndexChanged += new System.EventHandler(this.cbPortName_SelectedIndexChanged);
            // 
            // labPortName
            // 
            this.labPortName.Name = "labPortName";
            this.labPortName.Size = new System.Drawing.Size(59, 20);
            this.labPortName.TabIndex = 1;
            this.labPortName.Values.Text = "ComPort";
            // 
            // SKKSerialSelectBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbPortName);
            this.Controls.Add(this.labPortName);
            this.Name = "SKKSerialSelectBox";
            this.Size = new System.Drawing.Size(125, 21);
            this.FontChanged += new System.EventHandler(this.SKKSerialSelectBox_FontChanged);
            this.ParentChanged += new System.EventHandler(this.SKKSerialSelectBox_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.cbPortName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private KryptonComboBox cbPortName;
        private KryptonLabel labPortName;
    }
}
