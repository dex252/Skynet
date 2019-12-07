using Skynet.Layers;
using Skynet.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skynet.Network
{
    public class NeuralNetwork
    {
        /// <summary>
        /// Топология (число нейронов на каждом слое)
        /// </summary>
        public Topology Topology { get; }
        /// <summary>
        /// Слои в нейросети
        /// </summary>
        public List<Layer> Layers { get; }
        public NeuralNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }
        /// <summary>
        /// Обучение на датасете, где Turtle - список датасетов с ожидаемым результатом и входящими данными и [int] - число эпох обучения
        /// </summary>
        public float Learn(float[] expected, float[,] inputs, int epoch)
        {
            var error = 0.0f;

            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < expected.Length; j++)
                {
                    var output = expected[j];
                    var input = GetRow(inputs, j);

                    error += Backpropagation(output, input);
                }
            }

            return (error / epoch);
        }

        /// <summary>
        /// Принимает набор входных сигналов из DataSet, число сигналов должно соответствовать топологии на первом слое
        /// </summary>
        public Neuron FeetForward(params float[] inputSignals)
        {
            if (Topology.InputCountNeurons != inputSignals.Length) throw new NotImplementedException();

            SendSignalsToInputNeurons(inputSignals);
            FeetForwardAllLayersAfterInput();

            if (Topology.OutputCountNeurons == 1)
            {
                return Layers.Last().Neurons[0];
            }
            else
            {
                return Layers.Last().Neurons.OrderByDescending((neuron) => neuron.Output).First();
            }

        }

        /// <summary>
        /// Получение строки [int] из датасета [float [,]]
        /// </summary>
        public float[] GetRow(float[,] matrix, int row)
        {
            var columns = matrix.GetLength(1);
            var array = new float[columns];

            for (int i = 0; i < columns; i++)
            {
                array[i] = matrix[row, i];
            }

            return array;
        }

        /// <summary>
        /// Нормализация данных, где [float [,]] - все входящие и отмасштабированные сигналы
        /// </summary>
        private float[,] Normalization(float[,] inputsSignals)
        {
            var result = new float[inputsSignals.GetLength(0), inputsSignals.GetLength(1)];

            for (int column = 0; column < inputsSignals.GetLength(1); column++)
            {
                var average = AverageValueOfNeuronSignal(inputsSignals, column);

                var standardDeviation = StandardDeviation(inputsSignals, column, average);

                //Новое значение сигнала
                for (int row = 0; row < inputsSignals.GetLength(0); row++)
                {
                    result[row, column] = (inputsSignals[row, column] - average) / standardDeviation;
                }

            }

            return result;
        }

        /// <summary>
        /// Вычисление стандартного среднего квадратичного отклонения нейрона
        /// </summary>
        private float StandardDeviation(float[,] inputsSignals, int column, float average)
        {
            var error = 0.0f;

            for (int row = 0; row < inputsSignals.GetLength(0); row++)
            {
                error += (float)Math.Pow((inputsSignals[row, column] - average), 2);
            }

            var standardDeviation = (float)Math.Sqrt(error / inputsSignals.GetLength(0));

            return standardDeviation;
        }

        /// <summary>
        /// Вычисление среднего значения сигнала нейрона
        /// </summary>
        private float AverageValueOfNeuronSignal(float[,] inputsSignals, int column)
        {

            var sum = 0.0f;

            for (int row = 0; row < inputsSignals.GetLength(0); row++)
            {
                sum += inputsSignals[row, column];
            }

            var average = sum / inputsSignals.GetLongLength(0);

            return average;
        }

        /// <summary>
        /// Масштабирование данных, где [float [,]] - все входящие сигналы
        /// </summary>
        private float[,] Scaling(float[,] inputsSignals)
        {
            var result = new float[inputsSignals.GetLength(0), inputsSignals.GetLength(1)];

            for (int column = 0; column < inputsSignals.GetLength(1); column++)
            {
                var min = inputsSignals[0, column];
                var max = inputsSignals[0, column];

                //находим min и max в каждом столбце
                for (int row = 1; row < inputsSignals.GetLength(0); row++)
                {
                    var item = inputsSignals[row, column];

                    if (item < min)
                    {
                        min = item;
                    }

                    if (item > max)
                    {
                        max = item;
                    }
                }

                //находим усредненное значение в пределах min и max для каждого значения в столбце
                for (int row = 1; row < inputsSignals.GetLength(0); row++)
                {
                    result[row, column] = (inputsSignals[row, column] - min) / (max - min);
                }

            }

            return result;
        }

        /// <summary>
        /// Обратное распространение ошибки, [float] - ожидаемый результат, [params float] - входящие сигналы
        /// </summary>
        private float Backpropagation(float excepted, params float[] inputs)
        {
            var actual = FeetForward(inputs).Output;

            var difference = actual - excepted;

            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.Learn(difference, Topology.LearningRate);
            }

            for (int i = Layers.Count - 2; i >= 0; i--)
            {
                var layer = Layers[i];
                var previousLayer = Layers[i + 1];

                for (int j = 0; j < layer.NeuronsCount; j++)
                {
                    var neuron = layer.Neurons[j];

                    for (int k = 0; k < previousLayer.NeuronsCount; k++)
                    {
                        var previousNeuron = previousLayer.Neurons[k];

                        var error = previousNeuron.Weights[j] * previousNeuron.Delta;
                        neuron.Learn(error, Topology.LearningRate);
                    }
                }

            }

            return (difference * difference);
        }

        /// <summary>
        /// Добавление первого слоя нейронов, у каждого всего 1 входящая связь
        /// </summary>
        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();

            for (int i = 0; i < Topology.InputCountNeurons; i++)
            {
                var neuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(neuron);
            }

            var inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }
        /// <summary>
        /// Добавление скрытых слоев нейронов, число связей равно числу нейронов в предыдущем слое
        /// </summary>
        private void CreateHiddenLayers()
        {
            for (int j = 0; j < Topology.CountNeuronsInHiddenLayers.Count; j++)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();

                for (int i = 0; i < Topology.CountNeuronsInHiddenLayers[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronsCount);
                    hiddenNeurons.Add(neuron);
                }

                var hiddentLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddentLayer);
            }
        }
       
        /// <summary>
        /// Передача сигналов из предыдущих слоев последующим
        /// </summary>
        private void FeetForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var previousLayerSignals = Layers[i - 1].GetSignals();

                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        /// <summary>
        /// Передача сигналов из DataSet на нейроны в первом слое
        /// </summary>
        private void SendSignalsToInputNeurons(params float[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                var signal = new List<float>()
                {
                    inputSignals[i]
                };

                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        /// <summary>
        /// Добавление последнего слоя нейронов, число связей равно числу нейронов в предыдущем слое
        /// </summary>
        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();

            for (int i = 0; i < Topology.OutputCountNeurons; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronsCount, NeuronType.Output);
                outputNeurons.Add(neuron);
            }

            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }
    }
}
