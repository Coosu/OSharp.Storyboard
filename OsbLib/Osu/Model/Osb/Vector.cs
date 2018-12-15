using System;
using System.Collections.Generic;
using System.Text;

namespace MGLib.Osu.Model.Osb
{
    public struct Vector2<T> where T : struct
    {
        public T X;
        public T Y;
        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }
    }

    public struct Vector3<T> where T : struct
    {
        public T X;
        public T Y;
        public T Z;
        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
