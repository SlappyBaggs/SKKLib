using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using SKKLib.Serial.Interface;
using SKKLib.Serial.Controls;

namespace SKKLib.Serial.Data
{
    public class SerialSelectBoxDesigner : ControlDesigner
    {
        // This Boolean state reflects whether the mouse is over the control.
        private bool sizeDisplay_ = false;
        public bool SizeDisplay
        {
            get => sizeDisplay_;
            set
            {
                if (sizeDisplay_ == value) return;
                sizeDisplay_ = value;
                Control.Refresh();
            }
        }

        private Color paddingColor_ = Color.Cyan;
        public Color PaddingColor
        {
            get => paddingColor_;
            set
            {
                if(paddingColor_ == value) return;
                paddingColor_ = value;
                Control.Refresh();
            }
        }

        private Color labelPaddingColor_ = Color.Blue;
        public Color LabelPaddingColor
        {
            get => labelPaddingColor_;
            set
            {
                if(labelPaddingColor_ == value) return;
                labelPaddingColor_ = value;
                Control.Refresh();
            }
        }

        private Color cbPaddingColor_ = Color.Red;
        public Color CBPaddingColor
        {
            get => cbPaddingColor_;
            set
            {
                if (cbPaddingColor_ == value) return;
                cbPaddingColor_ = value;
                Control.Refresh();
            }
        }


        private Color labCBGapColor_ = Color.Green;
        public Color LabCBGapColor
        {
            get => labCBGapColor_;
            set
            {
                if (labCBGapColor_ == value) return;
                labCBGapColor_ = value;
                Control.Refresh();
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            if (SizeDisplay)
            {
                SKKSerialSelectBox box = Control as SKKSerialSelectBox;
                Brush paddingBrush = new SolidBrush(PaddingColor);
                pe.Graphics.FillRectangle(paddingBrush, 0, 0, box.Width, box.Padding.Top);
                pe.Graphics.FillRectangle(paddingBrush, 0, box.Height - box.Padding.Bottom, box.Width, box.Padding.Bottom);
                pe.Graphics.FillRectangle(paddingBrush, 0, box.Padding.Top, box.Padding.Left, box.Height - box.Padding.Top - box.Padding.Bottom);
                pe.Graphics.FillRectangle(paddingBrush, box.Width - box.Padding.Right, box.Padding.Top, box.Padding.Left, box.Height - box.Padding.Top - box.Padding.Bottom);


                Brush labBrush = new SolidBrush(LabelPaddingColor);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X - box.LabelPadding.Left,
                    box.LABL.Y - box.LabelPadding.Top,
                    box.LABS.Width + box.LabelPadding.Left + box.LabelPadding.Right,
                    box.LabelPadding.Top);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X - box.LabelPadding.Left,
                    box.LABL.Y + box.LABS.Height,
                    box.LABS.Width + box.LabelPadding.Left + box.LabelPadding.Right,
                    box.LabelPadding.Bottom);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X - box.LabelPadding.Left,
                    box.LABL.Y,
                    box.LabelPadding.Left,
                    box.LABS.Height);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X + box.LABS.Width,
                    box.LABL.Y,
                    box.LabelPadding.Right,
                    box.LABS.Height);

                Brush cbBrush = new SolidBrush(CBPaddingColor);
                pe.Graphics.FillRectangle(cbBrush,
                    box.CBL.X - box.CBPadding.Left,
                    box.CBL.Y - box.CBPadding.Top,
                    box.CBS.Width + box.CBPadding.Left + box.CBPadding.Right,
                    box.CBPadding.Top);
                pe.Graphics.FillRectangle(cbBrush,
                    box.CBL.X - box.CBPadding.Left,
                    box.CBL.Y + box.CBS.Height,
                    box.CBS.Width + box.CBPadding.Left + box.CBPadding.Right,
                    box.CBPadding.Bottom);
                pe.Graphics.FillRectangle(cbBrush,
                    box.CBL.X - box.CBPadding.Left,
                    box.CBL.Y,
                    box.CBPadding.Left,
                    box.CBS.Height);
                pe.Graphics.FillRectangle(cbBrush,
                    box.CBL.X + box.CBS.Width,
                    box.CBL.Y,
                    box.CBPadding.Right,
                    box.CBS.Height);

/*
                Brush txtBrush = new SolidBrush(Color.Black);
                pe.Graphics.DrawString($"{box.Padding.Left} + {box.LabelPadding.Left} + {box.LABS.Width} + {box.LabelPadding.Right} + {box.LabCBGap} + {box.CBPadding.Left} = "+
                    $"{box.Padding.Left + box.LabelPadding.Left + box.LABS.Width + box.LabelPadding.Right + box.LabCBGap + box.CBPadding.Left}",
                    box.Font, txtBrush, 0, 0);
*/
/*
                Brush gapBrush = new SolidBrush(LabCBGapColor);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X - box.LabelPadding.Left,
                    box.LABL.Y - box.LabelPadding.Top,
                    box.LABS.Width + box.LabelPadding.Left + box.LabelPadding.Right,
                    box.LabelPadding.Top);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X - box.LabelPadding.Left,
                    box.LABL.Y + box.LABS.Height,
                    box.LABS.Width + box.LabelPadding.Left + box.LabelPadding.Right,
                    box.LabelPadding.Bottom);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X - box.LabelPadding.Left,
                    box.LABL.Y,
                    box.LabelPadding.Left,
                    box.LABS.Height);
                pe.Graphics.FillRectangle(labBrush,
                    box.LABL.X + box.LABS.Width,
                    box.LABL.Y,
                    box.LabelPadding.Right,
                    box.LABS.Height);
*/
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            PropertyDescriptor display = TypeDescriptor.CreateProperty(typeof(SerialSelectBoxDesigner), "SizeDisplay", typeof(bool), new Attribute[] { new DesignOnlyAttribute(true) });
            PropertyDescriptor pd = TypeDescriptor.CreateProperty(typeof(SerialSelectBoxDesigner), "PaddingColor", typeof(Color), new Attribute[] { new DesignOnlyAttribute(true) });
            PropertyDescriptor pdLab = TypeDescriptor.CreateProperty(typeof(SerialSelectBoxDesigner), "LabelPaddingColor", typeof(Color), new Attribute[] { new DesignOnlyAttribute(true) });
            PropertyDescriptor pdCB = TypeDescriptor.CreateProperty(typeof(SerialSelectBoxDesigner), "CBPaddingColor", typeof(Color), new Attribute[] { new DesignOnlyAttribute(true) });
            PropertyDescriptor pdGap = TypeDescriptor.CreateProperty(typeof(SerialSelectBoxDesigner), "LabCBGapColor", typeof(Color), new Attribute[] { new DesignOnlyAttribute(true) });

            properties.Add("SizeDisplay", display);
            properties.Add("PaddingColor", pd);
            properties.Add("LabelPaddingColor", pdLab);
            properties.Add("CBPaddingColor", pdCB);
            properties.Add("LabCBGapColor", pdGap);
        }
    }
}
