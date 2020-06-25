using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laborator_1_BDML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            dataSetList = new List<double[]>();
            centroidList = new List<double[]>();
            clustering = new List<int>();
            colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Gray, Color.Yellow, Color.Black, Color.Indigo, Color.Crimson, Color.Chocolate, Color.DarkSalmon };
            InitializeComponent();
        }

        private List<double[]> dataSetList;
        private List<double[]> centroidList;
        private List<int> clustering;
        private Color[] colors;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Random objRandom = new Random();
            dataSetList = new List<double[]>();
            for (int i = 0; i < (int)numericUpDown1.Value; i++)
            {
                double[] point = new double[2];
                for (int j = 0; j < 2; j++)
                {
                    point[j] = KMeansManager.GenerateRandomDouble(objRandom, 0, 400);
                }
                dataSetList.Add(point);
            }

            if (dataSetList.Count > 0 && centroidList.Count > 0)
            {
                clustering = KMeansManager.Cluster(dataSetList.ToArray(), centroidList.Count, centroidList.ToArray()).ToList();
            }

            picImage.Invalidate();
        }

        private void picImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (double[] point in dataSetList)
            {
                g.DrawEllipse(new Pen(Color.Gray, 2.0f), (float)point[0], (float)point[1], 10, 10);
            }
            foreach (double[] point in centroidList)
            {
                g.DrawEllipse(new Pen(Color.Red, 2.0f), (float)point[0], (float)point[1], 15, 15);
            }
            if (dataSetList.Count > 0 && centroidList.Count > 0)
            {
                for (int i = 0; i < clustering.Count; i++)
                {
                    g.DrawLine(new Pen(colors[clustering[i]], 1.0f), (float)dataSetList.ElementAt(i)[0], (float)dataSetList.ElementAt(i)[1], (float)centroidList.ElementAt(clustering[i])[0], (float)centroidList.ElementAt(clustering[i])[1]);
                }
            }
        }


        private void picImage_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                if (centroidList.Count < colors.Length)
                    centroidList.Add(new double[] { Convert.ToDouble(me.X - 7), Convert.ToDouble(me.Y - 7) });
            }
            else if (me.Button == MouseButtons.Right)
            {
                var poz = centroidList.FindIndex(c => c[0] <= me.X + 8 && c[0] >= me.X - 8 && c[1] <= me.Y + 8 && c[1] >= me.Y - 8);
                if (poz != -1)
                    centroidList.RemoveAt(poz);
            }

            if (dataSetList.Count > 0 && centroidList.Count > 0)
            {
                clustering = KMeansManager.Cluster(dataSetList.ToArray(), centroidList.Count, centroidList.ToArray()).ToList();
            }

            picImage.Invalidate();
        }


        private void event_keyDown(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                for (int i = 0; i < centroidList.Count; i++)
                {
                    var list = dataSetList.Where(d => clustering[dataSetList.IndexOf(d)] == i);
                    centroidList.ElementAt(i)[0] = list.Average(d => d[0]);
                    centroidList.ElementAt(i)[1] = list.Average(d => d[1]);
                }
            }

            picImage.Invalidate();
        }
    }
}
