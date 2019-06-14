using System;
using System.Collections.Generic;
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

            CalculateWedges();
            ExtractTriangles();
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

        private void ExtractTriangles()
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
        }

        private void CalculateWedges()
        {
            Wedges = new List<Wedge>();

            double currentHeight = 0;
            double heightStepPerAngleStep = DropAmount * AngleStepDegrees / (double) 360.0;
            var centerRadius = StartingCenterRadius;
            var innerLeft = new Point3d (
                centerRadius * Math.Sin(0),
                centerRadius * Math.Cos(0),
                currentHeight);
            var outerLeft = new Point3d(
                (centerRadius + SurfaceWidth) * Math.Sin(0),
                (centerRadius + SurfaceWidth) * Math.Cos(0),
                currentHeight);

            for (var angle=AngleStepDegrees; angle <= TotalAngleDegrees; angle += AngleStepDegrees)
            {
                centerRadius -= SpiralDelta;
                centerRadius *= SpiralRatio;
                currentHeight -= heightStepPerAngleStep;
                var innerRight = new Point3d(
                    centerRadius * Math.Sin((double) angle * Math.PI / 180.0),
                    centerRadius * Math.Cos((double) angle * Math.PI / 180.0),
                    currentHeight);
                var outerRight = new Point3d(
                    (centerRadius + SurfaceWidth) * Math.Sin((double)angle * Math.PI / 180.0),
                    (centerRadius + SurfaceWidth) * Math.Cos((double)angle * Math.PI / 180.0),
                    currentHeight);

                Wedges.Add(new Wedge(innerLeft, innerRight, outerLeft, outerRight, SurfaceHeight));
                outerLeft = outerRight;
                innerLeft = innerRight;
            }
        }

        public List<Wedge> Wedges;
        public List<Triangle3d> Triangles { get; private set; }
    }
}
