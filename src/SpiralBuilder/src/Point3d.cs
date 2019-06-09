using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTemplate
{
    public class Point3d : IComparable, IEquatable<Point3d>
    {
        public Point3d(double x, double y, double z)
        {
            X = Math.Round(x,5);
            Y = Math.Round(y,5);
            Z = Math.Round(z,5);
        }

        public Point3d Add(double x, double y, double z)
        {
            return new Point3d(X + x, Y + y, Z + z);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public int CompareTo(object obj)
        {
            Point3d v = (Point3d)obj;

            if (v.X > X)
            {
                return 1;
            }
            else if (v.X < X)
            {
                return -1;
            }

            if (v.Y > Y)
            {
                return 1;
            }
            else if (v.Y < Y)
            {
                return -1;
            }

            if (v.Z > Z)
            {
                return 1;
            }
            else if (v.Z < Z)
            {
                return -1;
            }

            return 0;
        }

        public bool Equals(Point3d other)
        {
            //Check whether the compared object is null. 
            if (other is null) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}
