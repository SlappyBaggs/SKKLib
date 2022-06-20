using System.Drawing;
using System.Collections.Generic;

namespace SKKLib.Console.Data
{
    public delegate void ConsoleEvent();

    public static class Defaults
    {
        public const string PageName = "Page";
        public const string PageALLName = "ALL";
        public const string PageColor = "White";
        public const string DefaultFontName = "Consolas";
        public const string DefaultFontSizeString = "12";

        public const string DefaultFontString = DefaultFontName + ", " + DefaultFontSizeString + "pt";
        public static readonly float DefaultFontSize = float.Parse(DefaultFontSizeString);
        public static readonly Font DefaultFont = new Font(DefaultFontName, DefaultFontSize);
        public static List<Color> DefaultColors = new List<Color>()
        {
            Color.Red,
            Color.Lime,
            Color.Green,
            Color.Cyan,
            Color.Blue,
            Color.White,
            Color.Yellow,
            Color.Magenta,
            Color.LightYellow,
            Color.LightGray,
            Color.Gray,
            Color.Purple,
            Color.Pink
        };

    }
}
