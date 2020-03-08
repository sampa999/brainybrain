using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CylindricalWedge
    {
        private readonly int deltaPhi;
        private readonly double deltaZ;
        private readonly double deltaRadius;

        public CylindricalWedge(
            CylindricalPolygon leftSide,
            int deltaPhi,
            double deltaZ,
            double deltaRadius)
        {
            this.deltaPhi = deltaPhi;
            this.deltaZ = deltaZ;
            this.deltaRadius = deltaRadius;
            LeftSide = leftSide;

            CalculateSides();
        }

        private void CalculateSides()
        {
            RightSide = CalculateRightSide();
            TopSide = CalculateTopSide();
            BottomSide = CalculateBottomSide();
            InSide = CalculateInSide();
            OutSide = CalculateOutSide();
        }

        /// <summary>
        /// Right side is Left side adjusted by deltaPhi, deltaZ, and deltaRadius.
        /// The new vertices are in opposite order from Left side to keep normalization correct.
        /// </summary>
        /// <returns></returns>
        private CylindricalPolygon CalculateRightSide()
        {
            var vertices = new List<CylindricalVertex>();
            foreach (var v in LeftSide.Vertices)
            {
                vertices.Add(
                    new CylindricalVertex(
                        v.Radius + deltaRadius,
                        v.Phi + deltaPhi,
                        v.Z + deltaZ
                    ));
            }

            vertices.Reverse();
            var side = new CylindricalPolygon(vertices);
            return side;
        }

        /// <summary>
        /// Top side is created from Left side and Right side
        /// </summary>
        /// <returns></returns>
        private CylindricalPolygon CalculateTopSide()
        {
            var vertices = new List<CylindricalVertex>
            {
                LeftSide.Vertices[1],
                RightSide.Vertices[3],
                RightSide.Vertices[2],
                LeftSide.Vertices[2]
            };

            var side = new CylindricalPolygon(vertices);
            return side;
        }

        /// <summary>
        /// Bottom side is created from Top side, adjusted down by the height of Left side
        /// 
        /// </summary>
        /// <returns></returns>
        private CylindricalPolygon CalculateBottomSide()
        {
            var height = LeftSide.Vertices[1].Z - LeftSide.Vertices[0].Z;
            var vertices = new List<CylindricalVertex>();
            foreach (var v in TopSide.Vertices)
            {
                vertices.Add(
                    new CylindricalVertex(
                        v.Radius,
                        v.Phi,
                        v.Z - height
                    ));
            }

            vertices.Reverse();
            var side = new CylindricalPolygon(vertices);
            return side;
        }

        /// <summary>
        /// In side is created from Left side and Right side
        /// </summary>
        /// <returns></returns>
        private CylindricalPolygon CalculateInSide()
        {
            var vertices = new List<CylindricalVertex>
            {
                LeftSide.Vertices[0],
                RightSide.Vertices[3],
                RightSide.Vertices[2],
                LeftSide.Vertices[1]
            };

            var side = new CylindricalPolygon(vertices);
            return side;
        }

        /// <summary>
        /// Out side is created from In side, adjusted out by the depth of Left side
        /// </summary>
        /// <returns></returns>
        private CylindricalPolygon CalculateOutSide()
        {
            var depth = LeftSide.Vertices[2].Radius - LeftSide.Vertices[1].Radius;
            var vertices = new List<CylindricalVertex>();
            foreach (var v in InSide.Vertices)
            {
                vertices.Add(
                    new CylindricalVertex(
                        v.Radius + depth,
                        v.Phi,
                        v.Z
                    ));
            }

            vertices.Reverse();
            var side = new CylindricalPolygon(vertices);
            return side;
        }

        public CylindricalPolygon LeftSide { get; private set; }
        public CylindricalPolygon RightSide { get; private set; }
        public CylindricalPolygon TopSide { get; private set; }
        public CylindricalPolygon BottomSide { get; private set; }
        public CylindricalPolygon InSide { get; private set; }
        public CylindricalPolygon OutSide { get; private set; }
    }
}
