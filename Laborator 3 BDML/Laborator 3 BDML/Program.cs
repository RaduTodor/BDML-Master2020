using System;
using System.Collections.Generic;

namespace Laborator_3_BDML
{
    class Program
    {
        public const int N = 50;
        public const int k = 2;
        public static void Main(string[] args)
        {
            Random objRandom = new Random();
            var dataSetList = new List<double[]>();
            for (int i = 0; i < N; i++)
            {
                double[] point = new double[2];
                for (int j = 0; j < 2; j++)
                {
                    point[j] = GenerateRandomDouble(objRandom, 0, 400);
                }
                dataSetList.Add(point);
            }

            var submultimi = new List<List<double[]>>();
            for (int i = 0; i < N / k; i++)
            {
                submultimi.Add(new List<double[]>());
                for (int j = 0; j < k; j++)
                {
                    var index = objRandom.Next(0, dataSetList.Count);
                    submultimi[i].Add(dataSetList[index]);
                    dataSetList.RemoveAt(index);
                }
            }
            //RandomTree()
            Console.WriteLine(submultimi);
        }

        public static double GenerateRandomDouble(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
