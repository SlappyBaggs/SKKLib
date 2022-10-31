using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Forms
{
    public partial class QuestionBox : KryptonForm
    {
        private static Form app_ = null;

        public static void SetApp(Form app) => app_ = app;

        private QuestionBox(string msg1, string msg2)
        {
            InitializeComponent();
            Icon = app_?.Icon;
            tbMessage.Text = msg1;
            if (msg2 != String.Empty)
                Text = msg2;

            tbMessage.SelectionLength = 0;
            tbMessage.SelectionStart = tbMessage.Text.Length;
        }

        public static bool AskQuestion(string msg1, string msg2 = "") => new QuestionBox(msg1, msg2).AskQuestion() == DialogResult.OK;
        
        private DialogResult AskQuestion() => ShowDialog();

        private void butYES_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void butNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void STECMessageBox_Load(object sender, EventArgs e) => Location = new Point(app_.Location.X + (app_.Width - Width) / 2, app_.Location.Y + (app_.Height - Height) / 2);
    }
}
