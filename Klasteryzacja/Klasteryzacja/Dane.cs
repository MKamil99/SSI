// Kamil Matula, gr. D, 27.03.2020, Klasteryzacja

using System;
using System.Collections.Generic;
using System.Linq;

namespace Klasteryzacja
{
    class Data
    {

        public static List<string> ExtractClasses(string[] lines)
        {
            List<string> classes = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                int lastIndex = tmp.Length - 1;
                if (classes.Contains(tmp[lastIndex])) continue;
                classes.Add(tmp[lastIndex]);
            }
            return classes;
        }

        public static double[][] Prepare(string[] lines, List<string> classes)
        {
            double[][] data = new double[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                int lastIndex = tmp.Length - 1;
                data[i] = new double[tmp.Length];
                for (int j = 0; j < tmp.Length - 1; j++)
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));
                data[i][lastIndex] = classes.IndexOf(tmp[lastIndex]);
            }
            return data;
        }

        public static List<double> CalculateDistances(double[][] data, double[] newObject)
        {
            List<double> distances = new List<double>();
            for (int line = 0; line < data.Length; line++)
            {
                double distance = 0;
                for (int number = 0; number < data[0].Length - 1; number++)
                    distance += (data[line][number] - newObject[number]) * (data[line][number] - newObject[number]);
                distances.Add(Math.Sqrt(distance));
            }
            return distances;
        }

        public static int[] ShortestDistances(double[][] data, List<double> distances, List<string> classes)
        {
            int[] shortest = new int[classes.Count];
            int k = distances.Count / 10, index = distances.IndexOf(distances.Min()), last = data[0].Length - 1;
            for (int i = 0; i < k; i++)
            {
                shortest[(int)data[index][last]] += 1;
                distances[index] = double.MaxValue;
                index = distances.IndexOf(distances.Min());
            }
            return shortest;
        }
    }
}