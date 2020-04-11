//Kamil Matula gr. D, 12.03.2020
using System;
using System.Collections.Generic;
using System.IO;

namespace Wprowadzenie
{
    class Dane
    {
        public static double[] Tasuj(double[] data)
        {
            List<double> lname = new List<double>(data);
            double[] tabname = new double[lname.Count];
            Random rnd = new Random();
            int x = 0; int n = lname.Count;
            for (int i = 0; i < n; i++)
            {
                x = rnd.Next(0, lname.Count);
                tabname[i] = lname[x];
                lname.RemoveAt(x);
            }
            return tabname;
        }

        public static double[] Normalizuj(double[] data)
        {
            double max = data[0], min = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] > max) max = data[i];
                else if (data[i] < min) min = data[i];
            }
            double nmin = 0, nmax = 1;
            for (int i = 0; i < data.Length; i++)
                data[i] = Math.Round((data[i] - min) / (max - min) * (nmax - nmin) + nmin, 4);
            return data;
        }

        public static double[][] Pobierz(string path)
        {
            string[] lines = File.ReadAllLines(path);
            double[][] data = new double[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                data[i] = new double[tmp.Length + 2];
                for (int j = 0; j < tmp.Length - 1; j++)
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));
                if (tmp[4] == "Iris-setosa") data[i][6] = 1;
                else if (tmp[4] == "Iris-versicolor") data[i][5] = 1;
                else if (tmp[4] == "Iris-virginica") data[i][4] = 1;
            }
            return data;
        }

        /*static void Tasuj2(double[] data)        // algorytm z prezentacji
        {
            Random rnd = new Random();
            int x = 0; int n = data.Length; double y = 0;
            for (int i = 0; i < n - 1; i++) {
                x = i + rnd.Next(n - i); y = data[x];
                data[x] = data[i]; data[i] = y;
            }
        }*/
    }
}