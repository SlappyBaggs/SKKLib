using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKKLib.Controls.Data
{
    public class SKKPadding
    {
        public SKKPadding()
        {
        }

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

        private static ErrorProvider errorProvider = new ErrorProvider();

        public static void SKKPaddingValidating(object sender, CancelEventArgs e)
        {
            if ((((Control)sender).Text != "") && (e.Cancel = !SKKPadding.TryParse(((Control)sender).Text, out _))) errorProvider.SetError(((Control)sender), $"Invalid Padding");
            else errorProvider.SetError((sender as Control), "");
        }

        public string PaddingS
        {
            get => ToString();
            set
            {
                if (TryParse(value, out SKKPadding p))
                {
                    Left = p.Left;
                    Right = p.Right;
                    Top = p.Top;
                    Bottom = p.Bottom;
                }
            }
        }
        public override string ToString() => $"{Left},{Top},{Right},{Bottom}";
        public void FromString(string s)
        {
            string[] sa = s.Split(',');
            int[] ints = Array.ConvertAll(sa, s => int.TryParse(s, out int x) ? x : -1);
            if (ints.Length != 4) throw new InvalidOperationException("Invalid number of padding values");
            if (ints.Contains(-1)) throw new InvalidOperationException("Unparsable value");
            Left = ints[0];
            Top = ints[1];
            Right = ints[2];
            Bottom = ints[3];
        }

        public static bool TryParse(string s, out SKKPadding p)
        {
            p = new SKKPadding();
            string[] sa = s.Split(',');
            int[] ints = Array.ConvertAll(sa, s => int.TryParse(s, out int x) ? x : -1);
            if ((ints.Length != 4) || (ints.Contains(-1))) return false;
            p.Left = ints[0];
            p.Top = ints[1];
            p.Right = ints[2];
            p.Bottom = ints[3];
            return true;
        }


        public static SKKPadding operator +(SKKPadding pad1, SKKPadding pad2) => new SKKPadding(pad1.Left + pad2.Left, pad1.Top + pad2.Top, pad1.Right + pad2.Right, pad1.Bottom + pad2.Bottom);
        public static SKKPadding operator +(SKKPadding pad1, Padding pad2) => new SKKPadding(pad1.Left + pad2.Left, pad1.Top + pad2.Top, pad1.Right + pad2.Right, pad1.Bottom + pad2.Bottom);
        public static Padding operator + (Padding pad1, SKKPadding pad2) => new Padding(pad1.Left + pad2.Left, pad1.Top + pad2.Top, pad1.Right + pad2.Right, pad1.Bottom + pad2.Bottom);

        public override bool Equals(object o) => base.Equals(o);
        public override int GetHashCode() => base.GetHashCode();

        public static bool Equals(SKKPadding left, SKKPadding right) => left == right;
        public static bool operator ==(SKKPadding pad1, SKKPadding pad2) => (pad1.Left == pad2.Left) && (pad1.Top == pad2.Top) && (pad1.Right == pad2.Right) && (pad1.Bottom == pad2.Bottom);
        public static bool operator !=(SKKPadding pad1, SKKPadding pad2) => (pad1.Left != pad2.Left) || (pad1.Top != pad2.Top) || (pad1.Right != pad2.Right) || (pad1.Bottom != pad2.Bottom);

        public static bool Equals(Padding left, SKKPadding right) => left == right;
        public static bool operator == (Padding pad1, SKKPadding pad2) => (pad1.Left == pad2.Left) && (pad1.Top == pad2.Top) && (pad1.Right == pad2.Right) && (pad1.Bottom == pad2.Bottom);
        public static bool operator !=(Padding pad1, SKKPadding pad2) => (pad1.Left != pad2.Left) || (pad1.Top != pad2.Top) || (pad1.Right != pad2.Right) || (pad1.Bottom != pad2.Bottom);

        public static bool Equals(SKKPadding left, Padding right) => left == right;
        public static bool operator == (SKKPadding pad1, Padding pad2) => (pad1.Left == pad2.Left) && (pad1.Top == pad2.Top) && (pad1.Right == pad2.Right) && (pad1.Bottom == pad2.Bottom);
        public static bool operator !=(SKKPadding pad1, Padding pad2) => (pad1.Left != pad2.Left) || (pad1.Top != pad2.Top) || (pad1.Right != pad2.Right) || (pad1.Bottom != pad2.Bottom);
    }
}
