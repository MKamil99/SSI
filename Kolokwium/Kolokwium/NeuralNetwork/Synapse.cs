using System;

namespace Kolokwium.NeuralNetwork
{
    class Synapse
    {
        static Random tmp = new Random();
        internal Neuron FromNeuron, ToNeuron;
        public double Weight { get; set; }                 // waga synapsy
        public double PushedData { get; set; }             // dotyczy jedynie synapsy wejściowej warstwy wejściowej
        public static int SynapsesCount { get; set; } = 0; // ilość synaps, z jakich składa się sieć; przydatne w walidacji danych

        public Synapse(Neuron fromneuron, Neuron toneuron) // konstruktor synapsu łączącej neurony
        {
            FromNeuron = fromneuron; ToNeuron = toneuron;
            Weight = (tmp.NextDouble() - 0.5) * 2;         // losowa waga z przedziału (-1; 1)
            SynapsesCount += 1;
        }

        public Synapse(Neuron toneuron, double output)     // konstruktor synapsy wejściowej warstwy wejściowej
        {
            ToNeuron = toneuron; PushedData = output;
            Weight = 1;
            SynapsesCount += 1;
        }

        // Jeśli nie ma neuronu wcześniej (czyli to pierwsza wartstwa) zwróć zaserwowane wcześniej dane; 
        // w przeciwnym razie zwróć wartość wyjściową wcześniejszego neuronu pomnożoną przez wagę synapsy:
        public double GetOutput()
            => FromNeuron == null ? PushedData : FromNeuron.OutputValue * Weight;
    }
}
