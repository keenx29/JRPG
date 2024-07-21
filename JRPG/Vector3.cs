using System;
using System.Collections.Generic;
using System.Text;

namespace JRPG
{
    public class Vector3
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
