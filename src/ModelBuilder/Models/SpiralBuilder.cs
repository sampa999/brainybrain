using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Models
{
    public class SpiralBuilder
    {
        public const double AllowedError = 0.00000001;

        private double StartingCenterRadius { get; set; }
        private int AngleStepDegrees { get; set; }
        private double SurfaceWidth { get; set; }
        private double SpiralRatio { get; set; }
        private double SpiralDelta { get; set; }
        private int TotalAngleDegrees { get; set; }
        private double SurfaceHeight { get; set; }
        private double SurfaceTiltAngle { get; set; }
        private double DropAmount { get; set; }
        /// <summary>
        /// This creates a 3d model of a spiral ramp
        /// </summary>
        /// <param name="startingCenterRadius">Distance from center of ramp to inner edge of platform</param>
        /// <param name="surfaceWidth">Width of platform</param>
        /// <param name="angleStep">Degrees to move for each step in the spiral. Smaller is smoother.</param>
        /// <param name="fullSpiralRatio">Proportional amount to shrink the center 'hole' per 360 </param>
        /// <param name="fullSpiralDelta">Absolute amount to shrink the center 'hole' per 360</param>
        /// <param name="totalAngle">Total amount to spiral. 360 is a full circle around.</param>
        /// <param name="surfaceHeight">Height of the surface ramp</param>
        /// <param name="surfaceTiltAngle">Tilt of the spiral surface in degrees</param>
        /// <param name="dropAmount">Amount to drop the ramp per full revolution</param>
        public SpiralBuilder(
            double startingCenterRadius,
            double surfaceWidth,
            int angleStep,
            double fullSpiralRatio,
            double fullSpiralDelta,
            int totalAngle,
            double surfaceHeight,
            int surfaceTiltAngle,
            double dropAmount)
        {
            StartingCenterRadius = startingCenterRadius;
            SurfaceWidth = surfaceWidth;
            AngleStepDegrees = angleStep;
            SpiralRatio = CalculateStepSpiralRatio(fullSpiralRatio);
            SpiralDelta = fullSpiralDelta * angleStep / (double) 360.0;
            TotalAngleDegrees = totalAngle;
            SurfaceHeight = surfaceHeight;
            SurfaceTiltAngle = surfaceTiltAngle;
            DropAmount = dropAmount;

            Supports = new List<Polygon>();
        }

        private double CalculateStepSpiralRatio(double fullSpiralRatio)
        {
            if (fullSpiralRatio == 1.0)
            {
                return 1.0;
            }
            double lowGuess = 0;
            double highGuess = 1;
            double power = ((double)360) / AngleStepDegrees;
            double lastError;
            double currentGuess;

            if (fullSpiralRatio < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fullSpiralRatio), "must be non-negative");
            }

            do
            {
                currentGuess = (lowGuess + highGuess) / 2;
                var result = Math.Pow(currentGuess, power);
                if (result > fullSpiralRatio)
                {
                    highGuess = currentGuess;
                }
                else
                {
                    lowGuess = currentGuess;
                }
                lastError = Math.Abs(fullSpiralRatio - result);
            } while (lastError > AllowedError);

            return currentGuess;
        }

        /// <summary>
        // Remove the bottom polygon from wedge topIndexLeft, topIndexLeft+1, topIndexLeft+2
        // Remove the outside top polygon from wedge bottomIndex 
        // Add connecting polygons from outline of outside top polygon wedge bottomIndex
        // to outline of bottom polygons from wedge topIndexLeft, topIndexLeft+1, topIndexLeft+2
        /// </summary>
        /// <param name="bottomIndex"></param>
        /// <param name="topIndexLeft"></param>
        private void AddSupport(
            int bottomIndex,
            int topIndexLeft)
        {
            var bottom0 = Wedges[topIndexLeft].BottomTrapezoids[0];
            Wedges[topIndexLeft].RemoveBottom();
            var bottom1 = Wedges[topIndexLeft + 1].BottomTrapezoids[0];
            Wedges[topIndexLeft + 1].RemoveBottom();
            var bottom2 = Wedges[topIndexLeft + 2].BottomTrapezoids[0];
            Wedges[topIndexLeft + 2].RemoveBottom();
            var top = Wedges[bottomIndex].TopTrapezoids[Wedges[bottomIndex].TopTrapezoids.Length - 1];
            Wedges[bottomIndex].RemoveTopOutside();

            // Left side
            var p = new Polygon(
                top.Vertices[3],
                top.Vertices[0],
                bottom0.Vertices[1],
                bottom0.Vertices[2]);
            Supports.Add(p);
            // Right side
            p = new Polygon(
                top.Vertices[1],
                top.Vertices[2],
                bottom2.Vertices[3],
                bottom2.Vertices[0]);
            Supports.Add(p);
            // Inside Left
            p = new Polygon(
                top.Vertices[0],
                bottom0.Vertices[0],
                bottom0.Vertices[1]
                );
            Supports.Add(p);
            // Inside Middle
            p = new Polygon(
                top.Vertices[0],
                top.Vertices[1],
                bottom1.Vertices[0],
                bottom1.Vertices[1]
                );
            Supports.Add(p);
            // Inside Right
            p = new Polygon(
                top.Vertices[1],
                bottom2.Vertices[0],
                bottom2.Vertices[1]
                );
            Supports.Add(p);
            // Outside Left
            p = new Polygon(
                top.Vertices[3],
                bottom0.Vertices[2],
                bottom0.Vertices[3]
                );
            Supports.Add(p);
            // Outside Middle
            p = new Polygon(
                top.Vertices[2],
                top.Vertices[3],
                bottom1.Vertices[2],
                bottom1.Vertices[3]
                );
            Supports.Add(p);
            // Outside Right
            p = new Polygon(
                top.Vertices[2],
                bottom2.Vertices[2],
                bottom2.Vertices[3]
                );
            Supports.Add(p);
        }

        public void AddSupports()
        {
            var offset = 360 / AngleStepDegrees;
            // Remove the bottom polygon from wedge 0,1,2
            // Remove the outside top polygon from wedge offset+1
            // Add connecting polygons from outline of outside top polygon wedge offset+1
            // to outline of bottom polygons from wedge 0,1,2
            // Repeat

            var currentBottom = offset + 1;
            while (currentBottom < Wedges.Count - 1)
            {
                AddSupport(currentBottom, currentBottom - offset - 1);
                currentBottom += 3;
            }
        }

        public void AddBaseSupportBottom(
            )
        {

        }

        private Vertex RemoveSquareAtBackOfTheRightsSideOfWedge(Wedge wedge)
        {
            var rightPolygon = wedge.RightPolygons[0];
            var bottomPolygon = wedge.BottomTrapezoids[0];

            var newVert = new Vertex(
                rightPolygon.Vertices[0].X,
                rightPolygon.Vertices[0].Y,
                rightPolygon.Vertices[5].Z);

            var newRightPolygon = new Polygon(
                rightPolygon.Vertices[0],
                rightPolygon.Vertices[1],
                rightPolygon.Vertices[2],
                rightPolygon.Vertices[3],
                rightPolygon.Vertices[4],
                newVert
                );

            wedge.RightPolygons[0] = newRightPolygon;

            var newBottomPolygon = new Polygon(
                bottomPolygon.Vertices[0],
                bottomPolygon.Vertices[1],
                bottomPolygon.Vertices[2],
                bottomPolygon.Vertices[3],
                newVert
                );

            wedge.BottomTrapezoids[0] = newBottomPolygon;

            return newVert;
        }

        public void AddBaseSupports()
        {
            // Remove the square at the back of the right side of the last wedge

            var lastWedge = Wedges.Last();
            var oldRightPolygon = lastWedge.RightPolygons[0];
            var leftPolygon = lastWedge.LeftPolygons[0];
            var newVert = RemoveSquareAtBackOfTheRightsSideOfWedge(lastWedge);
            var rightPolygon = lastWedge.RightPolygons[0];
            Debug.Assert(newVert == rightPolygon.Vertices.Last());

            var dx = oldRightPolygon.Vertices[6].X - leftPolygon.Vertices[5].X;
            var dy = oldRightPolygon.Vertices[6].Y - leftPolygon.Vertices[5].Y;

            // Create the vertices that will form the new box that contains the
            // now missing section of the right polygon
            var boxVertices = new Vertex[]
            {
                newVert,
                oldRightPolygon.Vertices[5],
                oldRightPolygon.Vertices[6],
                oldRightPolygon.Vertices[0],
                newVert.Add(dx, dy, 0),
                oldRightPolygon.Vertices[5].Add(dx, dy, 0),
                oldRightPolygon.Vertices[6].Add(dx, dy, 0),
                oldRightPolygon.Vertices[0].Add(dx, dy, 0)
            };

            var innerLeftBottom = boxVertices[0];
            var outerLeftBottom = boxVertices[1];
            var outerLeftTop = boxVertices[2];
            var innerLeftTop = boxVertices[3];

            var indexOfWedgeAbove = Wedges.Count - (360 / AngleStepDegrees);

            var z = newVert.Z;

            var innerRight = new Vertex(
                Wedges[indexOfWedgeAbove].InnerRight.X,
                Wedges[indexOfWedgeAbove].InnerRight.Y,
                z);

            var outerRight = new Vertex(
                Wedges[indexOfWedgeAbove].OuterRight.X,
                Wedges[indexOfWedgeAbove].OuterRight.Y,
                z);

            baseSupports = new List<Polygon>();

            var smallWedges = new List<SmallWedge>();

            // Add the first small wedge. It's special because it has to
            // align with the last regular wedge.
            smallWedges.Add(new SmallWedge(
                innerLeftBottom,
                innerRight,
                outerRight,
                outerLeftBottom,
                SurfaceHeight,
                innerLeftTop,
                outerLeftTop));

            var maxSteps = 360 / AngleStepDegrees;
            //var maxSteps = 2;

            var surfaceHeight = SurfaceHeight;

            for (var step = 1; step < maxSteps; step++)
            {
                var currentTopWedge = Wedges[indexOfWedgeAbove + step];

                var minZ = currentTopWedge.BottomTrapezoids[0].Vertices[0].Z;

                foreach(var trap in currentTopWedge.BottomTrapezoids)
                {
                    foreach (var vert in trap.Vertices)
                    {
                        if (vert.Z < minZ)
                        {
                            minZ = vert.Z;
                        }
                    }
                }

                innerLeftBottom = innerRight;
                outerLeftBottom = outerRight;
                innerLeftTop = innerLeftBottom.Add(0, 0, surfaceHeight);
                outerLeftTop = outerLeftBottom.Add(0, 0, surfaceHeight);

                innerRight = new Vertex(
                    currentTopWedge.InnerRight.X,
                    currentTopWedge.InnerRight.Y,
                    z);

                outerRight = new Vertex(
                    currentTopWedge.OuterRight.X,
                    currentTopWedge.OuterRight.Y,
                    z);

                surfaceHeight = 0.1;

                var newWedge = new SmallWedge(
                    innerLeftBottom,
                    innerRight,
                    outerRight,
                    outerLeftBottom,
                    surfaceHeight,
                    innerLeftTop,
                    outerLeftTop);

                if (newWedge.Top[0].Vertices[0].Z >= minZ)
                {
                    break;
                }

                if (step % 2 == 1)
                {
                    // Remove the top and bottom of the newWedge and currentTopWedge, respectively
                    var newWedgeTop = newWedge.RemoveTop()[0];
                    var currentTopWedgeBottom = currentTopWedge.RemoveBottom()[0];

                    MakeConnectingPolygons(newWedgeTop, currentTopWedgeBottom);
                }

                smallWedges.Add(newWedge);
            }

            // Add left connecting polygon

            foreach (var smallWedge in smallWedges)
            {
                baseSupports.AddRange(smallWedge.Inside);
                baseSupports.AddRange(smallWedge.Outside);
                baseSupports.AddRange(smallWedge.Top);
                baseSupports.AddRange(smallWedge.Bottom);
            }

            baseSupports.AddRange(smallWedges.Last().Right);

        }

        private void MakeConnectingPolygons(Polygon bottom, Polygon top)
        {
            if (bottom.Vertices.Length != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bottom), "collection must have 4 vertices");
            }
            if (top.Vertices.Length != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bottom), "collection must have 4 vertices");
            }

            // Inside?
            baseSupports.Add(
                new Polygon(
                    bottom.Vertices[0],
                    bottom.Vertices[1],
                    top.Vertices[0],
                    top.Vertices[1]));
            // Right?
            baseSupports.Add(
                new Polygon(
                    bottom.Vertices[1],
                    bottom.Vertices[2],
                    top.Vertices[3],
                    top.Vertices[0]));
            // Outside?
            baseSupports.Add(
                new Polygon(
                    bottom.Vertices[2],
                    bottom.Vertices[3],
                    top.Vertices[2],
                    top.Vertices[3]));
            // Left?
            baseSupports.Add(
                new Polygon(
                    bottom.Vertices[3],
                    bottom.Vertices[0],
                    top.Vertices[1],
                    top.Vertices[2]));
        }

        private Vertex FindMatchingVertex(Vertex[] vertexSet, Vertex vertex)
        {
            foreach (var v in vertexSet)
            {
                if (v.X == vertex.X && v.Y == vertex.Y)
                {
                    return v;
                }
            }

            return null;
        }

        private List<Polygon> baseSupports;

        private List<Polygon> Supports;

        public void ExtractTriangles()
        {
            Triangles = new List<Triangle3d>();

            Triangles.AddRange(Wedges.First().LeftTriangles);
            foreach (var wedge in Wedges)
            {
                Triangles.AddRange(wedge.TopTriangles);
                Triangles.AddRange(wedge.BottomTriangles);
                Triangles.AddRange(wedge.InsideTriangles);
                Triangles.AddRange(wedge.OutsideTriangles);
            }
            Triangles.AddRange(Wedges.Last().RightTriangles);

            foreach (var p in Supports)
            {
                Triangles.AddRange(p.Triangles);
            }

            if (baseSupports != null)
            {
                foreach (var p in baseSupports)
                {
                    Triangles.AddRange(p.Triangles);
                }
            }
        }

        public void CalculateWedges()
        {
            Wedges = new List<Wedge>();

            double currentHeight = 0;
            double heightStepPerAngleStep = DropAmount * AngleStepDegrees / (double) 360.0;
            var centerRadius = StartingCenterRadius;
            var innerLeft = new Vertex(
                centerRadius * Math.Sin(0),
                centerRadius * Math.Cos(0),
                currentHeight);
            var outerLeft = new Vertex(
                (centerRadius + SurfaceWidth) * Math.Sin(0),
                (centerRadius + SurfaceWidth) * Math.Cos(0),
                currentHeight);

            for (var angle=AngleStepDegrees; angle <= TotalAngleDegrees; angle += AngleStepDegrees)
            {
                centerRadius -= SpiralDelta;
                centerRadius *= SpiralRatio;
                currentHeight -= heightStepPerAngleStep;
                var innerRight = new Vertex(
                    centerRadius * Math.Sin((double) angle * Math.PI / 180.0),
                    centerRadius * Math.Cos((double) angle * Math.PI / 180.0),
                    currentHeight);
                var outerRight = new Vertex(
                    (centerRadius + SurfaceWidth) * Math.Sin((double)angle * Math.PI / 180.0),
                    (centerRadius + SurfaceWidth) * Math.Cos((double)angle * Math.PI / 180.0),
                    currentHeight);

                Wedges.Add(new Wedge(innerLeft, innerRight, outerLeft, outerRight, SurfaceHeight, angle));
                outerLeft = outerRight;
                innerLeft = innerRight;
            }
        }

        public List<Wedge> Wedges;
        public List<Triangle3d> Triangles { get; private set; }
    }
}
