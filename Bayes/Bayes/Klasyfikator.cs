// Kamil Matula, gr. D, 19.03.2020, Naiwny klasyfikator Bayesa

using System;
using System.Collections.Generic;

namespace Bayes
{
    class Klasyfikator
    {
        public static void PokazZbiorTreningowy(List<WarunkiPogodowe> warunki)
        {
            Console.WriteLine("        *** ZBIÓR TRENINGOWY ***");
            for (int i = 0; i < warunki.Count; i++)
                Console.WriteLine(" " + (i + 1).ToString() + ". " + warunki[i]);
        }

        public static List<WarunkiPogodowe> InicjujZbior()
        {
            List<WarunkiPogodowe> zbior = new List<WarunkiPogodowe>();
            zbior.Add(new WarunkiPogodowe("słonecznie", 23, "umiarkowany", "tak"));
            zbior.Add(new WarunkiPogodowe("deszczowo", 15, "mocny", "nie"));
            zbior.Add(new WarunkiPogodowe("pochmurno", 17, "słaby", "tak"));
            zbior.Add(new WarunkiPogodowe("pochmurno", 21, "umiarkowany", "nie"));
            zbior.Add(new WarunkiPogodowe("słonecznie", 20, "mocny", "tak"));
            zbior.Add(new WarunkiPogodowe("słonecznie", 25, "słaby", "tak"));
            zbior.Add(new WarunkiPogodowe("deszczowo", 22, "słaby", "tak"));
            zbior.Add(new WarunkiPogodowe("słonecznie", 14, "mocny", "nie"));
            zbior.Add(new WarunkiPogodowe("pochmurno", 19, "mocny", "nie"));
            zbior.Add(new WarunkiPogodowe("deszczowo", 16, "słaby", "nie"));
            return zbior;
        }

        public static void Zdecyduj(List<WarunkiPogodowe> zbior, WarunkiPogodowe przypadek)
        {
            string pogodaTeraz = przypadek.Pogoda, tempTeraz = przypadek.Temp, wiatrTeraz = przypadek.Wiatr;
            int lambda = 1; // współczynnik korekcyjny
            int[] rozneatrybuty = { 3, 3, 3 }; //liczba różnych wartości danego atrybutu

            //Budowa "klas" C1 i C2
            List<WarunkiPogodowe> zbiorTAK = new List<WarunkiPogodowe>(); //C1
            List<WarunkiPogodowe> zbiorNIE = new List<WarunkiPogodowe>(); //C2
            for (int i = 0; i < zbior.Count; i++)
            {
                if (zbior[i].Decyzja == "tak") zbiorTAK.Add(zbior[i]);
                else zbiorNIE.Add(zbior[i]);
            }

            //Prawdopodobieństwo, że ten przypadek należy do C1:
            double C1 = (double)zbiorTAK.Count / zbior.Count;
            double pogoda = 0, temperatura = 0, wiatr = 0;
            for (int i = 0; i < zbiorTAK.Count; i++)
            {
                if (zbiorTAK[i].Pogoda == pogodaTeraz) pogoda += 1;
                if (zbiorTAK[i].Temp == tempTeraz) temperatura += 1;
                if (zbiorTAK[i].Wiatr == wiatrTeraz) wiatr += 1;
            }

            // If-else'y w przypadku wystąpienia problemu częstości zera
            if (pogoda == 0) pogoda = (pogoda + lambda) / (zbiorTAK.Count + lambda * rozneatrybuty[0]);
            else pogoda /= zbiorTAK.Count;
            if (temperatura == 0) temperatura = (temperatura + lambda) / (zbiorTAK.Count + lambda * rozneatrybuty[1]);
            else temperatura /= zbiorTAK.Count;
            if (wiatr == 0) wiatr = (wiatr + lambda) / (zbiorTAK.Count + lambda * rozneatrybuty[2]);
            else wiatr /= zbiorTAK.Count;
            double razemC1 = C1 * pogoda * temperatura * wiatr;

            //Prawdopodobieństwo, że ten przypadek należy do C2:
            double C2 = (double)zbiorNIE.Count / zbior.Count; pogoda = 0; temperatura = 0; wiatr = 0;
            for (int i = 0; i < zbiorNIE.Count; i++)
            {
                if (zbiorNIE[i].Pogoda == pogodaTeraz) pogoda += 1;
                if (zbiorNIE[i].Temp == tempTeraz) temperatura += 1;
                if (zbiorNIE[i].Wiatr == wiatrTeraz) wiatr += 1;
            }

            // If-else'y w przypadku wystąpienia problemu częstości zera
            if (pogoda == 0) pogoda = (pogoda + lambda) / (zbiorNIE.Count + lambda * rozneatrybuty[0]);
            else pogoda /= zbiorNIE.Count;
            if (temperatura == 0) temperatura = (temperatura + lambda) / (zbiorNIE.Count + lambda * rozneatrybuty[1]);
            else temperatura /= zbiorNIE.Count;
            if (wiatr == 0) wiatr = (wiatr + lambda) / (zbiorNIE.Count + lambda * rozneatrybuty[2]);
            else wiatr /= zbiorNIE.Count;
            double razemC2 = C2 * pogoda * temperatura * wiatr;

            Console.WriteLine("\n C1 (TAK): " + razemC1 + ", C2 (NIE): " + razemC2);
            Console.Write(" Decyzja: ");
            if (razemC1 > razemC2) Console.Write("Warto wyjść na spacer.");
            else Console.Write("Nie powinieneś wychodzić na spacer.");
        }
    }
}
