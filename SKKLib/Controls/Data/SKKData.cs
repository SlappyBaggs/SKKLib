using System;
using System.Drawing;

namespace SKKLib.Controls.Data
{
    public static class Defaults
    {
        public const string defColorChecked = "Lime";
        public const string defColorUnchecked = "Red";
        public const string defLabelText = "LabelText";
        public const int defCheckSize = 10;
        public const bool defChecked = false;
        public const int defCheckPad = 3;

        public static readonly Font defFont = new Font(FontName, FontSize);
        public const string defFontString = "Microsoft Sans Serif, 8.25pt";
        public const string FontName = "Microsoft Sans Serif";
        public const float FontSize = 8.25f;
    }

    public class CheckBoxEventArgs : EventArgs
    {
        public static CheckBoxEventArgs GetArgs(bool b) => new CheckBoxEventArgs(b);
        private CheckBoxEventArgs(bool @checked) => Checked = @checked;
        public bool Checked { get; private set; }
    }

    public class SKKCBOption<T>
    {
        public SKKCBOption(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public T Value { get; private set; }
    }
}
