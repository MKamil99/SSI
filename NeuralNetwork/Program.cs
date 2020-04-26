using System;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Preparing Data
            double[][] data = Data.LoadIrises(@"Baza.txt");
            double[] numbers = new double[data.Length];
            for (int i = 0; i < data.Length; i++) numbers[i] = i;
            Data.Shuffle(numbers);
            double[][] tmpdata = new double[data.Length][];
            for (int i = 0; i < numbers.Length; i++)
                tmpdata[i] = data[(int)numbers[i]];
            data = tmpdata;

            double[][] expectedvalues = new double[data.Length][];
            double[][] trainingdata = new double[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                trainingdata[i] = new double[4]; expectedvalues[i] = new double[3];
                for (int j = 0; j < 4; j++) trainingdata[i][j] = data[i][j];
                for (int j = 4; j < 7; j++) expectedvalues[i][j - 4] = data[i][j];
            }
            #endregion

            Network network = new Network(4, 2, 4, 3);
            network.PushExpectedValues(expectedvalues);
            network.Train(trainingdata, 0.02);

            // TESTING:
            double[][] dataagain = Data.LoadIrises(@"Baza.txt"); double[][] importantdata = new double[dataagain.Length][];
            for (int i = 0; i < dataagain.Length; i++) importantdata[i] = new double[] { dataagain[i][0], dataagain[i][1], dataagain[i][2], dataagain[i][3] };
            for (int i = 0; i < importantdata.Length; i++) Data.ClassifyIris(importantdata[i], network);

            Console.ReadKey();
        }
    }
}