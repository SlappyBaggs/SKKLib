using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKKLib.Controls.Forms
{
    public partial class InputBox : KryptonForm
    {
        public static Form MyApp { private get; set; }

        public static string ShowInput(string title, string existingText = "Enter your input here", bool multi = false)
        {
            InputBox ib = new InputBox(title, existingText, multi);
            return (ib.ShowDialog() == DialogResult.OK) ? ib.tbInput.Text : null;
        }

        private InputBox(string title, string exText, bool multi)
        {
            InitializeComponent();
            Icon = MyApp.Icon;
            Text = title;
            tbInput.Multiline = multi;
            tbInput.Text = ((exText == null) || (exText == "")) ? "Enter your input here" : exText;
            tbInput.SelectAll();
            DialogResult = DialogResult.Cancel;
        }

        private void InputBox_Shown(object sender, EventArgs e) => tbInput.Focus();
        private void butOK_Click(object sender, EventArgs e) => CloseMe(DialogResult.OK);
        private void butCancel_Click(object sender, EventArgs e) => CloseMe(DialogResult.Cancel);
        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!tbInput.Multiline) && ((e.KeyChar == (char)Keys.Enter) || (e.KeyChar == (char)Keys.Return)))
                CloseMe(DialogResult.OK);
            else if (e.KeyChar == (char)Keys.Escape)
                CloseMe(DialogResult.Cancel);
        }

        private void tbInput_MultilineChanged(object sender, EventArgs e)
        {
            Dock = tbInput.Multiline ? DockStyle.Fill : DockStyle.None;
            FormBorderStyle = tbInput.Multiline ? FormBorderStyle.Sizable : FormBorderStyle.Fixed3D;
            if (!tbInput.Multiline) Size = new Size(500, 100);
        }

        private void CloseMe(DialogResult dr)
        {
            DialogResult = dr;
            Close();
        }

    }
}
