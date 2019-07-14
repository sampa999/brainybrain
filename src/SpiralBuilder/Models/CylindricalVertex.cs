using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CylindricalVertex : IComparable, IEquatable<CylindricalVertex>
    {
        public CylindricalVertex(double radius, double phi, double z)
        {
            Radius = radius;
            Phi = phi;
            Z = z;
        }

        public CylindricalVertex Add(double radius, double phi, double z)
        {
            return new CylindricalVertex(Radius + radius, (Phi + phi) % 360.0, Z + z);
        }

        public static Vertex Subtract(Vertex v1, Vertex v2)
        {
            return new Vertex
                (
                v1.X - v2.X,
                v1.Y - v2.Y,
                v1.Z - v2.Z
                );
        }

        public static Vertex Divide(Vertex v1, double v2)
        {
            return new Vertex(
                v1.X / v2,
                v1.Y / v2,
                v1.Z / v2);
        }

        public static Vertex Multiply(Vertex v1, double v2)
        {
            return new Vertex(
                v1.X * v2,
                v1.Y * v2,
                v1.Z * v2);
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

        public double Radius { get; private set; }
        public double Phi { get; private set; }
        public double Z { get; private set; }

        public int CompareTo(object obj)
        {
            Vertex v = (Vertex)obj;

            if (v.X > Radius)
            {
                return 1;
            }
            else if (v.X < Radius)
            {
                return -1;
            }

            if (v.Y > Phi)
            {
                return 1;
            }
            else if (v.Y < Phi)
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

        public bool Equals(CylindricalVertex other)
        {
            //Check whether the compared object is null. 
            if (other is null) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return Radius == other.Radius && Phi == other.Phi && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return Radius.GetHashCode() ^ Phi.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return $"({Radius},{Phi},{Z})";
        }
    }
}
