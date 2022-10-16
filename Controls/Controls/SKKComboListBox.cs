using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKKLib.Controls.Controls
{
    public partial class SKKComboListBox : UserControl
    {
        public SKKComboListBox()
        {
            InitializeComponent();

            listView.Visible = false;
            Width = tb1.Width + but1.Width;
            Height = 23 + (DroppedDown ? listView.Height : 0);
        }

        private bool DroppedDown { get; set; } = false;

        private void Refresh()
        {
            listView.Visible = DroppedDown;
            Height = 23 + (DroppedDown ? listView.Height : 0);
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            DroppedDown = !DroppedDown;
            Refresh();
        }
    }
}
