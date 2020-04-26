using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class Network
    {
        static double coefficient = 0.5;
        internal List<Layer> Layers;
        internal double[][] ExpectedResult;
        double[][] differences;

        public Network(int inputneuronscount, int hiddenlayerscount, int hiddenneuronscount, int outputneuronscount)
        {
            if (inputneuronscount < 1 || hiddenlayerscount < 1 || hiddenneuronscount < 1 || outputneuronscount < 1)
                throw new Exception("Incorrect Network Parameters");

            Layers = new List<Layer>();
            AddFirstLayer(inputneuronscount);
            for (int i = 0; i < hiddenlayerscount; i++) 
                AddNextLayer(new Layer(hiddenneuronscount));
            AddNextLayer(new Layer(outputneuronscount));

            differences = new double[Layers.Count][];
            for (int i = 1; i < Layers.Count; i++) 
                differences[i] = new double[Layers[i].Neurons.Count];
        }

        private void AddFirstLayer(int inputneuronscount)
        {
            Layer inputlayer = new Layer(inputneuronscount);
            foreach (Neuron neuron in inputlayer.Neurons) 
                neuron.AddInputSynapse(0);
            Layers.Add(inputlayer);
        }

        public void AddNextLayer(Layer newlayer)
        {
            Layer lastlayer = Layers[Layers.Count - 1];
            lastlayer.ConnectLayers(newlayer);
            Layers.Add(newlayer);
        }

        public void PushInputValues(double[] inputs)
        {
            if (inputs.Length != Layers[0].Neurons.Count) 
                throw new Exception("Incorrect Input Size");

            for (int i = 0; i < inputs.Length; i++) 
                Layers[0].Neurons[i].PushValueOnInput(inputs[i]);
        }

        public void PushExpectedValues(double[][] expectedvalues) 
        {
            if (expectedvalues[0].Length != Layers[Layers.Count - 1].Neurons.Count) 
                throw new Exception("Incorrect Expected Output Size");

            ExpectedResult = expectedvalues;
        }

        public List<double> GetOutput()
        {
            List<double> output = new List<double>();
            for (int i = 0; i < Layers.Count; i++)
                Layers[i].CalculateOutputOnLayer();
            foreach (Neuron neuron in Layers[Layers.Count - 1].Neurons)
                output.Add(neuron.OutputValue);
            return output;
        }

        public void Train(double[][] inputs, double maxerror)
        {
            Console.WriteLine("\n Trwa nauczanie sieci...");
            double error = double.MaxValue;
            while (error / inputs.Length > maxerror)
            {
                error = 0;
                List<double> outputs = new List<double>();
                for (int j = 0; j < inputs.Length; j++)
                {
                    PushInputValues(inputs[j]);
                    outputs = GetOutput();
                    ChangeWeights(outputs, j);
                    error += Functions.CalculateError(outputs, j, ExpectedResult);
                }
                //Console.WriteLine("Actual error: " + (error/inputs.Length).ToString());  // testing error
            }
            Console.WriteLine(" Sieć nauczona! Średni błąd średniokwadratowy wynosi: " + (Math.Round(error / inputs.Length, 5)).ToString() + "\n");
        }

        private void CalculateDifferences(List<double> outputs, int row)
        {
            for (int i = 0; i < Layers[Layers.Count - 1].Neurons.Count; i++)
                differences[Layers.Count - 1][i] = (ExpectedResult[row][i] - outputs[i]) 
                    * Functions.BipolarDifferential(Layers[Layers.Count - 1].Neurons[i].InputValue);
            for (int k = Layers.Count - 2; k > 0; k--)
                for (int i = 0; i < Layers[k].Neurons.Count; i++)
                {
                    differences[k][i] = 0;
                    for (int j = 0; j < Layers[k + 1].Neurons.Count; j++)
                        differences[k][i] += differences[k + 1][j] * Layers[k+1].Neurons[j].Inputs[i].Weight;
                    differences[k][i] *= Functions.BipolarDifferential(Layers[k].Neurons[i].InputValue);
                }
        }

        public void ChangeWeights(List<double> outputs, int row)
        {
            CalculateDifferences(outputs, row);
            for (int k = Layers.Count - 1; k > 0; k--)
                for (int i = 0; i < Layers[k].Neurons.Count; i++)
                    for (int j = 0; j < Layers[k - 1].Neurons.Count; j++)
                        Layers[k].Neurons[i].Inputs[j].UpdateWeight(
                            coefficient * 2 * differences[k][i] * Layers[k - 1].Neurons[j].OutputValue);
        }
    }
}