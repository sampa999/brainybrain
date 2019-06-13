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
    public class Polygon3d
    {
        public Point3d[] Vertices;

        public Polygon3d(params Point3d[] v)
        {
            Vertices = v;
            CalculateTriangles();
        }

        private void CalculateTriangles()
        {
            var vertexList = new List<Point3d>();
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
