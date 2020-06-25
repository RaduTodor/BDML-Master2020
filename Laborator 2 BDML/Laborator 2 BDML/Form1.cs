using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laborator_2_BDML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            dataSetList = new List<double[]>();
            colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Gray, Color.Yellow, Color.Black, Color.Indigo, Color.Crimson, Color.Chocolate, Color.DarkSalmon };
            InitializeComponent();
        }

        private List<double[]> dataSetList;
        private Color[] colors;

        private int numClasses = 3;
        private int k = 2;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Random objRandom = new Random();
            dataSetList = new List<double[]>();
            for (int i = 0; i < (int)numericUpDown1.Value; i++)
            {
                double[] point = new double[3];
                for (int j = 0; j < 2; j++)
                {
                    point[j] = KNearestNeighboursManager.GenerateRandomDouble(objRandom, 0, 400);
                }
                point[2] = i%numClasses;
                dataSetList.Add(point);
            }

            picImage.Invalidate();
        }

        private void picImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (double[] point in dataSetList)
            {
                g.DrawEllipse(new Pen(colors[Convert.ToInt32(point[2])], 2.0f), (float)point[0], (float)point[1], 10, 10);
            }
        }


        private void picImage_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                double predicted = KNearestNeighboursManager.Classify(new double[] { Convert.ToDouble(me.X - 7), Convert.ToDouble(me.Y - 7) }, dataSetList.ToArray(), numClasses, k);
                dataSetList.Add(new double[] { Convert.ToDouble(me.X - 7), Convert.ToDouble(me.Y - 7), predicted });
            }
            else if (me.Button == MouseButtons.Right)
            {
                var poz = dataSetList.FindIndex(c => c[0] <= me.X + 8 && c[0] >= me.X - 8 && c[1] <= me.Y + 8 && c[1] >= me.Y - 8);
                if (poz != -1)
                    dataSetList.RemoveAt(poz);
            }

            picImage.Invalidate();
        }


        private void event_keyDown(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                List<int> errors = new List<int>();
                for (int i = 1; i < dataSetList.Count; i++)
                {
                    errors.Add(0);
                    foreach(var point in dataSetList)
                    {
                        var value = KNearestNeighboursManager.Classify(new double[] { point[0], point[1] }, dataSetList.ToArray(), numClasses, i);
                        if (value != point[2])
                        {
                            errors[i-1]++;
                        }
                    }
                }
                this.label2.Text += errors.IndexOf(errors.Min())+1;
            }

            picImage.Invalidate();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
