using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ComponentFactory.Krypton.Toolkit;
using SKKLib.Controls.Data;

namespace SKKLib.Controls.Forms
{
    public partial class SKKPaddingEdit : KryptonForm
    {
        public static Form MyApp { private get; set; }

        public static SKKPadding EditPadding(SKKPadding padding)
        {
            SKKPaddingEdit padEdit = new SKKPaddingEdit(padding);
            return (padEdit.ShowDialog() == DialogResult.OK) ? padEdit.myPad : padEdit.origPad;
        }

        public SKKPaddingEdit(SKKPadding pad)
        {
            InitializeComponent();
            
            Icon = MyApp.Icon;
            
            myPad = pad;
            origPad = pad;

            numLeft.Value = pad.Left;
            numRight.Value = pad.Right;
            numTop.Value = pad.Top;
            numBottom.Value = pad.Bottom;
            numAll.Value = pad.All;
        }

        public SKKPadding myPad;
        public SKKPadding origPad;

        private void butOK_Click(object sender, EventArgs e)
        {
            myPad.Left = (int)numLeft.Value;
            myPad.Right = (int)numRight.Value;
            myPad.Top= (int)numTop.Value;
            myPad.Bottom= (int)numBottom.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
