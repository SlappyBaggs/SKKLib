using System.ComponentModel;
using System.Drawing;
using SKKLib.Console.Data;

namespace SKKLib.Console.Config
{
    [TypeConverter(typeof(ConsolePageConfigTypeConverter))]
    public class ConsolePageConfig
    {
        #region CONSTRUCTORS
        public ConsolePageConfig() : this(Defaults.PageName) { }
        public ConsolePageConfig(string name) : this(name, Color.Empty, null) { }
        public ConsolePageConfig(string name, Color c) : this(name, c, null) { }
        public ConsolePageConfig(string name, Color c, Font f)
        {
            PageName = name;
            PageColor = c;// (c != Color.Empty) ? c : SKKConsole.NextColor;
            PageFont = f;// (f != null) ? f : SKKConsole.DefaultFont;
        }
        #endregion


        #region CONSOLE PAGE CONFIG PROPERTIES
        private string pageName_;
        [Category("Page Config")]
        [DefaultValue(Defaults.DefaultFontName)]
        public virtual string PageName
        {
            get => pageName_;
            set => pageName_ = (PageName == Defaults.PageALLName) ? PageName : value;
        }

        [Category("Page Config")]
        [DefaultValue(typeof(Color), Defaults.PageColor)]
        public Color PageColor { get; set; }// = Color.Red;

        [Category("Page Config")]
        [DefaultValue(typeof(Font), Defaults.DefaultFontString)]
        public Font PageFont { get; set; }
        #endregion


        #region OVERRIDES
        public override string ToString() => $"{PageName}{ConsolePageConfigTypeConverter.delim_}{PageColor.Name}{ConsolePageConfigTypeConverter.delim_}" + ((PageFont == null) ? string.Empty : PageFont.ToString());
        public override bool Equals(object obj)
        {
            if (!(obj is ConsolePageConfig)) return base.Equals(obj);
            ConsolePageConfig cpc = obj as ConsolePageConfig;
            return (cpc == null) ? false : cpc.PageName.Equals(PageName) && cpc.PageColor.Equals(PageColor) && 
                (((cpc.PageFont is null) || (PageFont is null))?((cpc.PageFont is null) && (PageFont is null)):cpc.PageFont.Equals(PageFont));
        }
        public override int GetHashCode() => (int)(PageName.Length * PageColor.ToArgb() * ((PageFont == null) ? 1 : PageFont.Size));
        public static bool operator ==(ConsolePageConfig c1, ConsolePageConfig c2)
        {
            bool b1 = c1 is null;
            bool b2 = c2 is null;

            bool b3;
            bool b4, b5, b6;

            // Are either of them null?
            if (b3 = (b1 || b2))
            {
                // YES, at least 1 is null
                // return true if both are null (null equals null, right?)
                // return false if only 1 is null
                return (b1 && b2);
            }
            else
            {
                // NO, neither are null, compare properties
                b4 = c1.PageName == c2.PageName;
                b5 = c1.PageColor == c2.PageColor;
                b6 = c1.PageFont == c2.PageFont;

                // if all checks are true, return true
                // if even 1 fails, then return false
                return b4 && b5 && b6;
            }

            //if(c1 is null || c2 is null) ? (c1 is null && c2 is null) : ((c1.PageName == c2.PageName) && (c1.PageColor == c2.PageColor) && (c2.PageFont == c2.PageFont));
        }
        public static bool operator !=(ConsolePageConfig c1, ConsolePageConfig c2) => !(c1 == c2);

        #endregion
    }
}
