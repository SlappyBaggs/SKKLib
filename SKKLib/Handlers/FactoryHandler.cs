using System;
using System.Drawing;

namespace SKKLib.Handlers
{
    public static class FactoryHandler
    {
        public static Point GetPoint(Int32 i) {  return new Point(i); }
        public static Point GetPoint(Int32 x, Int32 y) { return new Point(x, y); }
        public static Point GetPoint(Size s) { return new Point(s); }
    }
}

