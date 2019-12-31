using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// You pass in a polygon at the start. That polygon is used to sweep
    /// through space creating a 'ribbon' which is used as the basis of the object.
    /// It is essential that the bottom of the polygon be flat. It is okay for it to 
    /// be tilted, but it must be a line.
    /// </summary>
    public class CylindricalSpiralBuilder
    {
        private double StartingCenterRadius { get; set; }
        private int AngleStepDegrees { get; set; }
        private double SpiralRatio { get; set; }
        private double SpiralDelta { get; set; }
        private int TotalAngleDegrees { get; set; }
        private double RiseAmountPerRotation { get; set; }
        private CylindricalPolygon Profile { get; set; }

        public CylindricalSpiralBuilder(
            CylindricalPolygon profile,
            double startingCenterRadius,
            int angleStep,
            double fullSpiralRatio,
            double fullSpiralDelta,
            int totalAngle,
            double riseAmountPerRotation)
        {
            Profile = profile;
            StartingCenterRadius = startingCenterRadius;
            AngleStepDegrees = angleStep;
            SpiralRatio = CalculateStepSpiralRatio(fullSpiralRatio);
            SpiralDelta = fullSpiralDelta * angleStep / (double)360.0;
            TotalAngleDegrees = totalAngle;
            RiseAmountPerRotation = riseAmountPerRotation;
        }

        public void CreateRibbonPolygons()
        {
            RibbonPolygons = new List<CylindricalPolygon>();

            var centerRadius = StartingCenterRadius;
            var currentHeight = 0.0;
            var riseStep = RiseAmountPerRotation * AngleStepDegrees / 360.0;

            for (var currentAngle = 0; currentAngle <= TotalAngleDegrees; currentAngle += AngleStepDegrees)
            {
                var offset = new CylindricalVertex(centerRadius, currentAngle, currentHeight);
                RibbonPolygons.Add(CylindricalPolygon.Translate(Profile, offset));
                currentHeight += riseStep;
                centerRadius += SpiralDelta;
                centerRadius *= SpiralRatio;
            }
        }

        public void ExtractTriangles()
        {
            // First, get the left side
            // Then the inside, top, outside, and bottoms
            // Then get the right side

            cylindricalTriangles = new List<CylindricalTriangle>();

            // Left
            var first = RibbonPolygons.First();
            cylindricalTriangles.AddRange(first.Triangles);

            for (var i = 0; i < RibbonPolygons.Count - 1; i++)
            {
                var left = RibbonPolygons[i];
                var right = RibbonPolygons[i + 1];

                var inside = new CylindricalPolygon(
                    new List<CylindricalVertex>
                    {
                        left.Vertices[0],
                        right.Vertices[0],
                        right.Vertices[1],
                        left.Vertices[1],
                    });
                cylindricalTriangles.AddRange(inside.Triangles);

                var top = new CylindricalPolygon(
                    new List<CylindricalVertex>
                    {
                        right.Vertices[1],
                        right.Vertices[2],
                        left.Vertices[2],
                        left.Vertices[1],
                    });
                cylindricalTriangles.AddRange(top.Triangles);

                var outside = new CylindricalPolygon(
                    new List<CylindricalVertex>
                    {
                        left.Vertices[2],
                        right.Vertices[2],
                        right.Vertices[3],
                        left.Vertices[3],
                    });
                cylindricalTriangles.AddRange(outside.Triangles);

                var bottom = new CylindricalPolygon(
                    new List<CylindricalVertex>
                    {
                        left.Vertices[0],
                        left.Vertices[3],
                        right.Vertices[3],
                        right.Vertices[0],
                    });
                cylindricalTriangles.AddRange(bottom.Triangles);
            }

            var last = RibbonPolygons.Last();
            CylindricalPolygon.FlipNormal(last);
            cylindricalTriangles.AddRange(last.Triangles);
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

        private List<CylindricalTriangle> cylindricalTriangles { get; set; }

        public List<Triangle3d> Triangles
        {
            get
            {
                var triangle3dList = new List<Triangle3d>();

                foreach (var ctri in cylindricalTriangles)
                {
                    triangle3dList.Add(ctri.ToTriangle3d());
                }

                return triangle3dList;
            }
        }

        private List<CylindricalPolygon> RibbonPolygons { get; set; }

    }
}
