using System;
using System.Collections.Generic;
using System.Linq;
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

        public CylindricalPolygon(CylindricalPolygon inputPolgon)
        {
            Vertices = new List<CylindricalVertex>();
            Vertices.AddRange(inputPolgon.Vertices);
        }

        public List<CylindricalVertex> Vertices { get; set; }

        public List<CylindricalTriangle> Triangles
        {
            get
            {
                var centerVertex = CylindricalVertex.Average(Vertices);

                var triangles = new List<CylindricalTriangle>();

                for (var i=1; i<Vertices.Count; i++)
                {
                    triangles.Add(
                        new CylindricalTriangle(
                            Vertices[i - 1], 
                            Vertices[i], 
                            centerVertex));
                }

                triangles.Add(
                    new CylindricalTriangle(
                        Vertices.Last(),
                        Vertices[0],
                        centerVertex));

                return triangles;
            }
        }

        public static CylindricalPolygon Translate(CylindricalPolygon inputPolygon, CylindricalVertex offset)
        {
            var vertices = new List<CylindricalVertex>();

            foreach (var v in inputPolygon.Vertices)
            {
                vertices.Add(new CylindricalVertex(v.Radius + offset.Radius, v.Phi + offset.Phi, v.Z + offset.Z));
            }

            return new CylindricalPolygon(vertices);
        }

        public static void FlipNormal(CylindricalPolygon inputPolygon)
        {
            var vertices = inputPolygon.Vertices;

            var first = vertices.First();
            vertices.Remove(first);
            vertices.Reverse();
            vertices.Insert(0, first);
        }
    }

}
