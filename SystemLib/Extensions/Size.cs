using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.SystemLib.Extensions
{
    public static class Extensions
    {
        // Size
        public static Size Multiply(this Size size, double scale) => new Size((int)(size.Width * scale), (int)(size.Height * scale));
    }
}
