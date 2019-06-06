using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTemplate
{
    public class SpiralBuilder
    {
        public const double AllowedError = 0.00000001;

        private double StartingCenterWidthMM { get; set; }
        private int AngleStepDegrees { get; set; }
        private double SurfaceWidthMM { get; set; }
        private double SpiralRatio { get; set; }
        private int TotalAngleDegrees { get; set; }
        private double SurfaceHeightMM { get; set; }
        private double DropAmountMM { get; set; }
        /// <summary>
        /// This creates a 3d model of a spiral ramp
        /// </summary>
        /// <param name="startingCenterWidth">Distance from center of ramp to inner edge of platform</param>
        /// <param name="surfaceWidth">Width of platform</param>
        /// <param name="angleStep">Degrees to move for each step in the spiral. Smaller is smoother.</param>
        /// <param name="spiralRatio">Amount to shrink the center 'hole' per 360 </param>
        /// <param name="totalAngle">Total amount to spiral. 360 is a full circle around.</param>
        /// <param name="surfaceHeight">Height of the surface ramp</param>
        /// <param name="dropAmount">Amount to drop the ramp per full revolution</param>
        public SpiralBuilder(
            double startingCenterWidth,
            double surfaceWidth,
            int angleStep,
            double fullSpiralRatio,
            int totalAngle,
            double surfaceHeight,
            double dropAmount)
        {
            StartingCenterWidthMM = startingCenterWidth;
            SurfaceWidthMM = surfaceWidth;
            AngleStepDegrees = angleStep;
            SpiralRatio = CalculateStepSpiralRatio(fullSpiralRatio);
            TotalAngleDegrees = totalAngle;
            SurfaceHeightMM = surfaceHeight;
            DropAmountMM = dropAmount;

            CalculateWedges();
            ExtractTriangles();
        }

        private double CalculateStepSpiralRatio(double fullSpiralRatio)
        {
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
            double heightStepPerAngleStep = DropAmountMM * AngleStepDegrees / (double) 360.0;
            var centerWidthMM = StartingCenterWidthMM;
            var innerLeft = new Point3d (
                centerWidthMM * Math.Sin(0),
                centerWidthMM * Math.Cos(0),
                currentHeight);
            var outerLeft = new Point3d(
                (centerWidthMM + SurfaceWidthMM) * Math.Sin(0),
                (centerWidthMM + SurfaceWidthMM) * Math.Cos(0),
                currentHeight);

            for (var angle=AngleStepDegrees; angle <= TotalAngleDegrees; angle += AngleStepDegrees)
            {
                centerWidthMM *= SpiralRatio;
                currentHeight -= heightStepPerAngleStep;
                var innerRight = new Point3d(
                    centerWidthMM * Math.Sin((double) angle * Math.PI / 180.0),
                    centerWidthMM * Math.Cos((double) angle * Math.PI / 180.0),
                    currentHeight);
                var outerRight = new Point3d(
                    (centerWidthMM + SurfaceWidthMM) * Math.Sin((double)angle * Math.PI / 180.0),
                    (centerWidthMM + SurfaceWidthMM) * Math.Cos((double)angle * Math.PI / 180.0),
                    currentHeight);

                Wedges.Add(new Wedge(innerLeft, innerRight, outerLeft, outerRight, SurfaceHeightMM));
                outerLeft = outerRight;
                innerLeft = innerRight;
            }
        }

        public List<Wedge> Wedges;
        public List<Triangle3d> Triangles { get; private set; }
    }
}
