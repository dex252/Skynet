using System;
using System.Collections.Generic;

namespace Skynet.Model
{
    public enum NeuronType
    {
        Input = 0,
        Hidden = 1,
        Output = 2,
    }

    public class Neuron
    {
        /// <summary>
        /// Список весов
        /// </summary>
        public List<float> Weights { get; }
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
        public Neuron(int inputCountNeuron, NeuronType type = NeuronType.Hidden)
        {
            Weights = new List<float>();
            NeuronType = type;

            for (int i = 0; i < inputCountNeuron; i++)
            {
                Weights.Add(1);
            }
        }

        /// <summary>
        /// УДАЛИТЬ после введения возможности обучения
        /// </summary>
        public void SetWeights(params float[] weights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                Weights[i] = weights[i];
            }
        }

        /// <summary>
        /// Возвращает вес нейрона.
        /// [List] - Список входных сигналов
        /// </summary>
        public float FeedForward(List<float> inputs)
        {
            if (Weights.Count == inputs.Count)
            {
                var sum = 0.0f;

                for (int i = 0; i < inputs.Count; i++)
                {
                    sum += inputs[i] * Weights[i];
                }

                if (NeuronType != NeuronType.Input)
                {
                    Output = Sigmoid(sum);
                }
                else
                {
                    Output = sum;
                }
               
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
