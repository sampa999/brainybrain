using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using FluentAssertions;

namespace ModelTester
{
    [TestClass]
    public class PolygonTests
    {
        [TestMethod]
        public void CreatePolygonOfSingleTriangle()
        {
            var p = new Polygon(new Vertex(0, 0, 0), new Vertex(0, 1, 0), new Vertex(1,0,0));
            var t = p.Triangles;
            t.Length.Should().Be(1);
        }

        [TestMethod]
        public void CreatePolygonOfSingleSquare()
        {
            var p = new Polygon(new Vertex(0, 0, 0), new Vertex(0, 1, 0), new Vertex(1,1,0), new Vertex(1,0,0));
            var t = p.Triangles;
            t.Length.Should().Be(2);
        }
    }
}
