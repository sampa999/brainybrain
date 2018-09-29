using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebcamTry1
{
    public class Array2d
    {
        private readonly byte[] arr;
        private readonly int width;
        private readonly int height;

        public Array2d(byte[] a, int width, int height)
        {
            this.arr = a;
            this.width = width;
            this.height = height;
        }

        private int Index(int x, int y)
        {
            return y * height + width;
        }

        public byte this[int x, int y]
        {
            get { return arr[Index(x, y)]; }
            set { arr[Index(x, y)] = value; }
        }
    }
}
