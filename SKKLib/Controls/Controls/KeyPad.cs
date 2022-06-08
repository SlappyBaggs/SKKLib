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
    public partial class KeyPad : UserControl
    {
        public KeyPad() : this(true, true, true) { }

        public KeyPad(bool showDisplay, bool showControlKeys, bool showOKKey)
        {
            InitializeComponent();

            ShowDisplay = showDisplay;
            ShowControlKeys = showControlKeys;
            ShowOKKey = showOKKey;

            UpdateAppearance();
        }

        #region KEYPAD ENUMS
        public enum KeyPadButton
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Negative,
            Decimal,
            Back,
            OK,
            Clear

        }
        #endregion

        #region EVENT DELEGATES
        public delegate void KeyPadButtonPressedHandler(KeyPadButton but);
        public delegate void KeyPadShowDisplayHandler(bool show);
        public delegate void KeyPadShowControlKeysHandler(bool show);
        public delegate void KeyPadShowOKKeyHandler(bool show);
        #endregion

        #region EVENTS
        public event KeyPadButtonPressedHandler KeyPadButtonPressed;
        public event KeyPadShowDisplayHandler KeyPadShowDisplay;
        public event KeyPadShowControlKeysHandler KeyPadShowControlKeys;
        public event KeyPadShowOKKeyHandler KeyPadShowOKKey;
        #endregion

        #region CONTROL PROPERTIES
        private bool showDisplay_ = true;
        [Category("KeyPad Options")]
        [DisplayName("Show Display")]
        [DefaultValue(true)]
        public bool ShowDisplay
        {
            get => showDisplay_;
            set
            {
                if (showDisplay_ == value) return;
                showDisplay_ = value;
                if (KeyPadShowDisplay != null)
                    KeyPadShowDisplay(showDisplay_);
                UpdateAppearance();
            }
        }

        private bool showControlKeys_ = true;
        [Category("KeyPad Options")]
        [DisplayName("Show Control Keys")]
        [DefaultValue(true)]
        public bool ShowControlKeys
        {
            get => showControlKeys_;
            set
            {
                if (showControlKeys_ == value) return;
                showControlKeys_ = value;
                if (KeyPadShowControlKeys != null)
                    KeyPadShowControlKeys(showControlKeys_);
                UpdateAppearance();
            }
        }

        private bool showOKKey_ = true;
        [Category("KeyPad Options")]
        [DisplayName("Show OK Key")]
        [DefaultValue(true)]
        public bool ShowOKKey
        {
            get => showOKKey_;
            set
            {
                if (showOKKey_ == value) return;
                showOKKey_ = value;
                if (KeyPadShowOKKey != null)
                    KeyPadShowOKKey(showOKKey_);
                UpdateAppearance();
            }
        }
        public string Output { get; set; }
        #endregion

        #region CONTROL FUNCTIONS
        private void UpdateAppearance()
        {
            tbDisplay.Visible = ShowDisplay;
            butC.Visible = ShowControlKeys;
            butOK.Visible = ShowControlKeys && ShowOKKey;
            butBack.Visible = ShowControlKeys;

            butC.Enabled = butBack.Enabled = tbDisplay.Text != "";
            butDec.Enabled = !tbDisplay.Text.Contains(".");
            butNeg.Enabled = butC.Enabled && !tbDisplay.Text.StartsWith("-");

            int th = tbDisplay.Height;

            int y = (ShowDisplay ? /*tbDisplay.Height*/20 + 6 : 0);
            Height = y + (ShowControlKeys ? 5 : 4) * (but1.Height + 6);
            
            but1.Location = new Point(3, y);
            but2.Location = new Point(45, y);
            but3.Location = new Point(87, y);
            y += but1.Height + 6;
            but4.Location = new Point(3, y);
            but5.Location = new Point(45, y);
            but6.Location = new Point(87, y);
            y += but1.Height + 6;
            but7.Location = new Point(3, y);
            but8.Location = new Point(45, y);
            but9.Location = new Point(87, y);
            y += but1.Height + 6;
            butNeg.Location = new Point(3, y);
            but0.Location = new Point(45, y);
            butDec.Location = new Point(87, y);
            y += but1.Height + 6;
            butC.Location = new Point(3, y);
            butOK.Location = new Point(45, y);
            butBack.Location = new Point(87, y);
        }
        #endregion

        #region EVENT HANDLERS
        private void tbDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void tbDisplay_Paint(object sender, PaintEventArgs e)
        {
            UpdateAppearance();
        }
    }
}
