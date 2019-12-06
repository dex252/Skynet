using Skynet.Layers;
using Skynet.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skynet.Network
{
    public class NeuralNetwork
    {
        /// <summary>
        /// Топология (число нейронов на каждом слое)
        /// </summary>
        public Topology Topology { get; }
        /// <summary>
        /// Слои в нейросети
        /// </summary>
        public List<Layer> Layers { get; }
        public NeuralNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }
        /// <summary>
        /// Обучение на датасете, где Turtle - список датасетов с ожидаемым результатом и входящими данными и [int] - число эпох обучения
        /// </summary>
        public float Learn(List<Tuple<float, float[]>> dataset, int epoch)
        {
            var error = 0.0f;

            for (int i = 0; i < epoch; i++)
            {
                foreach (var data in dataset)
                {
                    error += Backpropagation(data.Item1, data.Item2);
                }
            }

            return (error / epoch);
        }

        /// <summary>
        /// Обратное распространение ошибки, [float] - ожидаемый результат, [params float] - входящие сигналы
        /// </summary>
        private float Backpropagation(float excepted, params float[] inputs)
        {
            var actual = FeetForward(inputs).Output;

            var difference = actual - excepted;

            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.Learn(difference, Topology.LearningRate);
            }

            for (int i = Layers.Count - 2; i >= 0; i--)
            {
                var layer = Layers[i];
                var previousLayer = Layers[i + 1];

                for (int j = 0; j < layer.NeuronsCount; j++)
                {
                    var neuron = layer.Neurons[j];

                    for (int k = 0; k < previousLayer.NeuronsCount; k++)
                    {
                        var previousNeuron = previousLayer.Neurons[k];

                        var error = previousNeuron.Weights[j] * previousNeuron.Delta;
                        neuron.Learn(error, Topology.LearningRate);
                    }
                }

            }

            return (difference * difference);
        }

        /// <summary>
        /// Добавление первого слоя нейронов, у каждого всего 1 входящая связь
        /// </summary>
        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();

            for (int i = 0; i < Topology.InputCountNeurons; i++)
            {
                var neuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(neuron);
            }

            var inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }
        /// <summary>
        /// Добавление скрытых слоев нейронов, число связей равно числу нейронов в предыдущем слое
        /// </summary>
        private void CreateHiddenLayers()
        {
            for (int j = 0; j < Topology.CountNeuronsInHiddenLayers.Count; j++)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();

                for (int i = 0; i < Topology.CountNeuronsInHiddenLayers[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronsCount);
                    hiddenNeurons.Add(neuron);
                }

                var hiddentLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddentLayer);
            }
        }
        /// <summary>
        /// Принимает набор входных сигналов из DataSet, число сигналов должно соответствовать топологии на первом слое
        /// </summary>
        public Neuron FeetForward(params float[] inputSignals)
        {
            if (Topology.InputCountNeurons != inputSignals.Length) throw new NotImplementedException();

            SendSignalsToInputNeurons(inputSignals);
            FeetForwardAllLayersAfterInput();

            if (Topology.OutputCountNeurons == 1)
            {
                return Layers.Last().Neurons[0];
            }
            else
            {
                return Layers.Last().Neurons.OrderByDescending((neuron) => neuron.Output).First();
            }

        }
        /// <summary>
        /// Передача сигналов из предыдущих слоев последующим
        /// </summary>
        private void FeetForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var previousLayerSignals = Layers[i - 1].GetSignals();

                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        /// <summary>
        /// Передача сигналов из DataSet на нейроны в первом слое
        /// </summary>
        private void SendSignalsToInputNeurons(params float[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<float>()
                {
                    inputSignals[i]
                };

                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        /// <summary>
        /// Добавление последнего слоя нейронов, число связей равно числу нейронов в предыдущем слое
        /// </summary>
        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();

            for (int i = 0; i < Topology.OutputCountNeurons; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronsCount, NeuronType.Output);
                outputNeurons.Add(neuron);
            }

            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }
    }
}
