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
        public SKKPaddingEdit()
        {
            InitializeComponent();
        }

        public SKKPadding myPad;

        public void ShowPadding(SKKPadding pad)
        {
            myPad = pad;
        }
    }
}
