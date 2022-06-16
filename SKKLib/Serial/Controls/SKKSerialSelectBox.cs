using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;
using ComponentFactory.Krypton.Toolkit;
using SKKLib.Serial.Data;

namespace SKKLib.Serial.Controls
{
    [DesignerAttribute(typeof(SerialSelectBoxDesigner))]
    public partial class SKKSerialSelectBox : UserControl
    {
        public SKKSerialSelectBox()
        {
            InitializeComponent();
            cbPortName.DataSource = SerialPortDefaults.PortNames;
            Font = SerialSelectBoxDefaults.defFont;
        }

        #region PROPERTIES
        #region RUN-TIME PROPERTIES
        private Control lastParent_ = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private Control LastParent
        {
            get => lastParent_;
            set => lastParent_ = value;
        }

        #region APPEARANCE PROPERTIES

        #region COMMON
        private Padding padding_ = SerialSelectBoxDefaults.Padding;
        [Category("SelectBox Appearance")]
        [DisplayName("Padding")]
        [DefaultValue(typeof(Padding), SerialSelectBoxDefaults.DefPadding)]
        public new Padding Padding
        {
            get => padding_;
            set
            {
                if (padding_ != value)
                {
                    padding_ = value;
                    PositionLabel();
                }
            }
        }

        private int gap_ = SerialSelectBoxDefaults.LabCBGap;
        [Category("SelectBox Appearance")]
        [DisplayName("Label ComboBox Gap")]
        [Display(Order = 1)]
        [DefaultValue(SerialSelectBoxDefaults.LabCBGap)]
        public int LabCBGap
        {
            get => gap_;
            set
            {
                if (gap_ != value)
                {
                    gap_ = value;
                    PositionLabel();
                }
            }
        }
        
        private LabelPosition labelPos_ = SerialSelectBoxDefaults.LabelPos;
        [Category("SelectBox Appearance")]
        [DisplayName("Label Position")]
        [Display(Order = 6)]
        [DefaultValue(SerialSelectBoxDefaults.LabelPos)]
        public LabelPosition LabelPosition
        {
            get => labelPos_;
            set
            {
                if (labelPos_ != value)
                {
                    labelPos_ = value;
                    PositionLabel();
                }
            }
        }
        #endregion // common

        #region LABEL
        private string labText_ = SerialSelectBoxDefaults.LabText;
        [Category("SelectBox Appearance - Label")]
        [DisplayName("Label Text")]
        [Display(Order = 1)]
        [DefaultValue(SerialSelectBoxDefaults.LabText)]
        public string LabText
        {
            get => labText_;
            set
            {
                if (labText_ != value)
                {
                    labText_ = value;
                    labPortName.Text = labText_;
                    PositionLabel();
                }
            }
        }

        private Padding labPadding_ = SerialSelectBoxDefaults.LabPadding;
        [Category("SelectBox Appearance - Label")]
        [DisplayName("Label Padding")]
        [DefaultValue(typeof(Padding), SerialSelectBoxDefaults.DefLabPadding)]
        public Padding LabelPadding
        {
            get => labPadding_;
            set
            {
                if (labPadding_ != value)
                {
                    labPadding_ = value;
                    PositionLabel();
                }
            }
        }

        [Category("SelectBox Appearance - Label")]
        [DisplayName("Label Font")]
        [DefaultValue(null)]
        public Font LabelFont
        {
            get => labPortName.StateCommon.ShortText.Font;
            set
            {
                labPortName.Font = value;
                PositionLabel();
                labPortName.Invalidate();
            }
        }
        #endregion // label

        #region COMBOBOX
        private Padding cbPadding_ = SerialSelectBoxDefaults.CBPadding;
        [Category("SelectBox Appearance - ComboBox")]
        [DisplayName("ComboBox Padding")]
        [Display(Order = 4)]
        [DefaultValue(typeof(Padding), SerialSelectBoxDefaults.DefCBPadding)]
        public Padding CBPadding
        {
            get => cbPadding_;
            set
            {
                if (cbPadding_ != value)
                {
                    cbPadding_ = value;
                    PositionLabel();
                }
            }
        }

