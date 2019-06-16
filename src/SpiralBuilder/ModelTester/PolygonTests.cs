using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace ModelTester
{
    [TestClass]
    public class PolygonTests
    {
        [TestMethod]
        public void CreatePolygon()
        {
            var p = new Polygon(new Vertex(0, 0, 0), new Vertex(0, 1, 0), new Vertex(0, 0, 1));
        }
    }
}
