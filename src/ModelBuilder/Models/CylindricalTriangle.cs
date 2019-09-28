using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CylindricalTriangle
    {
        public CylindricalTriangle(
            CylindricalVertex v1,
            CylindricalVertex v2,
            CylindricalVertex v3)
        {
            Vertices = new List<CylindricalVertex>
            {
                v1,
                v2,
                v3
            };
        }

        public List<CylindricalVertex> Vertices { get; set; }

        public Triangle3d ToTriangle3d()
        {
            var vertices = new List<Vertex>();

            foreach (var v in Vertices)
            {
                vertices.Add(new Vertex(v.X, v.Y, v.Z));
            }

            return new Triangle3d(vertices[0], vertices[1], vertices[2]);
        }
    }
}
