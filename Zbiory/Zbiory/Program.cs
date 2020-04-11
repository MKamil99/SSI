// ZBIOREM MIĘKKIM nazywamy parę (F,A), gdzie F: A -> P(U).
// W powyższym wzorze F jest zbiorem obiektów (lub funkcją działającą z A w P(U)),
// A zbiorem parametrów je opisujących, a P(U) - zbiorem wszystkich możliwych 
// kombinacji tych parametrów.

// PODZBIOREM MIĘKKIM nazywamy zredukowany zbiór miękki zawierający się innym zbiorze miękkim.

// Jeśli P jest podzbiorem A składającym się z poszukiwanych parametrów, można stworzyć tabelę relacji
// binarnej, gdzie dostępne obiekty będą miały przypisane pewne parametry, których obecność będzie 
// oznaczana w tej tabeli jako 1, a brak - jako 0.

// Dla każdego obiektu jest następnie liczona waga (na podstawie tego, czego szukamy),
// a obiekt o maksymalnej wartości zostaje uznany za ten szukany.

using System;
using System.Collections.Generic;

namespace Zbiory
{
    class Program
    {
        static void Main(string[] args)
        {
            //                 SPODNIE
            Console.Write("           *** ZAKUP SPODNI ***\n\n");

            // Zbiory parametrów i obiektów:
            string[] parametry = { "drogie", "tanie", "jeans", "dresowe", "klasyczne", "modern",
                                 "fit", "granatowe", "czarne", "na zamek", "na guziki" };
            List<string[]> oferta = OfertaSpodni();
            int[][] tabelarelacji = Relacja(parametry, oferta);

            // Przychodzi klient do sklepu...
            string[] klientA = { "jeans", "modern", "na zamek" };
            string[] klientB = { "jeans", "klasyczne", "granatowe", "na guziki" };
            int decyzja1 = Decyzja(parametry, klientA, tabelarelacji);
            Console.WriteLine(" Najbliższa oczekiwaniom klienta A jest para nr {0}", decyzja1 + 1);
            int decyzja2 = Decyzja(parametry, klientB, tabelarelacji);
            Console.WriteLine(" Najbliższa oczekiwaniom klienta B jest para nr {0}", decyzja2 + 1);


            //               WARZYWA
            Console.Write("\n\n          *** ZAKUP WARZYW ***\n\n");
            string[] parametry2 = { "świeże", "mrożone", "ostre", "słodkie", "zielone", "czerwone", 
                                    "lokalne", "tropikalne", "liściaste", "bulwowe" };
            string[] nazwywarzyw = { "burak", "ziemniak", "rzodkiewka", "ogórek", "sałata","papryka", "chilli"};
            oferta = OfertaWarzyw(nazwywarzyw);
            tabelarelacji = Relacja(parametry2, oferta);
            string[] klient1 = { "świeże", "ostre", "czerwone" };
            string[] klient2 = { "mrożone", "zielone", "słodkie", "liściaste" };
            string[] klient3 = { "świeże", "zielone", "czerwone", "słodkie" };
            decyzja1 = Decyzja(parametry2, klient1, tabelarelacji);
            Console.WriteLine(" Klient 1 prawdopodobnie kupi {0} ", nazwywarzyw[decyzja1]);
            decyzja2 = Decyzja(parametry2, klient2, tabelarelacji);
            Console.WriteLine(" Klient 2 prawdopodobnie kupi {0} ", nazwywarzyw[decyzja2]);
            int decyzja3 = Decyzja(parametry2, klient3, tabelarelacji);
            Console.WriteLine(" Klient 3 prawdopodobnie kupi {0} ", nazwywarzyw[decyzja3]);
            Console.ReadKey();
        }

