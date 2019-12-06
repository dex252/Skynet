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
        /// Список входящих сигналов
        /// </summary>
        public List<float> InputSignals { get; }
        /// <summary>
        /// Тип нейрона
        /// </summary>
        public NeuronType NeuronType { get; }
        /// <summary>
        /// Сумма весов
        /// </summary>
        public float Output { get; private set; }
        /// <summary>
        /// Ошибка
        /// </summary>
        public float Delta { get; private set; }

        /// <summary>
        /// [int] - Количество входящих нейронов, [NeuronType] - тип создаваемого нейрона
        /// </summary>
        public Neuron(int inputCountNeuron, NeuronType type = NeuronType.Hidden)
        {
            Weights = new List<float>();
            InputSignals = new List<float>();
            NeuronType = type;

            InitWeightsRandomValues(inputCountNeuron);
        }

        private void InitWeightsRandomValues(int inputCountNeuron)
        {
            var rand = new Random();

            for (int i = 0; i < inputCountNeuron; i++)
            {
                Weights.Add((float)rand.NextDouble());
                InputSignals.Add(0);
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
                    InputSignals[i] = inputs[i];

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

        private float SigmoidDx(float x)
        {
            var sigmoid = Sigmoid(x);
            var result =  sigmoid / (1 - sigmoid);

            return result;
        }

        public void Learn(float error, float learningRate)
        {
            if (NeuronType != NeuronType.Input)
            {
                Delta = error * SigmoidDx(Output);

                for (int i = 0; i < Weights.Count; i++)
                {
                    var weight = Weights[i];
                    var input = InputSignals[i];

                    var newWeight = weight - input * Delta * learningRate;
                    Weights[i] = newWeight;
                }

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
