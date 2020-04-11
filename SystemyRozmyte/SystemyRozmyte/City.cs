// Kamil Matula gr. D, 31.03.2020, Systemy Rozmyte

namespace SystemyRozmyte
{
    class City
    {
        private string name; private double insolation, pollution;
        public City(string name, double insolation, double pollution)
        {
            this.name = name;
            this.insolation = insolation;
            this.pollution = pollution;
        }

        public City(City city)
        {
            name = city.name;
            insolation = city.insolation;
            pollution = city.pollution;
        }

        public string Name { get { return name; } set { name = value; } }
        public double Insolation { get { return insolation; } set { insolation = value; } }
        public double Pollution { get { return pollution; } set { pollution = value; } }
    }
}