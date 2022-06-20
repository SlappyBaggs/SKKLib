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
    public partial class SKKCheckBox : UserControl
    {
        private bool init = false;
        public SKKCheckBox()
        {
            init = true;
            InitializeComponent();
            init = false;

            panelCheck.BorderStyle = BorderStyle.FixedSingle;
            UpdateLayout();
        }

        private Color colorChecked_ = Color.FromName(Data.Defaults.defColorChecked);
        [Category("SKKCheckBox")]
        [DefaultValue(typeof(Color), Data.Defaults.defColorChecked)]
        public Color ColorChecked
        {
            get => colorChecked_;
            set
            {
                if (colorChecked_ == value) return;
                colorChecked_ = value;
                UpdateLayout();
            }
        }
        
        private Color colorUnchecked_ = Color.FromName(Data.Defaults.defColorUnchecked);
        [Category("SKKCheckBox")]
        [DefaultValue(typeof(Color), Data.Defaults.defColorUnchecked)]
        public Color ColorUnchecked
        {
            get => colorUnchecked_;
            set
            {
                if (colorUnchecked_ == value) return;
                colorUnchecked_ = value;
                UpdateLayout();
            }
        }

        private bool checked_;
        [Category("SKKCheckBox")]
        [DefaultValue(false)]
        public bool Checked
        {
            get => checked_;
            set
            {
                checked_ = value;
                UpdateLayout();
            }
        }

        private Font labelFont_ = Data.Defaults.defFont;
        [Category("SKKCheckBox")]
        [DefaultValue(typeof(Font), Data.Defaults.defFontString)]
        public Font LabelFont
        {
            get => labelFont_;
            set
            {
                if (labelFont_ == value) return;
                labelFont_ = label1.Font = value;
                UpdateLayout();
            }
        }

        private string labelText_ = Data.Defaults.defLabelText;
        [Category("SKKCheckBox")]
        [DefaultValue(Data.Defaults.defLabelText)]
        public string LabelText
        {
            get => labelText_;
            set
            {
                if (labelText_ == label1.Text) return;
                labelText_ = label1.Text = value;
                UpdateLayout();
            }
        }


        private int checkSize_ = Data.Defaults.defCheckSize;
        [Category("SKKCheckBox")]
        [DefaultValue(Data.Defaults.defCheckSize)]
        public int CheckSize
        {
            get => checkSize_;
            set
            {
                if (checkSize_ == value) return;
                checkSize_ = value;
                UpdateLayout();
            }
        }

        private int checkPad_ = Data.Defaults.defCheckPad;
        [Category("SKKCheckBox")]
        [DefaultValue(Data.Defaults.defCheckPad)]
        public int CheckPad
        {
            get => checkPad_;
            set
            {
                if (checkPad_ == value) return;
                checkPad_ = value;
                UpdateLayout();
            }
        }

        private void UpdateLayout()
        {
            if (init) return;
            panelCheck.BackColor = Checked ? ColorChecked : ColorUnchecked;
            
            panelCheck.Size = new Size(CheckSize, CheckSize);
            panelCheck.Location = new Point(checkPad_, checkPad_);

            label1.Size = TextRenderer.MeasureText(Text, LabelFont);
            label1.Location = new Point(panelCheck.Right + checkPad_, checkPad_);

            Size = new Size(checkPad_ * 3 + panelCheck.Width + label1.Width, System.Math.Max(panelCheck.Height, label1.Height) + 2 * checkPad_);
        }

        private void panelCheck_Click(object sender, EventArgs e) => CheckBox_ClickedEvent(this, Data.CheckBoxEventArgs.GetArgs(Checked));

        [Category("SKKCheckBox")]
        public event EventHandler<Data.CheckBoxEventArgs> CheckBox_ClickedEvent = delegate { };
    }
}
