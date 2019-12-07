using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        [TestMethod()]
        public void DataSetTest()
        {
            var outputs = new List<float>();
            var inputs = new List<float[]>();
            using (var sr = new StreamReader("heart.csv"))
            {
                var header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var values = row.Split(',').Select(data => (float)Convert.ToDouble(data.Replace(".",","))).ToList();

                    var output = values.Last();
                    var input = values.Take(values.Count - 1).ToArray();

                    outputs.Add(output);
                    inputs.Add(input);
                }
            }

            var inputsSignals = new float[inputs.Count, inputs[0].Length];
            for (int i = 0; i < inputsSignals.GetLength(0); i++)
            {
                for (int j = 0; j < inputsSignals.GetLength(1); j++)
                {
                    inputsSignals[i, j] = inputs[i][j];
                }
            }

            var topology = new Topology(0.01f, inputs[0].Length, 1, inputs[0].Length/2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs.ToArray(), inputsSignals, 1000);

            var results = new List<float>();


            for (int i = 0; i < outputs.Count; i++)
            {
                var row = inputs[i];
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