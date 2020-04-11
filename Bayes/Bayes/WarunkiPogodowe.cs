// Kamil Matula, gr. D, 19.03.2020, Naiwny klasyfikator Bayesa

namespace Bayes
{
    class WarunkiPogodowe
    {
        private string pogoda, wiatr, temp, decyzja;
        public WarunkiPogodowe(string pogoda, int temperatura, string wiatr, string decyzja)
        {
            this.pogoda = pogoda; this.wiatr = wiatr; this.decyzja = decyzja;
            if (temperatura <= 16) temp = "chłodno";
            else if (temperatura <= 20) temp = "ciepło";
            else temp = "gorąco";
        }

        public override string ToString()
        {
            if (decyzja != null) return $"{pogoda}, {temp}, wiatr {wiatr} - {decyzja}";
            else return $"{pogoda}, {temp}, wiatr {wiatr}";
        }

        public string Pogoda { get { return pogoda; } set { pogoda = value; } }
        public string Wiatr { get { return wiatr; } set { wiatr = value; } }
        public string Temp { get { return temp; } set { temp = value; } }
        public string Decyzja { get { return decyzja; } set { decyzja = value; } }
    }
}