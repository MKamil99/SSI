// Kamil Matula, gr. D, 25.03.2020, Algorytm wyszukiwania punktów kluczowych
// ----------------------------------------------------------------------------
// Zaproponowany przeze mnie algorytm przyjmuje na wejściu obraz przefiltrowany
// algorytmem znajdowania krawędzi, a następnie znajduje na bitmapie najjaśniejsze
// piksele (5% wszystkich) i zaznacza je na oryginalnym obrazie kolorem czerwonym 
// lub niebieskim (w zależności od podania argumentów podczas wywoływania funkcji).

using System;

namespace PunktyKluczowe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Punkty kluczowe:
            Console.Write("\n Podaj ścieżkę pliku graficznego: ");
            string path = Console.ReadLine();
            Grafika.BlueRed(path);  // (path, true), jeśli na niebiesko
            Console.WriteLine(" Obraz został przekonwertowany.");
            Console.ReadKey();
        }
    }
}