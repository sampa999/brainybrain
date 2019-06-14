/*
 * The Wedge class holds the information about a single wedge of the platform.
 */
using System.Linq;

namespace Models
{
    public class Wedge
    {
        public readonly Point3d InnerLeft;
        public readonly Point3d InnerRight;
        public readonly Point3d OuterLeft;
        public readonly Point3d OuterRight;
        public readonly double Height;

        public Wedge(
            Point3d innerLeft,
            Point3d innerRight,
            Point3d outerLeft,
            Point3d outerRight,
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
            var trapezoid = new Polygon3d(
                InnerLeft.Add(0, 0, Height),
                InnerRight.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height),
                OuterLeft.Add(0, 0, Height)
                );

            TopTrapezoids = new Polygon3d[] { trapezoid };
        }

        private void CalculateBottomTriangles()
        {
            var trapezoid = new Polygon3d(
                InnerRight,
                InnerLeft,
                OuterLeft,
                OuterRight
                );

            BottomTriangles = trapezoid.Triangles;
        }

        private void CalculateLeftTriangles()
        {
            var trapezoid = new Polygon3d(
                InnerLeft,
                InnerLeft.Add(0, 0, Height),
                OuterLeft.Add(0, 0, Height),
                OuterLeft
                );

            LeftTriangles = trapezoid.Triangles;
        }

        private void CalculateRightTriangles()
        {
            var trapezoid = new Polygon3d(
                InnerRight.Add(0, 0, Height),
                InnerRight,
                OuterRight,
                OuterRight.Add(0, 0, Height)
                );

            RightTriangles = trapezoid.Triangles;
        }

        private void CalculateInsideTriangles()
        {
            var trapezoid = new Polygon3d(
                InnerLeft,
                InnerRight,
                InnerRight.Add(0, 0, Height),
                InnerLeft.Add(0, 0, Height)
                );

            InsideTriangles = trapezoid.Triangles;
        }

        private void CalculateOutsideTriangles()
        {
            var trapezoid = new Polygon3d(
                OuterRight,
                OuterLeft,
                OuterLeft.Add(0, 0, Height),
                OuterRight.Add(0, 0, Height)
                );

            OutsideTriangles = trapezoid.Triangles;
        }

        public Polygon3d[] TopTrapezoids { get; private set; }
        public Triangle3d[] TopTriangles
        {
            get
            {
                return TopTrapezoids.SelectMany(T => T.Triangles).ToArray();
            }
        }
        public Triangle3d[] BottomTriangles { get; private set; }
        public Triangle3d[] LeftTriangles { get; private set; }
        public Triangle3d[] RightTriangles { get; private set; }
        public Triangle3d[] InsideTriangles { get; private set; }
        public Triangle3d[] OutsideTriangles { get; private set; }
    }
}
