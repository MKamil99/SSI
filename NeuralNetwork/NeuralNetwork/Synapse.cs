// Kamil Matula gr. D, 26.04.2020, Sztuczne Sieci Neuronowe
using System;

namespace NeuralNetwork
{
    class Synapse
    {
        static Random tmp = new Random();
        internal Neuron FromNeuron, ToNeuron;
        public double Weight { get; set; }
        public double OutputValue { get; set; }

        public Synapse(Neuron fromneuron, Neuron toneuron)
        {
            FromNeuron = fromneuron; ToNeuron = toneuron;
            Weight = tmp.NextDouble() - 0.5;
        }

        public Synapse(Neuron toneuron, double output) // synapsa połączona z neuronami wejściowymi
        {
            ToNeuron = toneuron; OutputValue = output; 
            Weight = 1;
        }

        public double GetOutput()
        {
            if (FromNeuron == null) return OutputValue;
            return FromNeuron.OutputValue * Weight;
        }

        public void UpdateWeight(double delta)
        {
            Weight += delta;
        }
    }
}