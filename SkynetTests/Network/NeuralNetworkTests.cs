using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skynet.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            var dataset = new List<Tuple<float, float[]>>
            {
                // Результат - Пациент болен - 1
                //             Пациент Здоров - 0

                // Неправильная температура T
                // Хороший возраст A
                // Курит S
                // Правильно питается F
                //                                             T  A  S  F
                new Tuple<float, float[]>(0, new float[] {0, 0, 0, 0}),
                new Tuple<float, float[]>(0, new float[] {0, 0, 0, 1}),
                new Tuple<float, float[]>(1, new float[] {0, 0, 1, 0}),
                new Tuple<float, float[]>(0, new float[] {0, 0, 1, 1}),
                new Tuple<float, float[]>(0, new float[] {0, 1, 0, 0}),
                new Tuple<float, float[]>(0, new float[] {0, 1, 0, 1}),
                new Tuple<float, float[]>(1, new float[] {0, 1, 1, 0}),
                new Tuple<float, float[]>(0, new float[] {0, 1, 1, 1}),
                new Tuple<float, float[]>(1, new float[] {1, 0, 0, 0}),
                new Tuple<float, float[]>(1, new float[] {1, 0, 0, 1}),
                new Tuple<float, float[]>(1, new float[] {1, 0, 1, 0}),
                new Tuple<float, float[]>(1, new float[] {1, 0, 1, 1}),
                new Tuple<float, float[]>(1, new float[] {1, 1, 0, 0}),
                new Tuple<float, float[]>(0, new float[] {1, 1, 0, 1}),
                new Tuple<float, float[]>(1, new float[] {1, 1, 1, 0}),
                new Tuple<float, float[]>(1, new float[] {1, 1, 1, 1})
            };

            var topology = new Topology(0.01f, 4, 1, 2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(dataset, 400000);

            var results = new List<float>();
            foreach (var data in dataset)
            {
                results.Add(neuralNetwork.FeetForward(data.Item2).Output);
            }

            for (int i = 0; i < results.Count; i++)
            {
                var excepted = Math.Round(dataset[i].Item1, 3);
                var actual = Math.Round(results[i], 3);
                Assert.AreEqual(excepted, actual);
            }

        }
    }
}