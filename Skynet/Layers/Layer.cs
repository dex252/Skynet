using Skynet.Model;
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
        public NeuronType NeuronType { get; }
        /// <summary>
        /// Число нейронов в слое
        /// </summary>
        public int Count => Neurons?.Count ?? 0;

        /// <summary>
        /// [List] - список нейронов с слое, [NeuronType] - тип нейронов в слое
        /// </summary>
        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
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
    }
}
