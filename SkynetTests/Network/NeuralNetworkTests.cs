using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Skynet.Network.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeetForwardTest()
        {
            var outputs = new float[]
            {
                0,0,1,0,0,0,1,0,1,1,1,1,1,0,1,1
            };
            var inputs = new float[,]
            {
                {0,0,0,0 },
                {0,0,0,1 },
                {0,0,1,0 },
                {0,0,1,1 },
                {0,1,0,0 },
                {0,1,0,1 },
                {0,1,1,0 },
                {0,1,1,1 },
                {1,0,0,0 },
                {1,0,0,1 },
                {1,0,1,0 },
                {1,0,1,1 },
                {1,1,0,0 },
                {1,1,0,1 },
                {1,1,1,0 },
                {1,1,1,1 },
            };

            var topology = new Topology(0.01f, 4, 1, 2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs, inputs, 100000);

            var results = new List<float>();
      

            for (int i = 0; i < outputs.Length; i++)
            {
                var row = neuralNetwork.GetRow(inputs, i);
                var res = neuralNetwork.FeetForward(row).Output;
                results.Add(res);
            }

            for (int i = 0; i < results.Count; i++)
            {
                var excepted = Math.Round(outputs[i], 2);
                var actual = Math.Round(results[i], 2);
                Assert.AreEqual(excepted, actual);
            }

        }
    }
}