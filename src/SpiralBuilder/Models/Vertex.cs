using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Vertex : IComparable, IEquatable<Vertex>
    {
        public Vertex(double x, double y, double z)
        {
            X = Math.Round(x,5);
            Y = Math.Round(y,5);
            Z = Math.Round(z,5);
        }

        public Vertex Add(double x, double y, double z)
        {
            return new Vertex(X + x, Y + y, Z + z);
        }

        public static Vertex Average(Vertex v1, Vertex v2, double v1Weight)
        {
            return new Vertex
            (
                (v1.X * v1Weight + v2.X) / (1.0 + v1Weight),
                (v1.Y * v1Weight + v2.Y) / (1.0 + v1Weight),
                (v1.Z * v1Weight + v2.Z) / (1.0 + v1Weight)
            );
        }

        public static Vertex Average(params Vertex[] vertices)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            int count = 0;

            foreach (var v in vertices)
            {
                x += v.X;
                y += v.Y;
                z += v.Z;
                count++;
            }

            return new Vertex(x / count, y / count, z / count);
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public int CompareTo(object obj)
        {
            Vertex v = (Vertex)obj;

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

        public bool Equals(Vertex other)
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
