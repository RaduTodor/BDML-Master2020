using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator_2_BDML
{
    public static class KNearestNeighboursManager
    {
        public static double GenerateRandomDouble(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static double Classify(double[] unknown, double[][] trainData, int numClasses, int k)
        {
            int n = trainData.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            Array.Sort(info);
            double result = Vote(info, trainData, numClasses, k);
            //double result = VoteCuPonderi(info, trainData, numClasses, k);
            return result;
        }

        static double Distance(double[] unknown, double[] data)
        {
            if (unknown[0] == data[0] && unknown[1] == data[1])
                return Double.MaxValue;
            double sum = 0.0;
            for (int i = 0; i < unknown.Length; ++i)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }

        static double Vote(IndexAndDistance[] info, double[][] trainData, int numClasses, int k)
        {
            double[] votes = new double[numClasses];
            for (int i = 0; i < k; ++i)
            {
                int idx = info[i].idx;
                int c = (int)trainData[idx][2];
                ++votes[c];
            }
            double mostVotes = 0;
            int classWithMostVotes = 0;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }

        static double VoteCuPonderi(IndexAndDistance[] info, double[][] trainData, int numClasses, int k)
        {
            double[] votes = new double[numClasses];
            for (int i = 0; i < k; ++i)
            {
                int idx = info[i].idx;
                int c = (int)trainData[idx][2];
                votes[c] += 1 / Math.Sqrt(info[i].dist);
            }
            double mostVotes = 0;
            int classWithMostVotes = 0;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }
    }
}
