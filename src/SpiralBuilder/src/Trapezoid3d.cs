/*
 * Class to encapsulate a 3d square
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTemplate
{
    public class Trapezoid3d
    {
        public Point3d[] Vertices;
        private Trapezoid3d()
        {
        }

        public Trapezoid3d(Point3d v0, Point3d v1, Point3d v2, Point3d v3) : base()
        {
            Vertices = new Point3d[4];
            Triangles = new Triangle3d[2];

            Vertices[0] = v0;
            Vertices[1] = v1;
            Vertices[2] = v2;
            Vertices[3] = v3;
            Triangles[0] = new Triangle3d(v0, v1, v3);
            Triangles[1] = new Triangle3d(v1, v2, v3);
        }

        public Triangle3d[] Triangles { get; private set; }
    }
}
