using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

using SKKLib.SystemLib.Extensions;
using System.Linq;
using System;

namespace SKKLib.Controls.Validators
{
    public static class Validators
    {
        private static ErrorProvider errorProvider = new ErrorProvider();

        public static bool PointTryParse(string s, out Point p)
        {
            p = new Point();
            string[] sa = s.Split(',');
            int[] ints = Array.ConvertAll(sa, s => int.TryParse(s, out int x) ? x : -1);
            p.X = ints[0];
            p.Y = ints[1];
            return ((ints.Length == 2) && (!ints.Contains(-1)));
        }

        public static void PointValidator(object sender, CancelEventArgs args)
        {
            if ((((Control)sender).Text != "") && (args.Cancel = !PointTryParse(((Control)sender).Text, out _))) errorProvider.SetError(((Control)sender), $"Invalid Point");
            else errorProvider.SetError((sender as Control), "");
        }

        public static bool SizeTryParse(string s, out Size sz)
        {
            sz = new Size();
            string[] sa = s.Split(',');
            int[] ints = Array.ConvertAll(sa, s => int.TryParse(s, out int x) ? ((x >= 0) ? x : -1) : -1);
            sz.Width = ints[0];
            sz.Height = ints[1];
            return ((ints.Length == 2) && (!ints.Contains(-1)));
        }

        public static void SizeValidator(object sender, CancelEventArgs args)
        {
            if ((((Control)sender).Text != "") && (args.Cancel = !SizeTryParse(((Control)sender).Text, out _))) errorProvider.SetError(((Control)sender), $"Invalid Size");
            else errorProvider.SetError((sender as Control), "");
        }
    }
}
