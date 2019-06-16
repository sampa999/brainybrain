/*
 * Class to encapsulate a 3d polygon
 */

using System.Collections.Generic;

namespace Models
{
    public class Trapezoid3d
    {
        public Vertex[] Vertices;

        public Trapezoid3d[] Split()
        {
            var trapezoids = new Trapezoid3d[2];

            var v4 = Vertex.Average(Vertices[1], Vertices[2], 3);
            var v5 = Vertex.Average(Vertices[0], Vertices[3], 3);

            trapezoids[0] = new Trapezoid3d(Vertices[0], Vertices[1], v4, v5);
            trapezoids[1] = new Trapezoid3d(v5, v4, Vertices[2], Vertices[3]);

            return trapezoids;
        }

        public Trapezoid3d(params Vertex[] v)
        {
            Vertices = v;
            CalculateTriangles();
        }

        private void CalculateTriangles()
        {
            var vertexList = new List<Vertex>();
            vertexList.AddRange(Vertices);
            var triangleList = new List<Triangle3d>();
            while (vertexList.Count > 3)
            {
                triangleList.Add(new Triangle3d(vertexList[0], vertexList[1], vertexList[2]));
                vertexList.RemoveAt(1);
            }
            triangleList.Add(new Triangle3d(vertexList[0], vertexList[1], vertexList[2]));

            Triangles = triangleList.ToArray();
        }

        public Triangle3d[] Triangles { get; private set; }
    }
}
