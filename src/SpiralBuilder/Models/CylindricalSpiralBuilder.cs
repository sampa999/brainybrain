using System;
using System.Collections.Generic;
using System.Text;

namespace _Models
{
    public class CylindricalSpiralBuilder
    {
        private double StartingCenterRadius { get; set; }
        private int AngleStepDegrees { get; set; }
        private double SurfaceWidth { get; set; }
        private double SpiralRatio { get; set; }
        private double SpiralDelta { get; set; }
        private int TotalAngleDegrees { get; set; }
        private double SurfaceHeight { get; set; }
        private double SurfaceTiltAngle { get; set; }
        private double DropAmount { get; set; }

        public CylindricalSpiralBuilder(
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
            SpiralDelta = fullSpiralDelta * angleStep / (double)360.0;
            TotalAngleDegrees = totalAngle;
            SurfaceHeight = surfaceHeight;
            SurfaceTiltAngle = surfaceTiltAngle;
            DropAmount = dropAmount;
        }

        private double CalculateStepSpiralRatio(double fullSpiralRatio)
        {
            const double AllowedError = 0.00000001;

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
    }
}
