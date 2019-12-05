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
        public List <Layer> Layers { get; }

        public NeuralNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        /// <summary>
        /// Добавление первого слоя нейронов, у каждого всего 1 входящая связь
        /// </summary>
        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();

            for (int i = 0; i < Topology.InputCountNeurons; i++)
            {
                var neuron = new Neuron(1);
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
                    var neuron = new Neuron(lastLayer.Count);
                    hiddenNeurons.Add(neuron);
                }

                var hiddentLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddentLayer);
            }
        }

        /// <summary>
        /// Принимает набор входных сигналов из DataSet, число сигналов должно соответствовать топологии на первом слое
        /// </summary>
        public Neuron FeetForward(List<float> inputSignals)
        {
            if (Topology.InputCountNeurons != inputSignals.Count) throw new NotImplementedException();

            SendSignalsToInputNeurons(inputSignals);
            FeetForwardAllLayersAfterInput();

            if (Topology.OutputCountNeurons == 1)
            {
                return Layers.Last().Neurons[0];
            }
            else
            {
                return Layers.Last().Neurons.OrderByDescending( (neuron) => neuron.Output).First();
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
        private void SendSignalsToInputNeurons(List<float> inputSignals)
        {
            for (int i = 0; i < inputSignals.Count; i++)
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
                var neuron = new Neuron(lastLayer.Count);
                outputNeurons.Add(neuron);
            }

            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }
    }
}
