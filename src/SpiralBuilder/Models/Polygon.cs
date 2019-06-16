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
    }
}
