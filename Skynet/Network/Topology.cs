using System.Collections.Generic;

namespace Skynet.Network
{
    /// <summary>
    /// Топология нейросети
    /// </summary>
    public abstract class Topology
    {
        /// <summary>
        /// Число нейронов на первом слое
        /// </summary>
        public int InputCountNeurons { get; }
        /// <summary>
        /// Число нейронов на последнем слое
        /// </summary>
        public int OutputCountNeurons { get; }
        /// <summary>
        /// Число нейронов на каждом скрытом слое
        /// </summary>
        public List<int> CountNeuronsInHiddenLayers { get; }

        public Topology(int inputCountNeurons, int outputCountNeurons, params int[] layers)
        {
            InputCountNeurons = inputCountNeurons;
            OutputCountNeurons = outputCountNeurons;

            CountNeuronsInHiddenLayers = new List<int>(); 
            CountNeuronsInHiddenLayers.AddRange(layers);
        }
    }
}
