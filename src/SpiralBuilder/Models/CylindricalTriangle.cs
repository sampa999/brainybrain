using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CylindricalTriangle
    {
        public CylindricalTriangle(
            CylindricalVertex v1,
            CylindricalVertex v2,
            CylindricalVertex v3)
        {
            Vertices = new List<CylindricalVertex>
            {
                v1,
                v2,
                v3
            };
        }

        public List<CylindricalVertex> Vertices { get; set; }
    }
}
