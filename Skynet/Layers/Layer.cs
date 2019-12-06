using Skynet.Model;
using System;
using System.Collections.Generic;

namespace Skynet.Layers
{
    public class Layer
    {
        /// <summary>
        /// Список нейронов в слое
        /// </summary>
        public List<Neuron> Neurons { get; }
        /// <summary>
        /// Тип нейрона
        /// </summary>
        public int NeuronsCount => Neurons?.Count ?? 0;
        public NeuronType NeuronType { get; }
        /// <summary>
        /// [List] - список нейронов с слое, [NeuronType] - тип нейронов в слое
        /// </summary>
        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Hidden)
        {
            //TODO: проверка на тип нейронов и тип слоя
            foreach (var neuron in neurons)
            {
                if (neuron.NeuronType != type) throw new NotImplementedException();
            }

            NeuronType = type;

            Neurons = neurons;
        }

        /// <summary>
        /// Возвращает коллекцию сигналов в слое для дальнейшей передачи следущему слою
        /// </summary>
        /// 
        public List<float> GetSignals()
        {
            var result = new List<float>();

            foreach (var neuron in Neurons)
            {
                result.Add(neuron.Output);
            }

            return result;
        }

        public override string ToString()
        {
            return NeuronType.ToString();
        }
    }
}
