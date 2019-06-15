namespace Models
{
    public class Triangle3d
    {
        public Vertex[] Vertices;
        private Triangle3d()
        {
        }

        public Triangle3d(Vertex v0, Vertex v1, Vertex v2) : base()
        {
            // (0,100,10), (0,120,10), (0,120,0),
            Vertices = new Vertex[3];

            Vertices[0] = v0;
            Vertices[1] = v1;
            Vertices[2] = v2;
        }
    }
}
