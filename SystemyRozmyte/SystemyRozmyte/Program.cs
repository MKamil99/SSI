// Kamil Matula gr. D, 31.03.2020, Systemy Rozmyte

using System;
using System.Collections.Generic;

namespace SystemyRozmyte
{
    class Program
    {
        static void Main(string[] args) 
        {
            List<City> cities = Fuzzy.PrepareList();
            foreach (City city in cities) Fuzzy.LifeStyle(city);
            Console.ReadKey();
        }
    }
}