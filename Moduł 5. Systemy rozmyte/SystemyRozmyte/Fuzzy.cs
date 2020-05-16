// Kamil Matula gr. D, 31.03.2020, Systemy Rozmyte

using System;
using System.Collections.Generic;

namespace SystemyRozmyte
{
    class Fuzzy
    {
        #region Funkcje przynależności
        public static double TriangleFunction(double a, double b, double c, double x)
        {
            if ((a < x) && (x <= b)) return (x - a) / (b - a);
            if ((b < x) && (x < c)) return (c - x) / (c - b);
            return 0;
        }

        public static double TrapezeFunction(double a, double b, double c, double d, double x)
        {
            if (x <= a) return 0;
            if (x > a && x <= b) return (x - a) / (b - a);
            if (x > b && x < c) return 1;
            if (x >= c && x <= d) return (d - x) / (d - c);
            return 0;
        }
        #endregion

        #region Normy
        public static double prod(double a, double b) => a * b;
        public static double min(double a, double b) => a < b ? a : b;
        #endregion

        public static List<City> PrepareList()
        {
            List<City> cities = new List<City>();
            cities.Add(new City("Warszawa", 0.6, 0.7));
            cities.Add(new City("Kraków", 1.0, 0.9));
            cities.Add(new City("Gdańsk", 0.9, 0.1));
            cities.Add(new City("Wrocław", 0.8, 0.3));
            cities.Add(new City("Katowice", 0.3, 0.9));
            cities.Add(new City("Poznań", 0.7, 0.4));
            cities.Add(new City("Gliwice", 0.3, 0.9));
            return cities;
        }

        public static void LifeStyle(City city) 
        {
            // Reguły:
            List<double> InsolationRules = new List<double>();
            InsolationRules.Add(TrapezeFunction(0, 0, 0.2, 0.4, city.Insolation));    // małe nasłonecznienie
            InsolationRules.Add(TriangleFunction(0.2, 0.5, 0.8, city.Insolation));    // średnie nasłonecznienie
            InsolationRules.Add(TrapezeFunction(0.6, 0.8, 1, 1, city.Insolation));    // duże nasłonecznienie

            List<double> PollutionRules = new List<double>();
            PollutionRules.Add(TrapezeFunction(0, 0, 0.2, 0.3, city.Pollution));      // małe skażenie powietrza
            PollutionRules.Add(TriangleFunction(0.1, 0.4, 0.7, city.Pollution));      // średnie skażenie
            PollutionRules.Add(TrapezeFunction(0.5, 0.7, 1, 1, city.Pollution));      // duże skażenie

            // Wnioskowanie:
            List<double> results = new List<double>();
            for (int i = 0; i < InsolationRules.Count; i++)
                for (int j = 0; j < PollutionRules.Count; j++)
                    results.Add(min(InsolationRules[i], PollutionRules[j]));

            List<double> resultOfRules = new List<double>();
            resultOfRules.Add(0.6); // mało słońca i małe skażenie
            resultOfRules.Add(0.3); // mało słońca i średnie skażenie
            resultOfRules.Add(0.1); // mało słońca i duże skażenie
            resultOfRules.Add(0.8); // średnie nasłonecznienie i małe skażenie
            resultOfRules.Add(0.5); // średnie nasłonecznienie i średnie skażenie
            resultOfRules.Add(0.2); // średnie nasłonecznienie i duże skażenie
            resultOfRules.Add(1.0); // dużo słońca i małe skażenie
            resultOfRules.Add(0.7); // dużo słońca i średnie skażenie
            resultOfRules.Add(0.3); // dużo słońca i duże skażenie


            // Wyostrzenie:
            double decision = 0, tmp = 0;
            for (int i = 0; i < resultOfRules.Count; i++) decision += results[i] * resultOfRules[i];
            for (int i = 0; i < results.Count; i++) tmp += results[i];
            double result = Math.Round((decision / tmp) * 10, 1);
            Console.WriteLine(" Jakość życia w mieście {0} wynosi {1}/10", city.Name, result);
        }
    }
}