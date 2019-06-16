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

            CalculateTriangles();
        }

        private void CalculateTriangles()
        {
            CalculateTopTriangles();
            CalculateBottomTriangles();
            CalculateLeftTriangles();
            CalculateRightTriangles();
            CalculateInsideTriangles();
            CalculateOutsideTriangles();
        }

        private void CalculateTopTriangles()
        {
            var trapezoid = new Trapezoid3d(
                InnerLeft.Add(0, 0, Height),
                InnerRight.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height),
                OuterLeft.Add(0, 0, Height)
                );

            TopTrapezoids = trapezoid.Split();
        }

        private void CalculateBottomTriangles()
        {
            var trapezoid = new Trapezoid3d(
                InnerRight,
                InnerLeft,
                OuterLeft,
                OuterRight
                );

            BottomTrapezoids = trapezoid.Split();
        }

        private void CalculateLeftTriangles()
        {
            var trapezoid = new Trapezoid3d(
                    InnerLeft,
                    InnerLeft.Add(0, 0, Height),
                    OuterLeft.Add(0, 0, Height),
                    OuterLeft
                    );

            LeftTrapezoids = trapezoid.Split();
        }

        private void CalculateRightTriangles()
        {
            var trapezoid = new Trapezoid3d(
                    InnerRight.Add(0, 0, Height),
                    InnerRight,
                    OuterRight,
                    OuterRight.Add(0, 0, Height)
                    );

            RightTrapezoids = trapezoid.Split();
        }

        private void CalculateInsideTriangles()
        {
            var trapezoid = new Trapezoid3d(
                InnerLeft,
                InnerRight,
                InnerRight.Add(0, 0, Height),
                InnerLeft.Add(0, 0, Height)
                );

            InsideTriangles = trapezoid.Triangles;
        }

        private void CalculateOutsideTriangles()
        {
            var trapezoid = new Trapezoid3d(
                OuterRight,
                OuterLeft,
                OuterLeft.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height)
                );

            OutsideTriangles = trapezoid.Triangles;
        }

        public Trapezoid3d[] TopTrapezoids { get; private set; }
        public Triangle3d[] TopTriangles
        {
            get
            {
                return TopTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Trapezoid3d[] BottomTrapezoids { get; private set; }
        public Triangle3d[] BottomTriangles
        {
            get
            {
                return BottomTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Trapezoid3d[] LeftTrapezoids { get; private set; }
        public Triangle3d[] LeftTriangles
        {
            get
            {
                return LeftTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Trapezoid3d[] RightTrapezoids { get; private set; }
        public Triangle3d[] RightTriangles
        {
            get
            {
                return RightTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Triangle3d[] InsideTriangles { get; private set; }
        public Triangle3d[] OutsideTriangles { get; private set; }
    }
}
