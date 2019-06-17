/*
 * The Wedge class holds the information about a single wedge of the platform.
 */
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

        public Wedge(
            Vertex innerLeft,
            Vertex innerRight,
            Vertex outerLeft,
            Vertex outerRight,
            double height)
        {
            InnerLeft = innerLeft;
            InnerRight = innerRight;
            OuterLeft = outerLeft;
            OuterRight = outerRight;
            Height = height;

            CreateSides();
        }

        public void RemoveBottom()
        {
            BottomTrapezoids = new Polygon[0];
        }

        public void RemoveTopOutside()
        {
            TopTrapezoids = new Polygon[] { TopTrapezoids[0] };
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

        Vertex midTopLeft;
        Vertex midTopRight;

        private void CreateTop()
        {
            var trapezoid = new Trapezoid3d(
                InnerLeft.Add(0, 0, Height),
                InnerRight.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height),
                OuterLeft.Add(0, 0, Height)
                );

            TopTrapezoids = trapezoid.Split();
            midTopLeft = TopTrapezoids[1].Vertices[0];
            midTopRight = TopTrapezoids[1].Vertices[1];
        }

        private void CreateBottom()
        {
            BottomTrapezoids = new Trapezoid3d[]
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
                    midTopLeft,
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
                        midTopRight,
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
