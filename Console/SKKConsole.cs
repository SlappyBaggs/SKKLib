using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using SKKLib.Console.Config;
using SKKLib.Console.Data;
using SKKLib.Console.Controls;

namespace SKKLib.Console
{
    public partial class SKKConsole : Component
    {
        public void Break()
        {
            System.Diagnostics.Debugger.Break();
        }

        public SKKConsole()
        {
            InitializeComponent();
            DefaultPages = new ConsolePageConfigCollection(this);
        }

        public SKKConsole(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            DefaultPages = new ConsolePageConfigCollection(this);
        }

        private SKKConsoleForm consoleForm_ = null;

        public SKKConsoleForm CWindow { get => consoleForm_ ?? (consoleForm_ = new SKKConsoleForm(this)); }
        public bool Showing { get => CWindow.Visible; }
        public bool Hidden { get => !Showing; }
        public void ShowConsole() => CWindow.Show();
        public void HideConsole() => CWindow.Hide();
        public void ToggleConsole() { if (Showing) HideConsole(); else ShowConsole(); }

        public event ConsoleEvent ConsoleHidden = delegate { };
        public void OnConsoleHidden() => ConsoleHidden();

        public void Write(string s1, string s2) { CWindow.Write(s1, s2); }

        /*
         *  DefaultColors & DefaultFont hold their actual values in private static variables
         *  with public static getters so that they can be used by ConsolePageConfig without
         *  requiring to hand off an instance to the main console.
         * 
         */

        [Browsable(true)]
        [Category("Page Options")]
        [DisplayName("DefaultColors")]
        [Description("Colors to choose from for Pages that do not explicitly set one")]
        public List<Color> DefaultColors { get; set; } = Data.Defaults.DefaultColors;

        private int colorIndex_ = 0;
        [Browsable(false)]
        public Color NextColor
        {
            get
            {
                if (colorIndex_ >= DefaultColors.Count) colorIndex_ = 0;
                return DefaultColors[colorIndex_++];
            }
        }

        [Browsable(true)]
        [Category("Page Options")]
        [DisplayName("Default Font")]
        [Description("Font given to Pages that do not explicitly set one")]
        [DefaultValue(typeof(Font), Data.Defaults.DefaultFontString)]
        public Font DefaultFont { get; set; } = Data.Defaults.DefaultFont;


#if USE_ALL_PAGE_CONFIG
        /*
         *  The 'ALL' page doesn't need its own config
         *  It's name is always 'ALL', and it only uses colors from the other
         *  Color and Font are taken from the tabs they share a message with
         */
        [Browsable(true)]
        [Category("Page Options")]
        [DisplayName("Page ALL Page")]
        [Description("Setup for the main 'ALL' page")]
        [TypeConverter(typeof(ConsolePageConfigTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(ConsolePageConfigALL), Defaults.PageALLString)]
        public ConsolePageConfigALL PageALLConfig { get; set; } = new ConsolePageConfigALL(Defaults.PageALLFontName, Defaults.PageALLColor, new Font(Defaults.PageALLFontName, Defaults.PageALLFontSize));
#endif

        [Browsable(true)]
        [Category("Page Options")]
        [DisplayName("DefaultPages")]
        [Description("Pages to auto-create on start")]
        [TypeConverter(typeof(ConsolePageConfigCollectionTypeConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ConsolePageConfigCollection DefaultPages { get; set; }
    }
}
