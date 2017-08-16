// RTT Sample code in C#.NET and GDI+
//
// Originally written in Python by Steven LaValle (2011)
// Ported by Michel Michels

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTT
{
    public partial class Form1 : Form
    {
        // Constants
        private int windowHeight;
        private int windowWidth;
        private PointF rightBottomCorner;
        private double epsilon = 6.0;
        private int numNodes = 5000;
        private List<PointF> nodes;
        private Random rand;

        public Form1()
        {
            // Set variables
            nodes = new List<PointF>();
            rand = new Random();

            // Set background color
            this.BackColor = Color.Black;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Initialize gui
            InitializeComponent();
            this.Text = "Steven LaValle RRT port to C# and GDI+ by Michel Michels";

            windowHeight = this.Height;
            windowWidth = this.Width;
            rightBottomCorner = new PointF(windowWidth, windowHeight);

            // Start in left corner
            nodes.Add(new PointF(windowWidth / 2.0f, windowHeight / 2.0f));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Get graphics
            var g = e.Graphics;

            var pen = new Pen(Color.White, 1.0f);

            for (int i = 0; i < nodes.Count - 2; i += 2)
            {
                if (nodes.Count < 1) return;
                g.DrawLine(pen, nodes[i + 1], nodes[i + 2]);
            }

            //if (nodes.Count < 2) return;
            //g.DrawLine(pen, nodes[nodes.Count - 2], nodes[nodes.Count - 1]);

            base.OnPaint(e);
        }

        private double GetDistance(PointF p0, PointF p1)
        {
            // distance between 2 points = (x1 - x2)^2 + (y1 - y2)^2
            return Math.Sqrt((p0.X - p1.X) * (p0.X - p1.X) + (p0.Y - p1.Y) * (p0.Y - p1.Y));
        }

        private PointF StepFromPointToPoint(PointF p0, PointF p1)
        {
            if (GetDistance(p0, p1) < epsilon)
            {
                return p1;
            }
            else
            {
                var theta = Math.Atan2(p1.Y - p0.Y, p1.X - p0.X);
                return new PointF((float)(p0.X + epsilon * Math.Cos(theta)), (float)(p0.Y + epsilon * Math.Sin(theta)));
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numNodes; i++)
            {
                var randomPoint = new PointF((float)(rand.NextDouble() * windowWidth), (float)(rand.NextDouble() * windowHeight));

                var startNode = nodes[0];
                foreach (var node in nodes)
                {
                    if (GetDistance(node, randomPoint) < GetDistance(startNode, randomPoint)) startNode = node;
                }

                var newNode = StepFromPointToPoint(startNode, randomPoint);

                nodes.Add(startNode);
                nodes.Add(newNode);
            }

            this.Refresh();
        }
    }
}
