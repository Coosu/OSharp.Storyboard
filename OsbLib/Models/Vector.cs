using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkitic.OsbLib.Models
{
    public struct Vector2
    {
        public Vector2(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }

    public struct Vector3
    {
        public Vector3(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z{ get; set; }
    }
}
