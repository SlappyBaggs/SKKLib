using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.Math.Data
{
    public struct Point2<T>
    {
        public Point2(T x_, T y_)
        {
            X = x_; ;
            Y = y_; ;
        }

        public T X { get; private set; }
        public T Y { get; private set; }

        public override string ToString()
        {
            //return string.Format("X={0:D3} Y={1:D3}", this.X, this.Y);
            return $"X={X} Y={Y}";
        }
    }
}
