using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Polygon
    {
        public Vertex[] Vertices { get; private set; }

        public Polygon(params Vertex[] vertices)
        {
            if (vertices.Length < 3)
            {
                throw new ArgumentException("Requires 3 or more vertices", nameof(vertices));
            }

            Vertices = vertices;
        }

        public Triangle3d[] Triangles
        {
            get
            {
                var triangles = new List<Triangle3d>();
                if (Vertices.Length == 3)
                {
                    triangles.Add(new Triangle3d(Vertices[0], Vertices[1], Vertices[2]));
                }
                else
                {
                    var centerVertex = Vertex.Average(Vertices);

                    for (var i = 0; i < Vertices.Length - 1; i++)
                    {
                        triangles.Add(
                            new Triangle3d(
                                Vertices[i],
                                Vertices[i + 1],
                                centerVertex));
                    }
                    triangles.Add(
                        new Triangle3d(
                            Vertices[Vertices.Length - 1],
                            Vertices[0],
                            centerVertex));
                }
                return triangles.ToArray();
            }
        }
    }
}
