using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace polyAngles
{
    public abstract class Shape
    {
        protected static int radius = 20;
        protected static Pen shapePen;

        public int x0;
        public int y0;

        public bool isClicked = false;
        public int dx;
        public int dy;

        public bool isConnected;

        static Shape()
        {
            shapePen = new Pen(Color.FromArgb(0, 51, 88), 2);
        }

        public abstract bool IsInside(int clickedX, int clickedY);

        public abstract void Draw(Graphics g);

    }
}
