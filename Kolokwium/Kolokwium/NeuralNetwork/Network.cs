using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Kolokwium.NeuralNetwork
{
    class Network
    {
        static double LearningRate = 0.2;    // współczynnik uczenia
        internal List<Layer> Layers;         // lista warstw, z jakich składa się sieć neuronowa
        internal double[][] ExpectedResults; // tablica przetrzymująca spodziewane na wyjściu wyniki
        double[][] ErrorFunctionChanges;     // tablica przetrzymująca wyliczone tzw. zmiany funkcji błędu
                                             // zgodnie z algorytmem propagacji wstecznej


        // Konstruktor klasy Network przyjmujący za argumenty kolejno: ilość neuronów warstwy wejściowej,
        // ilość warstw ukrytych, ilość neuronów na każdej warstwie ukrytej, ilość neuronów na warstwie wyjściowej:
        public Network(int inputneuronscount, int hiddenlayerscount, int hiddenneuronscount, int outputneuronscount)
        {
            Console.WriteLine(" Building neural network...");
            if (inputneuronscount < 1 || hiddenlayerscount > 0 && hiddenneuronscount < 1 || outputneuronscount < 1)
                throw new Exception("Incorrect Network Parameters");

            Layers = new List<Layer>();
            AddFirstLayer(inputneuronscount);                   // dodanie warstwy wejściowej
            for (int i = 0; i < hiddenlayerscount; i++)         // dołączenie warstw ukrytych
                AddNextLayer(new Layer(hiddenneuronscount));
            AddNextLayer(new Layer(outputneuronscount));        // dołączenie warstwy wyjściowej

            ErrorFunctionChanges = new double[Layers.Count][];  // przygotowanie tablicy na "delty"
            for (int i = 1; i < Layers.Count; i++)
                ErrorFunctionChanges[i] = new double[Layers[i].Neurons.Count];
        }

        // Dodanie warstwy wejściowej o zadanej ilości neuronów; domyślnie serwowane do sieci są zera:
        private void AddFirstLayer(int inputneuronscount)
        {
            Layer inputlayer = new Layer(inputneuronscount);
            foreach (Neuron neuron in inputlayer.Neurons)
                neuron.AddInputSynapse(0);
            Layers.Add(inputlayer);
        }

        // Dodanie kolejnej warstwy (ukrytej lub wyjściowej) i powiązanie jej z ostatnią istniejącą:
        private void AddNextLayer(Layer newlayer)
        {
            Layer lastlayer = Layers[Layers.Count - 1];
            lastlayer.ConnectLayers(newlayer);
            Layers.Add(newlayer);
        }

        // Podanie każdemu neuronowi wejściowemu liczby z tablicy danych - udział synaps wejściowych warstwy wejściowej:
        public void PushInputValues(double[] inputs)
        {
            if (inputs.Length != Layers[0].Neurons.Count)
                throw new Exception("Incorrect Input Size");

            for (int i = 0; i < inputs.Length; i++)
                Layers[0].Neurons[i].PushValueOnInput(inputs[i]);
        }

        // Podanie sieci oczekiwanych wyników - wypełnienie tablicy ExpectedResults:
        public void PushExpectedValues(double[][] expectedvalues)
        {
            if (expectedvalues[0].Length != Layers[Layers.Count - 1].Neurons.Count)
                throw new Exception("Incorrect Expected Output Size");

            ExpectedResults = expectedvalues;
        }

        // Wyznaczenie wartości wyjściowej dla podanych sieci danych - w tym celu kolejno wyliczane są wartości
        // wyjściowe neuronów na każdej warstwie od warstwy wejściowej aż do warstwy wyjściowej, a następnie
        // wartości wyjściowe ostatniej warstwy dodawane są do listy, która jest ostatecznie zwracna przez funkcję:
        public List<double> GetOutput()
        {
            List<double> output = new List<double>();
            for (int i = 0; i < Layers.Count; i++)
                Layers[i].CalculateOutputOnLayer();
            foreach (Neuron neuron in Layers[Layers.Count - 1].Neurons)
                output.Add(neuron.OutputValue);
            return output;
        }

        // Trenowanie/nauczanie sieci poprzez wielokrotne serwowanie danych, wyznaczanie danych wyjściowych 
        // i zmienianie wag na synapsach; funkcja dodatkowo może pokazywać błąd średniokwadratowy liczony
        // na zbiorze testowym, a po nauczeniu sieci zapisuje wagi synaps do pliku "weights.txt":
        public void Train(double[][][] datasets, double expectectederror, bool showerror = false)
        {
            double[][] trainingInputs = datasets[0], trainingOutputs = datasets[1];
            double error = double.MaxValue;
            PushExpectedValues(trainingOutputs);
            Console.WriteLine(" Training neural network...");
            while (expectectederror < error)
            {
                List<double> outputs = new List<double>();
                for (int j = 0; j < trainingInputs.Length; j++)
                {
                    PushInputValues(trainingInputs[j]);
                    outputs = GetOutput();
                    ChangeWeights(outputs, j);
                }
                error = CalculateMeanSquareError(datasets[2], datasets[3], showerror);
            }
            SaveWeights(@"weights.txt");
            Console.WriteLine(" Done!");
        }

        // Wyznacza średni błąd średniokwadratowy dla zadanego zbioru (sumuje dla wszystkich inputów 
        // i dzieli przez wielkość zbioru) - przydatne do testowania sieci:
        private double CalculateMeanSquareError(double[][] inputs, double[][] expectedoutputs, bool showerror = false)
        {
            double error = 0;
            List<double> outputs = new List<double>();
            for (int i = 0; i < inputs.Length; i++)
            {
                PushInputValues(inputs[i]);
                outputs = GetOutput();
                error += Functions.CalculateError(outputs, i, expectedoutputs);
            }
            error /= inputs.Length;
            if (showerror == true) Console.WriteLine($" Average mean square error: {error}");
            return error;
        }

        // Zamiana wag na synapsach odbywa się zgodnie z algorytmem wstecznej propagacji: po wyznaczeniu zmian funkcji błędu
        // na każdym neuronie każdej warstwy (poza pierwszą) wartości te wykorzystywane są do wyliczania nowych wag;
        // procedura odbywa się od warstwy ostatniej do pierwszej i dotyczy synaps wejściowych:
        private void ChangeWeights(List<double> outputs, int row)
        {
            CalculateErrorFunctionChanges(outputs, row);
            for (int k = Layers.Count - 1; k > 0; k--)
                for (int i = 0; i < Layers[k].Neurons.Count; i++)
                    for (int j = 0; j < Layers[k - 1].Neurons.Count; j++)
                        Layers[k].Neurons[i].Inputs[j].Weight +=
                            LearningRate * 2 * ErrorFunctionChanges[k][i] * Layers[k - 1].Neurons[j].OutputValue;
            // waga j-tej synapsy wejściowej i-tego neuronu k-tej warstwy zmienia się o:
            // współczynnik uczenia * 2 * zmiana funkcji błędu na i-tym neuronie k-tej warstwy * w. wyjściowa j-tego neuronu warstwy wcześniejszej
        }


        // Pomocniczo najpierw osobno wyliczane są zmiany funkcji błędu, które potem wykorzystywane 
        // są w wyznaczaniu wartości gradientu błędu i docelowo nowych wag na synapsach:
        private void CalculateErrorFunctionChanges(List<double> outputs, int row)
        {
            for (int i = 0; i < Layers[Layers.Count - 1].Neurons.Count; i++) // osobno liczone są wartości dla warstwy ostatniej...
                ErrorFunctionChanges[Layers.Count - 1][i] = (ExpectedResults[row][i] - outputs[i])
                    * Functions.BipolarDifferential(Layers[Layers.Count - 1].Neurons[i].InputValue);
            for (int k = Layers.Count - 2; k > 0; k--) // ... i osobno dla reszty; tu wzór wygląda inaczaj i zależy od warstwy następnej
                for (int i = 0; i < Layers[k].Neurons.Count; i++)
                {
                    ErrorFunctionChanges[k][i] = 0;
                    for (int j = 0; j < Layers[k + 1].Neurons.Count; j++)
                        ErrorFunctionChanges[k][i] += ErrorFunctionChanges[k + 1][j] * Layers[k + 1].Neurons[j].Inputs[i].Weight;
                    ErrorFunctionChanges[k][i] *= Functions.BipolarDifferential(Layers[k].Neurons[i].InputValue);
                }
        }

        // Iteruje przez całą sieć (przez każdą synapsę wejściową każdego neuronu każdej warstwy), 
        // dodając wagi synaps do listy stringów, a następnie zapisuje je w pliku linia po linii:
        private void SaveWeights(string path)
        {
            List<string> tmp = new List<string>();
            foreach (Layer layer in Layers)
                foreach (Neuron neuron in layer.Neurons)
                    foreach (Synapse synapse in neuron.Inputs)
                        tmp.Add(synapse.Weight.ToString());
            File.WriteAllLines(path, tmp);
        }

        // Iteruje przez całą sieć (przez każdą synapsę wejściową każdego neuronu każdej warstwy), 
        // nadając wagi synapsom zgodnie z liczbami zapisanymi w pliku tekstym (są zapisane linia po linii):
        public void LoadWeights(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine(" Loading weights...");
                string[] lines = File.ReadAllLines(path);
                if (lines.Length != Synapse.SynapsesCount)
                    Console.WriteLine(" Incorrect input file.");
                else
                {
                    try
                    {
                        int i = 0;
                        foreach (Layer layer in Layers)
                            foreach (Neuron neuron in layer.Neurons)
                                foreach (Synapse synapse in neuron.Inputs)
                                    synapse.Weight = Double.Parse(lines[i++]);
                    }
                    catch (Exception) { Console.WriteLine(" Incorrect input file."); }
                }
            }
            else Console.WriteLine(" File doesn't exist.");
        }

        // Testowanie sieci, które polega na sprawdzeniu czy najwyższa liczba wśród liczb uzyskanych na wyjściu jest na indeksie,
        // na którym w oczekiwanych rezultatach jest jedynka - czy zaklasyfikowano prawidłowo:
        public void CalculatePrecision(double[][][] datasets, bool shownumbers = false)
        {
            double[][] testingInputs = datasets[2], testingOutputs = datasets[3];
            List<double> outputs; int correct = 0;
            for (int i = 0; i < testingInputs.Length; i++)
            {
                PushInputValues(testingInputs[i]);
                outputs = GetOutput();
                if (shownumbers == true) Classify(testingInputs[i], testingOutputs[i], outputs);
                if (outputs.IndexOf(outputs.Max()) == testingOutputs[i].ToList().IndexOf(1)) correct += 1;
            }
            Console.WriteLine($" Precision: {(Math.Round((double)correct / testingInputs.Length, 4) * 100).ToString()}%");
        }

        // Funkcja pomocnicza wypisująca dane wejściowe, oczekiwane dane wyjściowe i uzyskane dane wyjściowe:
        private static void Classify(double[] testingInputs, double[] testingOutputs, List<double> trueOutputs)
        {
            Console.Write(" Data:      ");
            for (int i = 0; i < testingInputs.Length; i++) Console.Write(string.Format("{0, 4}", testingInputs[i].ToString("0.0")) + " ");
            Console.Write("\n Should be: ");
            for (int i = 0; i < testingOutputs.Length; i++) Console.Write(string.Format("{0, 4}", testingOutputs[i].ToString("0.0")) + " ");
            Console.Write("\n Got:       ");
            for (int i = 0; i < trueOutputs.Count; i++) Console.Write(string.Format("{0, 4}", trueOutputs[i].ToString("0.0")) + " ");
            Console.WriteLine("\n");
        }
    }
}