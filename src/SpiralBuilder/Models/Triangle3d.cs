namespace Models
{
    public class Triangle3d
    {
        public Point3d[] Vertices;
        private Triangle3d()
        {
        }

        public Triangle3d(Point3d v0, Point3d v1, Point3d v2) : base()
        {
            // (0,100,10), (0,120,10), (0,120,0),
            Vertices = new Point3d[3];

            Vertices[0] = v0;
            Vertices[1] = v1;
            Vertices[2] = v2;
        }
    }
}
