using System;
using System.IO;

namespace Kolokwium
{
    class Data
    {
        public static void Shuffle(double[][] data)
        {
            Random rnd = new Random();
            int x = 0, n = data.Length; double[] y = new double[data[0].Length];
            for (int i = 0; i < n; i++)
            {
                x = i + rnd.Next(n - i); // losowanie liczby całkowitej z przedziału [0; n-i)
                y = data[x];             // i zamiana i-tego wiersza z wierszem o wylosowanym indeksie
                data[x] = data[i];       // miejscami; jeśli rnd.Next == 0, nie dojdzie do zamiany miejsc
                data[i] = y;
            }
        }

        public static void Normalize(double[][] data)
        {
            double[] tmp = new double[data.Length];
            for (int a = 0; a < data[0].Length - 1; a++) // normalizacja wszystkich kolumn oprócz ostatniej
            {
                for (int b = 0; b < data.Length; b++) tmp[b] = data[b][a];
                NormalizeColumn(tmp);
                for (int b = 0; b < data.Length; b++) data[b][a] = tmp[b];
            }
        }

        public static void NormalizeColumn(double[] data)
        {
            // Znajdowanie maksimum i minimum w tablicy:
            double max = data[0], min = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] > max) max = data[i];
                else if (data[i] < min) min = data[i];
            }

            // Normalizacja metodą min-max do przedziału [0;1]:
            double nmin = 0, nmax = 1;
            for (int i = 0; i < data.Length; i++)
                data[i] = Math.Round((data[i] - min) / (max - min) * (nmax - nmin) + nmin, 4);
        }

        public static double[][] LoadDatabase(string path)
        {
            string[] lines = File.ReadAllLines(path);                         // sczytanie linii z pliku
            double[][] data = new double[lines.Length][]; 
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');                           // oddzielenie liczb
                data[i] = new double[tmp.Length];
                for (int j = 0; j < tmp.Length; j++)
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));  // konwersja ze stringa na double
            }
            return data;
        }

        public static void PrintData(double[][] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[0].Length; j++)
                    Console.Write(string.Format("{0,8}", data[i][j].ToString("0.0000")) + " ");
                Console.WriteLine();
            }
        }

        public static double[][][] PrepareDatasets(double[][] data)
        {
            double[][][] datasets = new double[4][][];
            int row = 0, trainingpercent = data.Length * 7 / 10, columncount = data[0].Length;
            datasets[0] = new double[trainingpercent][];
            datasets[1] = new double[trainingpercent][];
            datasets[2] = new double[data.Length - trainingpercent][];
            datasets[3] = new double[data.Length - trainingpercent][];

            // Przygotowywanie zbioru trenującego (70% bazy danych):
            while (row < trainingpercent)
            {
                datasets[0][row] = new double[columncount - 1];
                for (int column = 0; column < columncount - 1; column++)
                    datasets[0][row][column] = data[row][column];

                datasets[1][row] = new double[2];
                datasets[1][row][(int)data[row][columncount - 1]] = 1;
                
                row++;
            }

            // Przygotowywanie zbioru testowego (pozostałe 30% bazy danych):
            while (row < data.Length)
            {
                datasets[2][row - trainingpercent] = new double[columncount - 1];
                for (int column = 0; column < columncount - 1; column++)
                    datasets[2][row - trainingpercent][column] = data[row][column];

                datasets[3][row - trainingpercent] = new double[2];
                datasets[3][row - trainingpercent][(int)data[row][columncount - 1]] = 1;
                
                row++;
            }
            return datasets;
        }
    }
}
