using System.Collections.Generic;

namespace Kolokwium.NeuralNetwork
{
    class Neuron
    {
        public List<Synapse> Inputs { get; set; }   // lista synaps wejściowych
        public List<Synapse> Outputs { get; set; }  // lista synaps wyjściowych
        public double InputValue { get; set; }      // wartość wejściowa neuronu
        public double OutputValue { get; set; }     // wartość wyjściowa neuronu

        public Neuron()
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
        }

        // Tworzy synapsę pomiędzy bieżącym a następnym neuronem, a także dodaje ją do listy synaps
        // wyjściowych w tym neuronie i do listy synaps wejściowych w tamtym neuronie:
        public void AddOutputNeuron(Neuron outputneuron)
        {
            Synapse synapse = new Synapse(this, outputneuron);
            Outputs.Add(synapse); outputneuron.Inputs.Add(synapse);
        }

        // Tworzy synapsę wejściową z podaną jako argument wartością (jest to liczba zaserwowana sieci jako dane
        // wejściowe) i dodaje ją do listy synaps wejściowych bieżącego neuronu:
        public void AddInputSynapse(double input)
        {
            Synapse syn = new Synapse(this, input);
            Inputs.Add(syn);
        }

        // Wylicza wartość wejściową neuronu poprzez sumowanie iloczynów wartości wyjściowych neuronów z poprzedniej
        // warstwy i wag synaps łączych te neurony z tym neuronem; wyznacza także na podstawie tej wartości
        // wartość wyjściową poprzez zastosowanie funkcji aktywacji:
        public void CalculateOutput()
        {
            InputValue = Functions.InputSumFunction(Inputs);
            OutputValue = Functions.BipolarLinearFunction(InputValue);
        }

        // Ustawia wartość wyjściową synapsy wejściowej warstwy wejściowej sieci - odpowiada za "wkładanie" danych:
        public void PushValueOnInput(double input)
        {
            Inputs[0].PushedData = input;
        }
    }
}