        static int[][] Relacja(string[] parametry, List<string[]> oferta)
        {
            // Stworzenie na podstawie powyższych zbiorów tabeli relacji:
            int[][] tabelarelacji = new int[oferta.Count][];
            for (int i = 0; i < tabelarelacji.Length; i++)
                tabelarelacji[i] = new int[parametry.Length];

            // Uzupełnienie tabeli relacji:
            for (int i = 0; i < tabelarelacji.Length; i++)
                for (int j = 0; j < tabelarelacji[0].Length; j++)
                    for (int k = 0; k < oferta[i].Length; k++)
                        if (oferta[i][k].ToString() == parametry[j].ToString())
                            tabelarelacji[i][j] = 1;

            // Wyświetlanie tabeli:
            /*
            for (int i = 0; i < tabelarelacji.Length; i++)
            {
                for (int j = 0; j < tabelarelacji[0].Length; j++)
                    Console.Write(tabelarelacji[i][j] + " ");
                Console.Write("\n");
            }
            */

            return tabelarelacji;
        }

        static int Decyzja(string[] parametry, string[] klient, int[][] tabelarelacji)
        {
            // Czego szuka klient?
            Console.Write("\n Klient szuka produktu, który ma parametry: ");
            for (int i = 0; i < klient.Length - 1; i++) Console.Write(klient[i] + ", ");
            Console.Write(klient[klient.Length - 1] + "\n");

            // Wybranie kolumn, na które należy zwrócić uwagę:
            int[] zgodneparametry = new int[parametry.Length];
            for (int i = 0; i < parametry.Length; i++)
                for (int j = 0; j < klient.Length; j++)
                    if (parametry[i] == klient[j])
                        zgodneparametry[i] = 1;

            // Zliczenie ile parametrów się zgadza z poszukiwanymi:
            int[] cobytuwybrac = new int[tabelarelacji.Length];
            for (int j = 0; j < tabelarelacji[0].Length; j++)
                if (zgodneparametry[j] == 1)
                    for (int i = 0; i < tabelarelacji.Length; i++)
                         if (tabelarelacji[i][j] == 1)
                             cobytuwybrac[i] += 1;

            // Sprawdzenie co ma największą zgodność
            int wybor = 0;
            for (int i = 1; i < cobytuwybrac.Length; i++)
                if (cobytuwybrac[i] > cobytuwybrac[wybor]) 
                    wybor = i;
            return wybor;
        }

        static List<string[]> OfertaSpodni()
        {
            List<string[]> oferta = new List<string[]>()
            {
                new string[] { "tanie", "jeans", "na zamek", "granatowe", "klasyczne" },
                new string[] { "drogie", "czarne", "modern", "jeans", "na guziki" },
                new string[] { "dresowe", "tanie", "czarne" },
                new string[] { "jeans", "fit", "granatowe", "na zamek" },
                new string[] { "jeans", "modern", "na zamek", "czarne", "drogie" }
            };

            for (int i = 0; i < oferta.Count; i++)
            {
                Console.Write(" Para nr {0}: ", i + 1);
                for (int j = 0; j < oferta[i].Length - 1; j++) Console.Write(oferta[i][j] + ", ");
                Console.Write(oferta[i][oferta[i].Length - 1] + "\n");
            }

            return oferta;
        }

        static List<string[]> OfertaWarzyw(string[] nazwywarzyw)
        {
            List<string[]> oferta = new List<string[]>()
            {
                new string[] { "świeże", "czerwone", "lokalne", "bulwowe" },//burak
                new string[] { "słodkie", "bulwowe", "lokalne" }, //ziemniak
                new string[] { "lokalne", "świeże", "czerwone"}, //rzodkiewka
                new string[] { "zielone", "lokalne", "świeże" }, //ogórek
                new string[] { "zielone", "liściaste", "lokalne" }, //sałata
                new string[] { "zielone", "czerwone", "słodkie" }, //papryka
                new string[] { "tropikalne", "czerwone", "ostre", "świeże" } //chilli
            };

            for (int i = 0; i < oferta.Count; i++)
            {
                Console.Write(" {0}: ", nazwywarzyw[i]);
                for (int j = 0; j < oferta[i].Length - 1; j++) Console.Write(oferta[i][j] + ", ");
                Console.Write(oferta[i][oferta[i].Length - 1] + "\n");
            }

            return oferta;
        }
    }
}