using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Models
{
    /// <summary>
    /// Object composed of triangles.
    /// Can be created from Triangle3d collection.
    /// Exposes Vertex Collection and Triangle Collection that references the Vertex Collection by index
    /// </summary>
    public class TriangleObject
    {
        public class Triangle
        {
            public Triangle()
            {
                Vertex = new int[3];
            }

            public int [] Vertex { get; set; }
        }

        public TriangleObject(Triangle3d [] triangles)
        {
            ExtractVertices(triangles);
            MapTriangles(triangles);
        }

        private void MapTriangles(Triangle3d[] triangles)
        {
            Triangles = new Triangle[triangles.Length];

            for (var i=0;  i<triangles.Length; i++)
            {
                Triangles[i] = new Triangle();
                for (var vIndex = 0; vIndex < 3; vIndex++)
                {
                    var index = Array.BinarySearch(Vertices, triangles[i].Vertices[vIndex]);
                    var index2 = FindPointInArray(Vertices, triangles[i].Vertices[vIndex]);
                    Debug.Assert(index == index2);
                    if (index < 0)
                    {
                        throw new Exception("Unable to locate vertex in Vertices list");
                    }
                    Triangles[i].Vertex[vIndex] = index;
                }
            }
        }

        private int FindPointInArray(Point3d[] pArray, Point3d p)
        {
            for (var i=0; i<pArray.Length; i++)
            {
                if (p.X == pArray[i].X && p.Y == pArray[i].Y && p.Z == pArray[i].Z)
                {
                    return i;
                }
            }

            return -1;
        }

        private void ExtractVertices(IEnumerable<Triangle3d>  triangles)
        {
            var vertexList = new List<Point3d>();

            foreach (var triangle in triangles)
            {
                foreach (var v in triangle.Vertices)
                {
                    vertexList.Add(new Point3d(v.X, v.Y, v.Z));
                }
            }

            var distinctList = vertexList.Distinct().ToList();
            distinctList.Sort();
            Vertices = distinctList.ToArray();
        }

        public Point3d[] Vertices { get; private set; }
        public Triangle[] Triangles { get; private set; }
    }
}
