using System;
using System.Windows.Forms;
using SKKLib.Controls.Data;
using ComponentFactory.Krypton.Toolkit;

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

        private bool AllChanged { get; set; } = false;
        private bool RegChanged { get; set; } = false;

        private void numAll_ValueChanged(object sender, EventArgs e)
        {
            if (RegChanged) return;

            AllChanged = true;
            numLeft.Value = numAll.Value;
            numTop.Value = numAll.Value;
            numRight.Value = numAll.Value;
            numBottom.Value = numAll.Value;
            AllChanged= false;
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            if (AllChanged) return;
        
            RegChanged= true;
            numAll.Value = ((numLeft.Value == numTop.Value) &&
                (numLeft.Value == numRight.Value) &&
                (numLeft.Value == numBottom.Value)) ?
                numAll.Value = numLeft.Value : -1;
            RegChanged= false;
        }
    }
}
