using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;

namespace SKKLib.Serial.Data
{
    #region EVENT DELEGATES
    public delegate void SKKSerialSelectBoxSetPort_EH(Interface.ISKKSerialPort port);
    public delegate void SKKSerialSelectBoxPortNameChanged_EH(string name);
    #endregion

    public enum LabelPosition
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3,
        Hidden = 4
    }

    public static class SerialSelectBoxDefaults
    {
        public static Padding Padding = new Padding(0, 0, 0, 0);
        public const string DefPadding = "0, 0, 0, 0";
        public const LabelPosition LabelPos = LabelPosition.Left;
        public const int LabCBGap = 5;

        public const string LabText = "ComPort";
        public const int LabelExtraWidth = 0;
        public const int LabelExtraHeight = 0;
        public const int LabControlWidth = 0;
        public const int LabControlHeight = 0;
        public static Padding LabPadding = new Padding(0, 0, 0, 0);
        public const string DefLabPadding = "0, 0, 0, 0";
        //public static readonly Point LabLoc = new Point(0, 4);

        public const int ComboExtraWidth = 0;
        public const int ComboExtraHeight = 0;
        public const int CBControlWidth = 25;
        public const int CBControlHeight = 8; //16;
        public static readonly Padding CBPadding = new Padding(0, 0, 0, 0);
        public const string DefCBPadding = "0, 0, 0, 0";
        //public static readonly Point CBLoc = new Point(3, 3);

        public static readonly Font defFont = new Font(FontName, FontSize);
        public const string FontString = "Microsoft Sans Serif, 8.25pt";
        public const string FontName = "Microsoft Sans Serif";
        public const float FontSize = 8.25f;

        public const Interface.ISKKSerialPort SerialPortC = null;
    }
}
