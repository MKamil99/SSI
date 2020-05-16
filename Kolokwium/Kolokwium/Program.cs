// Kamil Matula, gr. D, 04.05.2020, Kolokwium z Systemów Sztucznej Inteligencji
using System;

namespace Kolokwium
{
    using NeuralNetwork;
    class Program
    {
        static void Main(string[] args)
        {
            // Zadanie 1:
            double[][] data = Data.LoadDatabase(@"data_banknote_authentication.txt");
            Data.Shuffle(data);
            Data.Normalize(data);


            // Zadanie 2:
            Console.WriteLine("\n   *** SOFT SETS ***");       // cechy banknotów zgodnie z bazą: 
            double[] weights = new double[] { 0, 0, 1, 0.5 };  // variance, skewness, curtosis, entropy;
            SoftSets.Decide(data, weights);                    // wagi to po prostu to, jak ważna 
                                                               // dla użytkownika jest dana cecha

            // Zadanie 3:
            Console.WriteLine("\n   *** NEURAL NETWORK ***");
            double[][][] datasets = Data.PrepareDatasets(data);
            // datasets[0] - dane wejściowe zbioru trenującego: 70% bazy
            // datasets[1] - oczekiwane dane wyjściowe zbioru trenującego (70% bazy)
            // datasets[2] - dane wejściowe zbioru walidacyjnego: 30% bazy
            // datasets[3] - oczekiwane dane wyjściowe zbioru walidacyjnego (30% bazy)

            Network network = new Network(4, 6, 5, 2);
            //network.LoadWeights(@"weights.txt");
            network.CalculatePrecision(datasets);
            network.Train(datasets, 0.02);           // ze względu na losowość wag inicjalizacyjnych
                                                     // zdecydowałem się na zastosowanie pętli while w miejsce fora;
                                                     // sieć uczy się tak długo aż osiągnie zadany błąd średniokwadratowy
            network.CalculatePrecision(datasets, true);

            Console.ReadKey();
        }
    }
}