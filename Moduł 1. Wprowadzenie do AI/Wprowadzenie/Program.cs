//Kamil Matula gr. D, 12.03.2020
using System;

namespace Wprowadzenie
{
    class Program
    {
        static void Main(string[] args)
        {
            //LAB1:
                //A. Pobranie i znormalizowanie danych
            double[][] data = Dane.Pobierz(@"Baza.txt");
            double[] tmp = new double[data.Length];
            for (int a = 0; a < data[0].Length - 3; a++)
            {
                for (int b = 0; b < data.Length; b++)
                    tmp[b] = data[b][a];
                tmp = Dane.Normalizuj(tmp);
                for (int b = 0; b < data.Length; b++)
                    data[b][a] = tmp[b];
            }

                //B. Przetasowanie wierszy
            double[] numbers = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
                numbers[i] = i;
            numbers = Dane.Tasuj(numbers);
            double[][] tmpdata = new double[data.Length][];
            for (int i = 0; i < numbers.Length; i++)
                tmpdata[i] = data[(int)numbers[i]];
            data = tmpdata;

                //C. Wyświetlanie
            for (int m = 0; m < data.Length; m++)
            {
                for (int n = 0; n < data[m].Length; n++)
                {
                    if (n < 4) Console.Write(data[m][n].ToString("0.0000") + " ");
                    else Console.Write(data[m][n] + " ");
                }
                Console.Write("\n");
            }
            Console.ReadKey();

            //LAB2:
            Console.Write("\nPodaj ścieżkę pliku graficznego: ");
            string path1 = Console.ReadLine();
            Grafika.greyPicture(path1);
            Console.WriteLine("Obraz został przekonwertowany na czarno-biały");

            //LAB3:
            Console.Write("\nPodaj ścieżkę pliku graficznego: ");
            string path2 = Console.ReadLine();
            Grafika.filtr(path2);
            Console.WriteLine("Obraz został przefiltrowany");
        }
    }
}