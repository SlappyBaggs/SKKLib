using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Controls
{
    partial class KeyPad
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
            this.but1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but6 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but5 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but9 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but8 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but7 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.butDec = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.but0 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.butNeg = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.tbDisplay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.butBack = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.butOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.butC = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // but1
            // 
            this.but1.Location = new System.Drawing.Point(3, 30);
            this.but1.Name = "but1";
            this.but1.Size = new System.Drawing.Size(36, 25);
            this.but1.TabIndex = 1;
            this.but1.Values.Text = "1";
            this.but1.Click += new System.EventHandler(this.button_Click);
            // 
            // but2
            // 
            this.but2.Location = new System.Drawing.Point(45, 30);
            this.but2.Name = "but2";
            this.but2.Size = new System.Drawing.Size(36, 25);
            this.but2.TabIndex = 2;
            this.but2.Values.Text = "2";
            this.but2.Click += new System.EventHandler(this.button_Click);
            // 
            // but3
            // 
            this.but3.Location = new System.Drawing.Point(87, 30);
            this.but3.Name = "but3";
            this.but3.Size = new System.Drawing.Size(36, 25);
            this.but3.TabIndex = 3;
            this.but3.Values.Text = "3";
            this.but3.Click += new System.EventHandler(this.button_Click);
            // 
            // but6
            // 
            this.but6.Location = new System.Drawing.Point(87, 61);
            this.but6.Name = "but6";
            this.but6.Size = new System.Drawing.Size(36, 25);
            this.but6.TabIndex = 6;
            this.but6.Values.Text = "6";
            this.but6.Click += new System.EventHandler(this.button_Click);
            // 
            // but5
            // 
            this.but5.Location = new System.Drawing.Point(45, 61);
            this.but5.Name = "but5";
            this.but5.Size = new System.Drawing.Size(36, 25);
            this.but5.TabIndex = 5;
            this.but5.Values.Text = "5";
            this.but5.Click += new System.EventHandler(this.button_Click);
            // 
            // but4
            // 
            this.but4.Location = new System.Drawing.Point(3, 61);
            this.but4.Name = "but4";
            this.but4.Size = new System.Drawing.Size(36, 25);
            this.but4.TabIndex = 4;
            this.but4.Values.Text = "4";
            this.but4.Click += new System.EventHandler(this.button_Click);
            // 
            // but9
            // 
            this.but9.Location = new System.Drawing.Point(87, 92);
            this.but9.Name = "but9";
            this.but9.Size = new System.Drawing.Size(36, 25);
            this.but9.TabIndex = 9;
            this.but9.Values.Text = "9";
            this.but9.Click += new System.EventHandler(this.button_Click);
            // 
            // but8
            // 
            this.but8.Location = new System.Drawing.Point(45, 92);
            this.but8.Name = "but8";
            this.but8.Size = new System.Drawing.Size(36, 25);
            this.but8.TabIndex = 8;
            this.but8.Values.Text = "8";
            this.but8.Click += new System.EventHandler(this.button_Click);
            // 
            // but7
            // 
            this.but7.Location = new System.Drawing.Point(3, 92);
            this.but7.Name = "but7";
            this.but7.Size = new System.Drawing.Size(36, 25);
            this.but7.TabIndex = 7;
            this.but7.Values.Text = "7";
            this.but7.Click += new System.EventHandler(this.button_Click);
            // 
            // butDec
            // 
            this.butDec.Location = new System.Drawing.Point(87, 123);
            this.butDec.Name = "butDec";
            this.butDec.Size = new System.Drawing.Size(36, 25);
            this.butDec.TabIndex = 12;
            this.butDec.Values.Text = ".";
            this.butDec.Click += new System.EventHandler(this.button_Click);
            // 
            // but0
            // 
            this.but0.Location = new System.Drawing.Point(45, 123);
            this.but0.Name = "but0";
            this.but0.Size = new System.Drawing.Size(36, 25);
            this.but0.TabIndex = 11;
            this.but0.Values.Text = "0";
            this.but0.Click += new System.EventHandler(this.button_Click);
            // 
            // butNeg
            // 
            this.butNeg.Location = new System.Drawing.Point(3, 123);
            this.butNeg.Name = "butNeg";
            this.butNeg.Size = new System.Drawing.Size(36, 25);
            this.butNeg.TabIndex = 10;
            this.butNeg.Values.Text = "-";
            this.butNeg.Click += new System.EventHandler(this.button_Click);
            // 
            // tbDisplay
            // 
            this.tbDisplay.Location = new System.Drawing.Point(3, 3);
            this.tbDisplay.Name = "tbDisplay";
            this.tbDisplay.ReadOnly = true;
            this.tbDisplay.Size = new System.Drawing.Size(120, 23);
            this.tbDisplay.TabIndex = 13;
            this.tbDisplay.TextChanged += new System.EventHandler(this.tbDisplay_TextChanged);
            this.tbDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.tbDisplay_Paint);
            // 
            // butBack
            // 
            this.butBack.Location = new System.Drawing.Point(87, 154);
            this.butBack.Name = "butBack";
            this.butBack.Size = new System.Drawing.Size(36, 25);
            this.butBack.TabIndex = 16;
            this.butBack.Values.Text = "<--";
            this.butBack.Click += new System.EventHandler(this.button_Click);
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(45, 154);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(36, 25);
            this.butOK.TabIndex = 15;
            this.butOK.Values.Text = "OK";
            this.butOK.Click += new System.EventHandler(this.button_Click);
            // 
            // butC
            // 
            this.butC.Location = new System.Drawing.Point(3, 154);
            this.butC.Name = "butC";
            this.butC.Size = new System.Drawing.Size(36, 25);
            this.butC.TabIndex = 14;
            this.butC.Values.Text = "C";
            this.butC.Click += new System.EventHandler(this.button_Click);
            // 
            // KeyPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.butBack);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.butC);
            this.Controls.Add(this.tbDisplay);
            this.Controls.Add(this.butDec);
            this.Controls.Add(this.but0);
            this.Controls.Add(this.butNeg);
            this.Controls.Add(this.but9);
            this.Controls.Add(this.but8);
            this.Controls.Add(this.but7);
            this.Controls.Add(this.but6);
            this.Controls.Add(this.but5);
            this.Controls.Add(this.but4);
            this.Controls.Add(this.but3);
            this.Controls.Add(this.but2);
            this.Controls.Add(this.but1);
            this.Name = "KeyPad";
            this.Size = new System.Drawing.Size(126, 182);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KryptonButton but1;
        private KryptonButton but2;
        private KryptonButton but3;
        private KryptonButton but6;
        private KryptonButton but5;
        private KryptonButton but4;
        private KryptonButton but9;
        private KryptonButton but8;
        private KryptonButton but7;
        private KryptonButton butDec;
        private KryptonButton but0;
        private KryptonButton butNeg;
        private KryptonTextBox tbDisplay;
        private KryptonButton butBack;
        private KryptonButton butOK;
        private KryptonButton butC;
    }
}
