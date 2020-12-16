using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace polyAngles
{
    public class Square : Shape
    {
        int leftX;
        int topY;
        int diameter = radius * 2;

        public Square(int x, int y)
        {
            x0 = x;
            y0 = y;
        }
        public override bool IsInside(int clickedX, int clickedY)
        {
            leftX = x0 - radius;
            topY = y0 - radius;

            return leftX < clickedX && clickedX < leftX + diameter && topY < clickedY && clickedY < topY + diameter;
            
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(shapePen, x0 - radius, y0 - radius, diameter, diameter);
        }
    }
}
