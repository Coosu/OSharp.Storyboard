using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb
{
    public struct Range<T> where T : struct
    {
        public T X;
        public T Y;
        public Range(T x, T y)
        {
            X = x;
            Y = y;
        }
    }
}
