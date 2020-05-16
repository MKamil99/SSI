using System.Collections.Generic;

namespace Kolokwium.NeuralNetwork
{
    class Layer
    {
        public List<Neuron> Neurons;  // lista neuronów wchodzących w skład warstwy sieci

        // Tworzy listę "pustych" neuronów, które stanowią razem jedną warstwę sieci neuronowej:
        public Layer(int numberofneurons)
        {
            Neurons = new List<Neuron>();
            for (int i = 0; i < numberofneurons; i++)
                Neurons.Add(new Neuron());
        }

        // Łączy dwie warstwy ze sobą poprzez dopinanie do każdego neuronu warstwy bieżącej 
        // każdego neuronu warstwy następnej
        public void ConnectLayers(Layer outputlayer)
        {
            foreach (Neuron thisneuron in Neurons)
                foreach (Neuron thatneuron in outputlayer.Neurons)
                    thisneuron.AddOutputNeuron(thatneuron);
        }

        // Dla każdego neuronu w warstwie wylicza jego wartość wejściową i wyjściową; zapobiega to konieczności
        // wielokrotnego przechodzenia przez całą sieć w celu wyznaczenia poszczególnych wartości - zwłaszcza podczas
        // nauczania algorytmem wstecznej propagacji
        public void CalculateOutputOnLayer()
        {
            foreach (Neuron neuron in Neurons)
                neuron.CalculateOutput();
        }
    }
}
