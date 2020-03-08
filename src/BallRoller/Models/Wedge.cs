/*
 * The Wedge class holds the information about a single wedge of the platform.
 */
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Wedge
    {
        public readonly Vertex InnerLeft;
        public readonly Vertex InnerRight;
        public readonly Vertex OuterLeft;
        public readonly Vertex OuterRight;
        public readonly double Height;
        public readonly int AngleForRightSide;

        public Wedge(
            Vertex innerLeft,
            Vertex innerRight,
            Vertex outerLeft,
            Vertex outerRight,
            double height,
            int angleForRightSide)
        {
            InnerLeft = innerLeft;
            InnerRight = innerRight;
            OuterLeft = outerLeft;
            OuterRight = outerRight;
            Height = height;
            AngleForRightSide = angleForRightSide;

            CreateSides();
        }

        public Polygon[] RemoveBottom()
        {
            var p = BottomTrapezoids;
            BottomTrapezoids = new Polygon[0];
            return p;
        }

        public void RemoveTopOutside()
        {
            var l = new List<Polygon>();

            for (var i=0; i<TopTrapezoids.Length-1; i++)
            {
                l.Add(TopTrapezoids[i]);
            }
            TopTrapezoids = l.ToArray();
        }

        private void CreateSides()
        {
            CreateTop();
            CreateBottom();
            CreateLeft();
            CreateRight();
            CreateInside();
            CreateOutside();
        }

        Vertex topVertex4;
        Vertex topVertex5;
        Vertex topVertex6;
        Vertex topVertex7;
        Vertex topVertex8;
        Vertex topVertex9;

        Vertex dropZ = new Vertex(0, 0, 2);

        private void CreateTop()
        {
            var trapezoid = new Trapezoid3d(
                InnerLeft.Add(0, 0, Height),
                InnerRight.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height),
                OuterLeft.Add(0, 0, Height)
                );

            var vDiff = Vertex.Subtract(trapezoid.Vertices[3], trapezoid.Vertices[0]);
            vDiff = Vertex.Divide(vDiff, 4);
            topVertex5 = Vertex.Subtract(trapezoid.Vertices[3], vDiff);
            topVertex7 = Vertex.Subtract(topVertex5, vDiff);
            topVertex9 = Vertex.Subtract(topVertex7, vDiff);
            topVertex7 = Vertex.Subtract(topVertex7, dropZ);
            topVertex9 = Vertex.Subtract(topVertex9, dropZ);

            vDiff = Vertex.Subtract(trapezoid.Vertices[2], trapezoid.Vertices[1]);
            vDiff = Vertex.Divide(vDiff, 4);
            topVertex4 = Vertex.Subtract(trapezoid.Vertices[2], vDiff);
            topVertex6 = Vertex.Subtract(topVertex4, vDiff);
            topVertex8 = Vertex.Subtract(topVertex6, vDiff);
            topVertex6 = Vertex.Subtract(topVertex6, dropZ);
            topVertex8 = Vertex.Subtract(topVertex8, dropZ);

            TopTrapezoids = new Polygon[]
            {
                new Trapezoid3d(
                    trapezoid.Vertices[0],
                    trapezoid.Vertices[1],
                    topVertex8,
                    topVertex9),
                new Trapezoid3d(
                    topVertex9,
                    topVertex8,
                    topVertex6,
                    topVertex7),
                new Trapezoid3d(
                    topVertex7,
                    topVertex6,
                    topVertex4,
                    topVertex5),
                new Trapezoid3d(
                    topVertex5,
                    topVertex4,
                    trapezoid.Vertices[2],
                    trapezoid.Vertices[3])
            };

            var topTrapezoids = trapezoid.Split();
            var tmpmidTopLeft = new Vertex[] { TopTrapezoids[1].Vertices[0] };
            var tmpmidTopRight = new Vertex[] { TopTrapezoids[1].Vertices[1] };
        }

        private void CreateBottom()
        {
            BottomTrapezoids = new Polygon[]
            {
                new Trapezoid3d(
                    InnerRight,
                    InnerLeft,
                    OuterLeft,
                    OuterRight
                )
            };
        }

        private void CreateLeft()
        {
            LeftPolygons =
                new Polygon[]
                {
                new Polygon(
                    InnerLeft,
                    InnerLeft.Add(0, 0, Height),
                    topVertex9,
                    topVertex7,
                    topVertex5,
                    OuterLeft.Add(0, 0, Height),
                    OuterLeft
                    )
                };
        }

        private void CreateRight()
        {
            RightPolygons =
                new Polygon[]
                {
                    new Polygon(
                        topVertex4,
                        topVertex6,
                        topVertex8,
                        InnerRight.Add(0, 0, Height),
                        InnerRight,
                        OuterRight,
                        OuterRight.Add(0, 0, Height)
                        )
                };
        }

        private void CreateInside()
        {
            var trapezoid = new Trapezoid3d(
                InnerLeft,
                InnerRight,
                InnerRight.Add(0, 0, Height),
                InnerLeft.Add(0, 0, Height)
                );

            InsideTriangles = trapezoid.Triangles;
        }

        private void CreateOutside()
        {
            var trapezoid = new Trapezoid3d(
                OuterRight,
                OuterLeft,
                OuterLeft.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height)
                );

            OutsideTriangles = trapezoid.Triangles;
        }

        public Polygon[] TopTrapezoids { get; private set; }
        public Triangle3d[] TopTriangles
        {
            get
            {
                return TopTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Polygon[] BottomTrapezoids { get; private set; }
        public Triangle3d[] BottomTriangles
        {
            get
            {
                return BottomTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Polygon[] LeftPolygons { get; private set; }
        public Triangle3d[] LeftTriangles
        {
            get
            {
                return LeftPolygons.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Polygon[] RightPolygons { get; private set; }
        public Triangle3d[] RightTriangles
        {
            get
            {
                return RightPolygons.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Triangle3d[] InsideTriangles { get; private set; }
        public Triangle3d[] OutsideTriangles { get; private set; }
    }
}
