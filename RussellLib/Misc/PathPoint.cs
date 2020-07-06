using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace RussellLib.Misc
{
    public struct PathPoint
    {
        public double X;
        public double Y;
        public double Speed;

        public PathPoint(double x, double y, double speed)
        {
            X = x;
            Y = y;
            Speed = speed;
        }

        public override int GetHashCode() // ????????
        {
            return (int)(((int)X ^ (int)Y) * Speed);
        }

        public override bool Equals(object obj)
        {
            var o = (PathPoint)obj;
            return (X == o.X) && (Y == o.Y) && (Speed == o.Speed);
        }

        public static bool operator !=(PathPoint lhs, PathPoint rhs)
        {
            return (lhs.X != rhs.X) || (lhs.Y != rhs.Y) || (lhs.Speed != rhs.Speed);
        }

        public static bool operator ==(PathPoint lhs, PathPoint rhs)
        {
            return (lhs.X == rhs.X) && (lhs.Y == rhs.Y) && (lhs.Speed == rhs.Speed);
        }
    }
}
