// Kamil Matula, gr. D, 27.03.2020, Klasteryzacja
// ----------------------------------------------
// Poniższa implementacja zadziała dla każdej bazy danych, której obiekty zapisane są w pliku 
// tekstowym w formacie "l1,l2,l3,l4,...,klasa", gdzie ln jest liczbą zmiennoprzecinkową z kropką

using System;
using System.IO;
using System.Collections.Generic;

namespace Klasteryzacja
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pobranie danych z pliku i wyodrębnienie klas
            string[] lines = File.ReadAllLines(@"Baza.txt");
            List<string> classes = Data.ExtractClasses(lines);
            double[][] data = Data.Prepare(lines, classes);

            // Podanie obiektu, który ma zostać zaklasyfikowany do danej klasy
            double[] newObject = new double[data[0].Length - 1];
            Console.WriteLine("Podaj odpowiednie dane zatwierdzając Enterem: ");
            for (int i = 0; i < newObject.Length; i++)
                newObject[i] = double.Parse(Console.ReadLine().Replace('.',','));

            // Policzenie dystansów, wybranie najkrótszych (10%) i zliczenie ile jest ich w danej klasie
            List<double> distances = Data.CalculateDistances(data, newObject);
            int[] shortest = Data.ShortestDistances(data, distances, classes);

            // Przypisanie do klasy
            int maxIndex = 0;
            for (int i = 1; i < shortest.Length; i++)
                if (shortest[i] > shortest[maxIndex]) 
                    maxIndex = i;
            Console.WriteLine("Zaklasyfikowano do klasy: " + classes[maxIndex].ToString());

            Console.ReadKey();
        }
    }
}