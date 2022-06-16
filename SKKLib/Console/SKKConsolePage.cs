using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Console
{
    public partial class SKKConsolePage : UserControl
    {
        public SKKConsolePage() : this("NoName")
        {
        }

        public SKKConsolePage(string name)
        {
            InitializeComponent();
            Name = name;
        }

        internal Color oldColor_ = Color.Empty;

        //internal KryptonRichTextBox RTB { get => tbRich; }

        private void butClear_Click(object sender, EventArgs e)
        {
            tbRich.Clear();
        }

        private void tbRich_TextChanged(object sender, EventArgs e)
        {
            butClear.Enabled = (tbRich.Text == String.Empty) ? ButtonEnabled.False : ButtonEnabled.True;
            int lines = tbRich.Lines.Count();
            string c = tbRich.SelectionColor.ToString();
            string f = tbRich.SelectionFont.ToString();
            groupPage.ValuesSecondary.Heading = $"{lines} lines / {c} / {f}";

            tbRich.ScrollToCaret();
        }

        private void tbRich_SystemColorsChanged(object sender, EventArgs e)
        {
            MessageBox.Show($"Colors changed... was {oldColor_} now {tbRich.SelectionColor}", "Color Changed");
        }
    }
}
