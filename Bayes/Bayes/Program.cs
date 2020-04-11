// Kamil Matula, gr. D, 19.03.2020, Naiwny klasyfikator Bayesa

using System;
using System.Collections.Generic;

namespace Bayes
{
    class Program
    {
        static void Main(string[] args)
        {
            List<WarunkiPogodowe> zbior = Klasyfikator.InicjujZbior();
            Klasyfikator.PokazZbiorTreningowy(zbior);

            // PRZYKŁAD 2 Z PREZENTACJI 
            WarunkiPogodowe nowyprzypadek1 = new WarunkiPogodowe("deszczowo", 21, "słaby", null);
            Console.WriteLine("\n Obecne warunki: " + nowyprzypadek1.ToString());
            Klasyfikator.Zdecyduj(zbior, nowyprzypadek1);

            // PRZYKŁAD 3 Z PREZENTACJI - PROBLEM CZĘSTOŚCI ZERA
            Console.WriteLine("\n");
            WarunkiPogodowe nowyprzypadek2 = new WarunkiPogodowe("pochmurno", 14, "mocny", null);
            Console.WriteLine("\n Obecne warunki: " + nowyprzypadek2.ToString());
            Klasyfikator.Zdecyduj(zbior, nowyprzypadek2);

            Console.ReadKey();
        }
    }
}