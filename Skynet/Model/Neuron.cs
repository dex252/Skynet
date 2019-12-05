using System;
using System.Collections.Generic;

namespace Skynet.Model
{
    public enum NeuronType
    {
        Input = 0,
        Normal = 1,
        Output = 2,
    }

    public class Neuron
    {
        /// <summary>
        /// Список весов
        /// </summary>
        public List<float> Weight { get; }
        /// <summary>
        /// Тип нейрона
        /// </summary>
        public NeuronType NeuronType { get; }
        /// <summary>
        /// Сумма весов
        /// </summary>
        public float Output { get; private set; }

        /// <summary>
        /// [int] - Количество входящих нейронов, [NeuronType] - тип создаваемого нейрона
        /// </summary>
        public Neuron(int inputCountNeuron, NeuronType type = NeuronType.Normal)
        {
            NeuronType = type;
            Weight = new List<float>();

            for (int i = 0; i < inputCountNeuron; i++)
            {
                Weight.Add(1);
            }
        }

        /// <summary>
        /// Возвращает вес нейрона.
        /// [List] - Список входных сигналов
        /// </summary>
        public float FeedForward(List<float> inputs)
        {
            if (Weight.Count == inputs.Count)
            {
                var sum = 0.0f;

                for (int i = 0; i < inputs.Count; i++)
                {
                    sum += inputs[i] * Weight[i];
                }

                Output = Sigmoid(sum);

                return Output;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private float Sigmoid(float x)
        {
            return 1.0f / (1.0f + (float)Math.Exp(-x));
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }   
}
