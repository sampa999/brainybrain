using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTemplate
{
    public class Triangle3d
    {
        public Point3d[] Vertices;
        private Triangle3d()
        {
        }

        public Triangle3d(Point3d v0, Point3d v1, Point3d v2) : base()
        {
            // (0,100,10), (0,120,10), (0,120,0),
            Vertices = new Point3d[3];

            var foo = 6;
            if (v0.X == 0 && v0.Y == 100 && v0.Z == 10 &&
                v1.X == 0 && v1.Y == 120 && v1.Y == 10 &&
                v2.X == 0 && v2.Y == 120 && v2.Z == 0)
            {
                foo = 7;
            }
            Vertices[0] = v0;
            Vertices[1] = v1;
            Vertices[2] = v2;
        }
    }
}
