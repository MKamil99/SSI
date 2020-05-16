using System;
using System.Linq;

namespace Kolokwium
{
    class SoftSets
    {
        // Działanie zbioru miękkiego:
        public static void Decide(double[][] data, double[] weights)
        {
            double[][] ZeroOneTable = RoundData(data);
            if (ZeroOneTable[0].Length != weights.Length) 
                throw new Exception("Incorrect weights amount!");

            // Mnożenie wag podanych przez użytkownika przez wyznaczone zera i jedynki 
            // - wyliczanie wartości wyboru:
            double[] choices = new double[data.Length];
            for (int i = 0; i < ZeroOneTable.Length; i++)
            {
                choices[i] = 0;
                for (int j = 0; j < ZeroOneTable[i].Length; j++)
                    choices[i] += ZeroOneTable[i][j] * weights[j];
            }

            // Znalezienie maksimum, zliczenie wystąpień dla obu klas i wybór klasy:
            double max = choices.Max();
            int[] count = new int[2];
            for (int i = 0; i < choices.Length; i++)
                if (choices[i] == max)
                    count[(int)data[i][data[0].Length - 1]] += 1;
            Console.WriteLine($" Max: {max}, Class 0: {count[0]} times, Class 1: {count[1]} times");
            Console.WriteLine($" Decision: Class {count.ToList().IndexOf(count.Max())}");
        }

        // Na podstawie (znormalizowanej) bazy danych tworzy tablicę dwuwymiarową,
        // która będzie wypełniona zerami i jedynkami, dzięki funkcji Math.Round:
        private static double[][] RoundData(double[][] data)
        {
            double[][] ZeroOneTable = new double[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                ZeroOneTable[i] = new double[data[0].Length - 1];
                for (int j = 0; j < ZeroOneTable[i].Length; j++)
                    ZeroOneTable[i][j] = Math.Round(data[i][j]);
            }
            return ZeroOneTable;
        }
    }
}