        [Category("SelectBox Appearance - ComboBox")]
        [DisplayName("ComboBox Font")]
        [DefaultValue(null)]
        public Font ComboFont
        {
            get => cbPortName.StateCommon.ComboBox.Content.Font;
            set
            {
                cbPortName.StateCommon.ComboBox.Content.Font = value;
                PositionLabel();
                cbPortName.Invalidate();
            }
        }
        #endregion // combobox


        private bool ShouldSerializeFont() => !(Font.Equals(SerialSelectBoxDefaults.defFont));
        #endregion

        private Interface.ISKKSerialPort serialPort_ = SerialSelectBoxDefaults.SerialPortC;
        [Editor(typeof(SKKSerialPortCEditor), typeof(UITypeEditor))]
        [Category("SKKSerialSelectBox Options")]
        [DisplayName("SKKSerialPort")]
        [Display(Order = 1)]
        [DefaultValue(SerialSelectBoxDefaults.SerialPortC)]
        public Interface.ISKKSerialPort SerialPort
        {
            get => serialPort_;
            set
            {
                if (serialPort_ != value)
                {
                    // If we already had a port, remove our event hooks from it
                    if (serialPort_ != null) RemoveEventHooks(value);
                    
                    // Set our port to new value
                    serialPort_ = value;
                    
                    // If new port is not null, add our event hooks into it and set our combo box to new port's portname
                    if (serialPort_ != null)
                    {
                        cbPortName.SelectedItem = serialPort_.PortName;
                        AddEventHooks(value);
                    }

                    // Fire off port set event
                    OnSerialSelectBoxSetPort(serialPort_);
                }
            }
        }
        #endregion

