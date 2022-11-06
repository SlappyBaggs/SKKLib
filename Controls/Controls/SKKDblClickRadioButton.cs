using System.ComponentModel;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace SKKLib.Controls.Controls
{
    public class SKKDblClickRadioButton : KryptonRadioButton
    {
        public SKKDblClickRadioButton()
        {
            this.SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        public new event MouseEventHandler MouseDoubleClick = delegate { };

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            MouseDoubleClick(this, e);
        }
    }
}
