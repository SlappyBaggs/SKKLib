﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Forms
{
    public partial class MessageBox : KryptonForm
    {
        private static Form app_ = null;

        public static void SetApp(Form app) => app_ = app;

        private MessageBox(string msg1, string msg2)
        {
            InitializeComponent();

            Icon = app_?.Icon;

            tbMessage.Text = msg1;
            if (msg2 != String.Empty) Text = msg2;

            tbMessage.SelectionLength = 0;
            tbMessage.SelectionStart = tbMessage.Text.Length;

            szWDiff = Width - tbMessage.Width;
            szHDiff = Height - tbMessage.Height;

            gapTBBut = butOK.Location.Y - tbMessage.Bottom;
        }

        private int szWDiff;
        private int szHDiff;
        private int gapTBBut;

        public static void ShowMessage(string msg1, string msg2 = "") => new MessageBox(msg1, msg2).ShowDialog();
        
        private void SKKMessageBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar == (char)Keys.Enter) || (e.KeyChar == (char)Keys.Escape))
                Close();
        }

        private void butOK_Click(object sender, EventArgs e) => Close();
        
        private void SKKMessageBox_Load(object sender, EventArgs e) => Location = new Point(app_.Location.X + (app_.Width - Width) / 2, app_.Location.Y + (app_.Height - Height) / 2);

        private void MessageBox_SizeChanged(object sender, EventArgs e)
        {
            tbMessage.Size = new Size(Width - szWDiff, Height - szHDiff);
            butOK.Location = new Point(Width / 2 - butOK.Width / 2, tbMessage.Bottom + gapTBBut);
        }
    }
}