        #region DESIGN-TIME PROPERTIES
        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("CB Size"), DesignOnly(true), XmlIgnore]
        public Size CBS { get => cbPortName.Size; }
        
        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("CB Loc"), DesignOnly(true), XmlIgnore]
        public Point CBL { get => cbPortName.Location; }

        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("CB Txt"), DesignOnly(true), XmlIgnore]
        public Size CBT { get; set; }

        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("Label Size"), DesignOnly(true), XmlIgnore]
        public Size LABS { get => labPortName.Size; }

        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("Label Loc"), DesignOnly(true), XmlIgnore]
        public Point LABL { get => labPortName.Location; }

        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("Lab Txt"), DesignOnly(true), XmlIgnore]
        public Size LABT { get; set; }

        [Category("SKKSerialSelectBox Design DEBUG")]
        [DisplayName("Size ALL"), DesignOnly(true), XmlIgnore]
        public Size TOTW { get => Size; }
        #endregion
        #endregion

        #region EVENTS & HANDLERS
        private void OnSerialPortOpenClose(OpenedClosed oc) => Invoke((Action)delegate { cbPortName.Enabled = oc == OpenedClosed.Closed; });

        public event PortAssigned_EH PortAssigned;
        private void OnSerialSelectBoxSetPort(Interface.ISKKSerialPort port) { if (PortAssigned != null) PortAssigned(port); }

        public event PortNameChanged_EH PortNameChanged;
        private void OnSerialSelectBoxPortNameChanged(string name)
        { 
            if (PortNameChanged != null) 
                PortNameChanged(name);

            // We should update our SelectedIndex here...
            if (cbPortName.Items.Contains(name))
                cbPortName.SelectedItem = name;

            PositionLabel();
        }

        private void cbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change actual port name
            if (SerialPort != null) SerialPort.PortName = cbPortName.SelectedItem.ToString();

            // Fire off our PortNameChanged event (after name change)
            // Skip this during Init????
            // Setting the new portname in the serial port should propagate up to ultimately call our PortNameChangedEvent, so don't manually call it...
            //OnSerialSelectBoxPortNameChanged(cbPortName.SelectedItem.ToString());
            
            // Update appearance
            // Update the appearance in the actual PortNameChanged event handler...
            //PositionLabel();
        }

        private void SKKSerialSelectBox_FontChanged(object sender, EventArgs e)
        {
            labPortName.Font = Font;
            cbPortName.Font = Font;
            PositionLabel();
        }
        private void SKKSerialSelectBox_ParentChanged(object sender, EventArgs e)
        {
            if (LastParent != null)
            {
                LastParent.BackColorChanged -= Parent_BackColorChanged;
                LastParent.ForeColorChanged -= Parent_ForeColorChanged;
            }

            LastParent = Parent;

            if (Parent != null)
            {
                Parent.BackColorChanged += Parent_BackColorChanged;
                Parent.ForeColorChanged += Parent_ForeColorChanged;
                DoBackColor();
                DoForeColor();
            }
        }

        private void Parent_BackColorChanged(object p, EventArgs e) => DoBackColor();

        private void Parent_ForeColorChanged(object p, EventArgs e) => DoForeColor();
        #endregion

        #region LOCAL FUNCS
        private void DoBackColor()
        {
            BackColor = Parent.BackColor;
            labPortName.BackColor = BackColor;
            cbPortName.BackColor = BackColor;
        }
        private void DoForeColor()
        {
            ForeColor = Parent.ForeColor;
            labPortName.ForeColor = ForeColor;
            cbPortName.ForeColor = ForeColor;
        }

        private void RemoveEventHooks(Interface.ISKKSerialPort port)
        {
            serialPort_.SKKPortOpenedClosedEvent -= OnSerialPortOpenClose;
            serialPort_.SKKPortPortNameChangedEvent -= OnSerialSelectBoxPortNameChanged;
        }

        private void AddEventHooks(Interface.ISKKSerialPort port)
        {
            serialPort_.SKKPortOpenedClosedEvent += OnSerialPortOpenClose;
            serialPort_.SKKPortPortNameChangedEvent += OnSerialSelectBoxPortNameChanged;
        }

        public override void ResetFont() => Font = null;

        private void PositionLabel()
        {
            /*
                The following values are stored in 'SKKSerialSelectBoxDefaults'
                They exist for proper sizing of the internal controls to account for control area beyond just text
                These values are NOT changeable in either design-time or run-time
                    LabControlWidth, LabControlHeight, CBControlWidth, CBControlHeight 
                
                The following values are properties of the 'SKKSerialSelectBox'
                Their default values are stored in 'SKKSerialSelectBoxDefaults'
                They exist to make adjustments to the appearance of the box as desired
                They are editable at design-time and run-time and will update the box's appearance immediately
                    LabelExtraWidth, LabelExtraHeight, ComboExtraWidth, ComboExtraHeight
             */


            // Calculate text size of 'labPortName'...
            Size labSize = TextRenderer.MeasureText(labPortName.Text, (LabelFont != null) ? LabelFont : Font);
            LABT = labSize;
            //labSize = labPortName.Size;
            labSize.Width += SerialSelectBoxDefaults.LabControlWidth + LabelPadding.Left + LabelPadding.Right;
            labSize.Height += SerialSelectBoxDefaults.LabControlHeight + LabelPadding.Top + LabelPadding.Bottom;

            // Calculate text size of 'cbPortName'...
            Size cbSize = TextRenderer.MeasureText(cbPortName.Text, (ComboFont != null) ? ComboFont : Font);
            CBT = cbSize;
            cbSize.Width += SerialSelectBoxDefaults.CBControlWidth + CBPadding.Left + CBPadding.Right;
            cbSize.Height += SerialSelectBoxDefaults.CBControlHeight + CBPadding.Top + CBPadding.Bottom;

            // Empty size data to fill in with size of 'this'...
            Size mySize = new Size(0, 0);
            
            // Empty point data to fill in with positions of 'labPortName' and 'cbPortName'...
            Point labLoc = new Point(0, 0);
            Point cbLoc = new Point(0, 0);

            // Switch 'labelPos_' to find locations of 'labPortName' and 'cbPortName'...
            switch (labelPos_)
            {
                case LabelPosition.Left:
                    labLoc.X = Padding.Left + LabelPadding.Left;
                    labLoc.Y = Padding.Top + LabelPadding.Top;
                    //cbLoc.X = Padding.Left + LabelPadding.Left + labSize.Width + LabelPadding.Right + LabCBGap + CBPadding.Left;
                    cbLoc.X = Padding.Left + labSize.Width + LabCBGap + CBPadding.Left;
                    cbLoc.Y = Padding.Top + CBPadding.Top;
                    break;
                case LabelPosition.Right:
                    //labLoc.X = Padding.Left + CBPadding.Left + cbSize.Width + CBPadding.Right + LabCBGap + LabelPadding.Left;
                    labLoc.X = Padding.Left + cbSize.Width +  LabCBGap + LabelPadding.Left;
                    labLoc.Y = Padding.Top + LabelPadding.Top;
                    cbLoc.X = Padding.Left + CBPadding.Left;
                    cbLoc.Y = Padding.Top + CBPadding.Top;
                    break;
                case LabelPosition.Top:
                    labLoc.X = Padding.Left + LabelPadding.Left;
                    labLoc.Y = Padding.Top + LabelPadding.Top;
                    cbLoc.X = Padding.Left + CBPadding.Left;
                    //Loc.Y = Padding.Top + LabelPadding.Top + labSize.Height + LabelPadding.Bottom + LabCBGap + CBPadding.Top;
                    cbLoc.Y = Padding.Top + labSize.Height + LabCBGap + CBPadding.Top;
                    break;
                case LabelPosition.Bottom:
                    labLoc.X = Padding.Left + LabelPadding.Left;
                    //labLoc.Y = Padding.Top + CBPadding.Top + cbSize.Height + CBPadding.Bottom + LabCBGap + LabelPadding.Top;
                    labLoc.Y = Padding.Top + cbSize.Height + LabCBGap + LabelPadding.Top;
                    cbLoc.X = Padding.Left + CBPadding.Left;
                    cbLoc.Y = Padding.Top + CBPadding.Top;
                    break;
                case LabelPosition.Hidden:
                    labLoc.X = 0;
                    labLoc.Y = 0;
                    cbLoc.X = Padding.Left + CBPadding.Left;
                    cbLoc.Y = Padding.Top + CBPadding.Top;
                    break;
            }

            // Switch 'labelPos_' to find size of 'this'...
            switch (labelPos_)
            {
                case LabelPosition.Left:
                case LabelPosition.Right:
                    //mySize.Width = Padding.Left + LabelPadding.Left + labSize.Width + LabelPadding.Right + LabCBGap + CBPadding.Left + cbSize.Width + CBPadding.Right + Padding.Right;
                    //mySize.Height = Padding.Top + Math.Max(LabelPadding.Top + labSize.Height + LabelPadding.Bottom, CBPadding.Top + cbSize.Height + CBPadding.Bottom) + Padding.Bottom;
                    mySize.Width = Padding.Left + labSize.Width + LabCBGap + cbSize.Width + Padding.Right;
                    mySize.Height = Padding.Top + System.Math.Max(labSize.Height, cbSize.Height) + Padding.Bottom;
                    break;
                case LabelPosition.Top:
                case LabelPosition.Bottom:
                    //mySize.Width = Padding.Left + Math.Max(LabelPadding.Left + labSize.Width + LabelPadding.Right, CBPadding.Left + cbSize.Width + CBPadding.Right) + Padding.Right;
                    //mySize.Height = Padding.Top + LabelPadding.Top + labSize.Height + LabelPadding.Bottom + LabCBGap + CBPadding.Top + cbSize.Height + CBPadding.Bottom + Padding.Bottom;
                    mySize.Width = Padding.Left + System.Math.Max(labSize.Width, cbSize.Width) + Padding.Right;
                    mySize.Height = Padding.Top + labSize.Height + LabCBGap + cbSize.Height + Padding.Bottom;
                    break;
                case LabelPosition.Hidden:
                    //mySize.Width = Padding.Left + CBPadding.Left + cbSize.Width + CBPadding.Right + Padding.Right;
                    //mySize.Height = Padding.Top + CBPadding.Top + cbSize.Height + CBPadding.Bottom + Padding.Bottom;
                    mySize.Width = Padding.Left + cbSize.Width + Padding.Right;
                    mySize.Height = Padding.Top + cbSize.Height + Padding.Bottom;
                    break;
            }

            // Show/Hide 'labPortName' based on 'labelPos_'...
            labPortName.Visible = labelPos_ != LabelPosition.Hidden;

            // Set Location and Size of 'labPortName' and 'cbPortName'...
            labPortName.Location = labLoc;
            labPortName.Size = labSize;
            cbPortName.Location = cbLoc;
            cbPortName.Size = cbSize;

            // Set size of 'this'...
            Size = mySize;
        }
        #endregion
    }
}    


