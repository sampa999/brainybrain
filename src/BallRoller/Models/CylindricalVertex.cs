using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CylindricalVertex : IComparable, IEquatable<CylindricalVertex>
    {
        public static CylindricalVertex Average(List<CylindricalVertex> vertices)
        {
            double radius = 0;
            double phi = 0;
            double z = 0;
            int count = 0;

            foreach (var v in vertices)
            {
                radius += v.Radius;
                phi += v.Phi;
                z += v.Z;
                count++;
            }

            return new CylindricalVertex(radius / count, phi / count, z / count);
        }


        public CylindricalVertex(double radius, double phi, double z)
        {
            Radius = radius;
            Phi = phi;
            Z = z;
        }

        public double Radius { get; private set; }
        public double Phi { get; private set; }
        public double Z { get; private set; }

        public double X
        {
            get
            {
                return Math.Cos((double)Phi * Math.PI / 180.0) * Radius;
            }
        }

        public double Y
        {
            get
            {
                return Math.Sin((double)Phi * Math.PI / 180.0) * Radius;
            }
        }

        public int CompareTo(object obj)
        {
            var v = (CylindricalVertex)obj;

            if (v.Z > Z)
            {
                return 1;
            }
            else if (v.Z < Z)
            {
                return -1;
            }

            if (v.Phi > Phi)
            {
                return 1;
            }
            else if (v.Phi < Phi)
            {
                return -1;
            }

            if (v.Radius > Radius)
            {
                return 1;
            }
            else if (v.Radius < Radius)
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
