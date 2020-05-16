using System;
using System.Collections.Generic;

namespace Kolokwium.NeuralNetwork
{
    class Functions
    {
        private static double Alpha { get; set; } = 0.5;

        // Funkcja licząca błąd średniokwadratowy - używana do celów testowych:
        public static double CalculateError(List<double> outputs, int row, double[][] expectedresults)
        {
            double error = 0;
            for (int i = 0; i < outputs.Count; i++)
                error += Math.Pow(outputs[i] - expectedresults[row][i], 2);
            return error;
        }

        // Funkcja wejściowa: iloczyn wartości wyjściowych wszystkich synaps wejściowych tego neuronu
        public static double InputSumFunction(List<Synapse> Inputs)
        {
            double input = 0;
            foreach (Synapse syn in Inputs)
                input += syn.GetOutput();
            return input;
        }

        public static double BipolarLinearFunction(double input) // funkcja aktywacji: bipolarna liniowa
            => (1 - Math.Pow(Math.E, -Alpha * input)) / (1 + Math.Pow(Math.E, -Alpha * input));

        public static double BipolarDifferential(double input)   // pochodna funkcji aktywacji (bipolarnej liniowej)
            => (2 * Alpha * Math.Pow(Math.E, -Alpha * input)) / (Math.Pow(1 + Math.Pow(Math.E, -Alpha * input), 2));
    }
}
