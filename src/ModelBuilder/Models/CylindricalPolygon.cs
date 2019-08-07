using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CylindricalPolygon
    {
        public CylindricalPolygon(
            IEnumerable<CylindricalVertex> vertices)
        {
            Vertices = new List<CylindricalVertex>();
            Vertices.AddRange(vertices);
        }

        public List<CylindricalVertex> Vertices { get; set; }

        public List<CylindricalTriangle> Triangles
        {
            get
            {
                var triangles = new List<CylindricalTriangle>
                {
                    new CylindricalTriangle(Vertices[0], Vertices[1], Vertices[2]),
                    new CylindricalTriangle(Vertices[2], Vertices[1], Vertices[3])
                };
                return triangles;
            }
        }
    }

}
