using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace polyAngles
{
    class Circle : Shape
    {
        int diameter = radius * 2;

        public Circle(int x, int y)
        {
            x0 = x;
            y0 = y;

        }
        public override bool IsInside(int clickedX, int clickedY)
        {
            double distance = Math.Sqrt(Math.Pow(clickedX - x0, 2) + Math.Pow(clickedY - y0, 2));

            return distance < radius;
        }

        public override void Draw(Graphics g)
        {
            g.DrawEllipse(shapePen, x0 - radius, y0 - radius, diameter, diameter);
        }
    }
}
