﻿
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDKTemplate;

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
            int totalAngle = 90;
            double surfaceHeight = 10;
            double dropAmount = 0;

            var spiralBuilder = new SpiralBuilder(
                startingCenterWidth,
                surfaceWidth,
                angleStep,
                spiralRatio,
                totalAngle,
                surfaceHeight,
                dropAmount);

            Assert.AreEqual(1, spiralBuilder.Wedges.Count);
            Assert.AreEqual(12, spiralBuilder.Triangles.Count);

            var triangleObject = new TriangleObject(spiralBuilder.Triangles.ToArray());
            Assert.AreEqual(12, triangleObject.Triangles.Length);
        }
        [TestMethod]
        public void SingleWedgeSpiralSpiralRatio()
        {
            double startingCenterWidth = 100;
            double surfaceWidth = 100;
            int angleStep = 90;
            double spiralRatio = 0.9;
            int totalAngle = 90;
            double surfaceHeight = 10;
            double dropAmount = 0;

            var spiralBuilder = new SpiralBuilder(
                startingCenterWidth,
                surfaceWidth,
                angleStep,
                spiralRatio,
                totalAngle,
                surfaceHeight,
                dropAmount);

            Assert.AreEqual(1, spiralBuilder.Wedges.Count);
            Assert.AreEqual(12, spiralBuilder.Triangles.Count);

            var triangleObject = new TriangleObject(spiralBuilder.Triangles.ToArray());
            Assert.AreEqual(12, triangleObject.Triangles.Length);
        }
    }
}