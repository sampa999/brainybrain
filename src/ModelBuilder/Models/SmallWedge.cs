using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class SmallWedge
    {
        public readonly Vertex InnerLeft;
        public readonly Vertex InnerRight;
        public readonly Vertex OuterLeft;
        public readonly Vertex OuterRight;
        public readonly Vertex InnerTopLeft;
        public readonly Vertex OuterTopLeft;
        public readonly Vertex InnerTopRight;
        public readonly Vertex OuterTopRight;
        public readonly double Height;

        public SmallWedge(
            Vertex innerLeft,
            Vertex innerRight,
            Vertex outerRight,
            Vertex outerLeft,
            double height,
            Vertex innerTopLeft = null,
            Vertex outerTopLeft = null)
        {
            InnerLeft = innerLeft;
            InnerRight = innerRight;
            OuterLeft = outerLeft;
            OuterRight = outerRight;
            Height = height;
            InnerTopLeft = innerTopLeft != null ? innerTopLeft : innerLeft.Add(0, 0, height);
            OuterTopLeft = outerTopLeft != null ? outerTopLeft : outerLeft.Add(0, 0, height);
            InnerTopRight = innerRight.Add(0, 0, height);
            OuterTopRight = outerRight.Add(0, 0, height);

            CreateSides();
        }

        public Polygon[] RemoveTop()
        {
            var p = Top;
            Top = new Polygon[0];
            return p;
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

        private void CreateOutside()
        {
            Outside = new Polygon[]
            {
                new Polygon(
                    OuterRight,
                    OuterLeft,
                    OuterTopLeft,
                    OuterTopRight
                    )
            };
        }

        private void CreateInside()
        {
            Inside = new Polygon[]
            {
                new Polygon(
                    InnerLeft,
                    InnerRight,
                    InnerTopRight,
                    InnerTopLeft
                    )
            };
        }

        private void CreateRight()
        {
            Right = new Polygon[]
            {
                new Polygon(
                    InnerRight,
                    OuterRight,
                    OuterTopRight,
                    InnerTopRight
                    )
            };
        }

        private void CreateTop()
        {
            Top = new Polygon[]
            {
                new Polygon(
                    InnerTopLeft,
                    InnerTopRight,
                    OuterTopRight,
                    OuterTopLeft
                    )
            };
        }

        private void CreateBottom()
        {
            Bottom = new Polygon[]
            {
                new Polygon(
                    InnerRight,
                    InnerLeft,
                    OuterLeft,
                    OuterRight
                    )
            };
        }

        private void CreateLeft()
        {
            Left = new Polygon[]
            {
                new Polygon(
                    InnerLeft,
                    InnerTopLeft,
                    OuterTopLeft,
                    OuterLeft
                    )
            };
        }

        public Polygon[] Top { get; private set; }
        public Triangle3d[] TopTriangles
        {
            get
            {
                return Top.SelectMany(T => T.Triangles).ToArray();
            }
        }

        public Polygon[] Bottom { get; private set; }
        public Triangle3d[] BottomTriangles
        {
            get
            {
                return Bottom.SelectMany(T => T.Triangles).ToArray();
            }
        }
        public Polygon[] Left { get; private set; }
        public Triangle3d[] LeftTriangles
        {
            get
            {
                return Left.SelectMany(T => T.Triangles).ToArray();
            }
        }
        public Polygon[] Right { get; private set; }
        public Triangle3d[] RightTriangles
        {
            get
            {
                return Right.SelectMany(T => T.Triangles).ToArray();
            }
        }
        public Polygon[] Inside { get; private set; }
        public Triangle3d[] InsideTriangles
        {
            get
            {
                return Inside.SelectMany(T => T.Triangles).ToArray();
            }
        }
        public Polygon[] Outside { get; private set; }
        public Triangle3d[] OutsideTriangles
        {
            get
            {
                return Outside.SelectMany(T => T.Triangles).ToArray();
            }
        }
    }
}
