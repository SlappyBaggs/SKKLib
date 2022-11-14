using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKKLib.Controls.Data
{
    public struct SKKPadding
    {
        public SKKPadding(int pad)
        {
            All = pad;
        }
        public SKKPadding(int horiz, int vert) : this(horiz / 2, vert / 2, horiz / 2, vert / 2)
        {
        }

        public SKKPadding(int padL, int padT, int padR, int padB)
        {
            Left = padL;
            Right = padR;
            Top = padT;
            Bottom = padB;
        }

        public int Left { get; set; } = 0;
        public int Right { get; set; } = 0;
        public int Top { get; set; } = 0;
        public int Bottom { get; set; } = 0;
        public int Horizontal { get => Left + Right; }
        public int Vertical { get => Top + Bottom; }
        public int All
        {
            get => ((Left == Right) && (Left == Top) && (Left == Bottom)) ? Left : -1;
            set => Left = Right = Top = Bottom = value;
        }
        public Size Size { get => new Size(Horizontal, Vertical); }

        public static SKKPadding operator +(SKKPadding pad1, SKKPadding pad2) => new SKKPadding(pad1.Left + pad2.Left, pad1.Top + pad2.Top, pad1.Right + pad2.Right, pad1.Bottom + pad2.Bottom);
        public static SKKPadding operator +(SKKPadding pad1, Padding pad2) => new SKKPadding(pad1.Left + pad2.Left, pad1.Top + pad2.Top, pad1.Right + pad2.Right, pad1.Bottom + pad2.Bottom);
        public static Padding operator + (Padding pad1, SKKPadding pad2) => new Padding(pad1.Left + pad2.Left, pad1.Top + pad2.Top, pad1.Right + pad2.Right, pad1.Bottom + pad2.Bottom); 
    
        public static bool operator == (Padding pad1, SKKPadding pad2) => (pad1.Left == pad2.Left) && (pad1.Top == pad2.Top) && (pad1.Right == pad2.Right) && (pad1.Bottom == pad2.Bottom);
        public static bool operator !=(Padding pad1, SKKPadding pad2) => (pad1.Left != pad2.Left) || (pad1.Top != pad2.Top) || (pad1.Right != pad2.Right) || (pad1.Bottom != pad2.Bottom);

        public static bool operator == (SKKPadding pad1, Padding pad2) => (pad1.Left == pad2.Left) && (pad1.Top == pad2.Top) && (pad1.Right == pad2.Right) && (pad1.Bottom == pad2.Bottom);
        public static bool operator !=(SKKPadding pad1, Padding pad2) => (pad1.Left != pad2.Left) || (pad1.Top != pad2.Top) || (pad1.Right != pad2.Right) || (pad1.Bottom != pad2.Bottom);
    }
}
