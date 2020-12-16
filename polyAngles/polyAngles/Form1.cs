using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace polyAngles
{
    public partial class Form1 : Form
    {
        public List<Shape> pointsList = new List<Shape>();

        bool isInside = false;

        int res = 1;

        readonly Pen linePen = new Pen(Color.FromArgb(0, 51, 88), 2);

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }


        public void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Shape point in pointsList)
            {
                point.isConnected = false;
                point.Draw(e.Graphics);
            }

            if (pointsList.Count > 2)
            {
                // DrawLineDefinition(e.Graphics);
                JarvisMarch(e.Graphics);
            }
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int clickedX = e.X;
            int clickedY = e.Y;

            Shape[] pointsArray = pointsList.ToArray();


            for (int i = 0; i < pointsList.Count; i++)
            {
                if (pointsArray[i].IsInside(clickedX, clickedY))
                {
                    pointsArray[i].isClicked = true;

                    if (e.Button == MouseButtons.Left)
                    {
                        isInside = true;

                        pointsArray[i].dx = clickedX - pointsArray[i].x0;
                        pointsArray[i].dy = clickedY - pointsArray[i].y0;

                        // break;
                    }

                    else
                    {
                        pointsList.RemoveAt(i);
                        if (isInside) isInside = false;

                        i -= 1;
                        pointsArray = pointsList.ToArray();
                    }
                }
             }

            if (!isInside && e.Button == MouseButtons.Left)
            {
                if (res == 1)
                {
                    pointsList.Add(new Circle(clickedX, clickedY));
                }
                else if (res == 2)
                {
                    pointsList.Add(new Square(clickedX, clickedY));
                }
                else if (res == 3)
                {
                    pointsList.Add(new Triangle(clickedX, clickedY));
                }
            }

            Refresh();
        }

        

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInside)
            {
                foreach (Shape vertex in pointsList)
                {
                    if (vertex.isClicked)
                    {
                        vertex.x0 = e.X - vertex.dx;
                        vertex.y0 = e.Y - vertex.dy;

                        Refresh();
                    }
                }
            }
        }


        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

            for (int i = 0; i < pointsList.Count; i++)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (pointsList[i].isClicked)
                    {
                        pointsList[i].isClicked = false;
                        isInside = false;
                    }

                    if (pointsList.Count > 3)
                    {
                        if (!pointsList[i].isConnected)
                        {
                            pointsList.RemoveAt(i);
                            Refresh();
                            i -= 1;
                        }

                        else pointsList[i].isConnected = false;
                    }
                }
            }
        }


        private void DrawLineDefinition(Graphics g)
        {
            Shape[] pointsArray = pointsList.ToArray();

            Point firstPoint;
            Point secondPoint;

            double k;
            double b;
            int isLess;
            int isMore;

            bool isGoodPair;

            for (int i = 0; i < pointsArray.Length; i++)
            {
                firstPoint = new Point(pointsArray[i].x0, pointsArray[i].y0);

                for (int j = i + 1; j < pointsArray.Length; j++)
                {
                    secondPoint = new Point(pointsArray[j].x0, pointsArray[j].y0);

                    isLess = 0;
                    isMore = 0;
                    isGoodPair = true;

                    if (firstPoint.X == secondPoint.X)
                    {
                        for (int z = 0; z < pointsArray.Length; z++)
                        {
                            if (z != i && z != j)
                            {
                                if (pointsArray[z].x0 > firstPoint.X) isMore += 1;

                                else isLess += 1;

                                if (!(isMore == 0 || isLess == 0))
                                {
                                    isGoodPair = false;
                                    break;
                                }
                            }
                        }
                    }

                    else
                    {
                        k = Convert.ToDouble(firstPoint.Y - secondPoint.Y) / Convert.ToDouble(firstPoint.X - secondPoint.X);
                        b = - Convert.ToDouble(firstPoint.Y) + k * Convert.ToDouble(firstPoint.X);

                        for (int z = 0; z < pointsArray.Length; z++)
                        {
                            if (z != i && z != j)
                            {
                                if (k * pointsArray[z].x0 - b > pointsArray[z].y0) isMore += 1;

                                else isLess += 1;

                                if (!(isMore == 0 || isLess == 0))
                                {
                                    isGoodPair = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (isGoodPair)
                    {
                        g.DrawLine(linePen, firstPoint, secondPoint);
                        pointsArray[i].isConnected = true;
                        pointsArray[j].isConnected = true;
                    }
                }
            }

            pointsList = pointsArray.ToList<Shape>();

        }

        private double VectorLength(Point vector)  // вспомогательный метод для алгоритма Джарвиса: определяем длину вектора
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        private double CosAngle(Point vector1, double vector1Length, Point vector2)  // вспомогательный метод для алгоритма Джарвиса: ищем косинус угла между двумя векторами
        {   
            return (vector1.X * vector2.X + vector1.Y * vector2.Y) / (vector1Length * VectorLength(vector2));
        }

        private void JarvisMarch(Graphics g)
        {
            Shape[] pointsArray = pointsList.ToArray();

            int startingIndex = 0;
            
            for (int i = 1; i < pointsArray.Length; i++) // ищем подходящую точку A для начала
            {
                if (pointsArray[i].y0 > pointsArray[startingIndex].y0 ||
                    (pointsArray[i].y0 == pointsArray[startingIndex].y0 && pointsArray[i].x0 < pointsArray[startingIndex].x0))
                {
                    startingIndex = i;
                }
            }
            int pointAIndex = startingIndex;
            pointsArray[startingIndex].isConnected = true;

            int pointBIndex = pointsArray.Length;

            Point vector1;
            Point vector2;
            double vectorLength;

            double minLength = 1000;
            double currentCosine;
            double minCosine = 2;


            for (int j = 0; j < pointsArray.Length; j++) // найдём такую точку В, чтобы тупой угол, который АВ образует с осью Х был максимальным
            {
                if (!pointsArray[j].isConnected)
                {
                    vector1 = new Point(pointsArray[j].x0 - pointsArray[pointAIndex].x0, pointsArray[j].y0 - pointsArray[pointAIndex].y0);
                    vector2 = new Point(-10, 0);
                    vectorLength = VectorLength(vector1);

                    currentCosine = CosAngle(vector1, vectorLength, vector2);

                    if (currentCosine < minCosine)
                    {
                        pointBIndex = j;
                        minCosine = currentCosine;
                        minLength = vectorLength;
                    }

                    else if (currentCosine == minCosine && vectorLength < minLength)
                    {
                        pointBIndex = j;
                        minLength = vectorLength;
                    }
                }
            }

            pointsArray[pointBIndex].isConnected = true;
            int pointCIndex;

            Point startingVector;
            double startingCosine;

            while (pointAIndex != pointBIndex) // Имея точки А и С ищем такую точку В, чтобы угол САВ был максимальным
            {
                g.DrawLine(linePen, new Point(pointsArray[pointAIndex].x0, pointsArray[pointAIndex].y0),
                        new Point(pointsArray[pointBIndex].x0, pointsArray[pointBIndex].y0));

                pointCIndex = pointAIndex;
                pointAIndex = pointBIndex;

                minCosine = 2;
                minLength = 1000;

               
                for (int j = 0; j < pointsArray.Length; j++)
                {
                    if (!pointsArray[j].isConnected)
                    {
                        vector1 = new Point(pointsArray[j].x0 - pointsArray[pointAIndex].x0, pointsArray[j].y0 - pointsArray[pointAIndex].y0);
                        vector2 = new Point(pointsArray[pointCIndex].x0 - pointsArray[pointAIndex].x0, pointsArray[pointCIndex].y0 - pointsArray[pointAIndex].y0);
                        vectorLength = VectorLength(vector1);

                        currentCosine = CosAngle(vector1, vectorLength, vector2);

                        startingVector = new Point(pointsArray[startingIndex].x0 - pointsArray[pointAIndex].x0, pointsArray[startingIndex].y0 - pointsArray[pointAIndex].y0);
                        startingCosine = CosAngle(startingVector, VectorLength(startingVector), vector2);

                        if (currentCosine > startingCosine) continue;

                        if (currentCosine < minCosine)
                        {
                            pointBIndex = j;
                            minCosine = currentCosine;
                            minLength = vectorLength;
                        }

                        else if (currentCosine == minCosine && vectorLength < minLength)
                        {
                            pointBIndex = j;
                            minLength = vectorLength;
                        }
                    }
                }

                pointsArray[pointBIndex].isConnected = true;
            }

            g.DrawLine(linePen, new Point(pointsArray[pointAIndex].x0, pointsArray[pointAIndex].y0),
                                        new Point(pointsArray[startingIndex].x0, pointsArray[startingIndex].y0));

            pointsList = pointsArray.ToList<Shape>();
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            res = 1;
        }

        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            res = 2;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            res = 3;
        }
    }
}
