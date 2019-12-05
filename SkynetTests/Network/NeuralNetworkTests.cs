using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skynet.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skynet.Network.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeetForwardTest()
        {
         
            var topology = new Topology(4, 1, 2);
            var neuralNetwork = new NeuralNetwork(topology);
            neuralNetwork.Layers[1].Neurons[0].SetWeights(0.5f, -0.1f, 0.3f, -0.1f);
            neuralNetwork.Layers[1].Neurons[1].SetWeights(0.1f, -0.3f, 0.7f, -0.3f);
            neuralNetwork.Layers[2].Neurons[0].SetWeights(1.2f, 0.8f);

            var result = neuralNetwork.FeetForward(new List<float>()
            {
                0,1,0,1
            });
        }
    }
}