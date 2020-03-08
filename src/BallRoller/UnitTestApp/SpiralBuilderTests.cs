
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelBuilder;
using Models;

namespace UnitTestApp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SingleWedgeSpiral()
        {
            double startingCenterWidth = 100;
            double surfaceWidth = 100;
            int angleStep = 90;
            double spiralRatio = 1.0;
            double spiralDelta = 0;
            int totalAngle = 90;
            double surfaceHeight = 10;
            int surfaceTiltAngle = 0;
            double dropAmount = 0;

            var spiralBuilder = new SpiralBuilder(
                startingCenterWidth,
                surfaceWidth,
                angleStep,
                spiralRatio,
                spiralDelta,
                totalAngle,
                surfaceHeight,
                surfaceTiltAngle,
                dropAmount);

            spiralBuilder.CalculateWedges();
            Assert.AreEqual(1, spiralBuilder.Wedges.Count);

            spiralBuilder.ExtractTriangles();
            Assert.AreEqual(42, spiralBuilder.Triangles.Count);

            var triangleObject = new TriangleObject(spiralBuilder.Triangles.ToArray());
            Assert.AreEqual(42, triangleObject.Triangles.Length);
        }
        [TestMethod]
        public void SingleWedgeSpiralSpiralRatio()
        {
            double startingCenterWidth = 100;
            double surfaceWidth = 100;
            int angleStep = 90;
            double spiralRatio = 0.9;
            double spiralDelta = 0;
            int totalAngle = 90;
            double surfaceHeight = 10;
            int surfaceTiltAngle = 0;
            double dropAmount = 0;

            var spiralBuilder = new SpiralBuilder(
                startingCenterWidth,
                surfaceWidth,
                angleStep,
                spiralRatio,
                spiralDelta,
                totalAngle,
                surfaceHeight,
                surfaceTiltAngle,
                dropAmount);

            spiralBuilder.CalculateWedges();
            Assert.AreEqual(1, spiralBuilder.Wedges.Count);

            spiralBuilder.ExtractTriangles();
            Assert.AreEqual(42, spiralBuilder.Triangles.Count);

            var triangleObject = new TriangleObject(spiralBuilder.Triangles.ToArray());
            Assert.AreEqual(42, triangleObject.Triangles.Length);
        }
    }
}
