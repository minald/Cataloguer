using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace lab2
{
    public class NeuralNetwork : IDisposable
    {
        #region Properties

        private static readonly string CurrentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private readonly string StoragePath = Path.Combine(CurrentDirectory, $"Weights_{MainWindow.N}x{MainWindow.N}_Output{OutputLayerLength}.json");
        private readonly string DatasetPath = Path.Combine(CurrentDirectory, $"Dataset_{MainWindow.N}x{MainWindow.N}_Output{OutputLayerLength}.json");

        /// <summary>
        /// Learning rate
        /// </summary>
        public static double a = 0.05;
        public static int InputLayerLength = MainWindow.N * MainWindow.N;
        public static int HiddenLayerLength = 12;
        public static int OutputLayerLength = Int32.Parse(ConfigurationManager.AppSettings["OutputLayerLength"]);
        public int[] InputLayer = new int[InputLayerLength];
        public double[] HiddenLayer = new double[HiddenLayerLength];
        public double[] ExpectedHidden = new double[HiddenLayerLength];
        public double[] OutputLayer = new double[OutputLayerLength];
        public double[] ExpectedOutput = new double[OutputLayerLength];
        public double[,] WeightsInputHidden = new double[InputLayerLength, HiddenLayerLength];
        public double[] BiasesHidden = new double[HiddenLayerLength];
        public double[,] WeightsHiddenOutput = new double[HiddenLayerLength, OutputLayerLength];
        public double[] BiasesOutput = new double[OutputLayerLength];
        public List<HandwrittenImage> Dataset = new List<HandwrittenImage>();

        #endregion

        public NeuralNetwork()
        {
            LoadWeightsAndBiases();
            LoadDataset();
        }

        private void LoadWeightsAndBiases()
        {
            string jsonContent = File.Exists(StoragePath) ? File.ReadAllText(StoragePath) : String.Empty;
            if (String.IsNullOrWhiteSpace(jsonContent))
            {
                WeightsInputHidden.InitializeRandomMatrix();
                BiasesHidden.InitializeRandomVector();
                WeightsHiddenOutput.InitializeRandomMatrix();
                BiasesOutput.InitializeRandomVector();
            }
            else
            {
                string[] weightsAndBiases = jsonContent.Split(new string[]{ Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                WeightsInputHidden = JsonConvert.DeserializeObject<double[,]>(weightsAndBiases[0]);
                BiasesHidden = JsonConvert.DeserializeObject<double[]>(weightsAndBiases[1]);
                WeightsHiddenOutput = JsonConvert.DeserializeObject<double[,]>(weightsAndBiases[2]);
                BiasesOutput = JsonConvert.DeserializeObject<double[]>(weightsAndBiases[3]);
            }
        }

        private void LoadDataset()
        {
            if (File.Exists(DatasetPath))
            {
                string jsonContent = File.ReadAllText(DatasetPath);
                Dataset = JsonConvert.DeserializeObject<List<HandwrittenImage>>(jsonContent);
            }
        }

        public void CalculateHiddenLayer()
        {
            for (int j = 0; j < HiddenLayerLength; j++)
            {
                double result = 0;
                for (int i = 0; i < InputLayerLength; i++)
                {
                    result += InputLayer[i] * WeightsInputHidden[i, j];
                }

                result += BiasesHidden[j];
                HiddenLayer[j] = CalculateSigmoid(result);
            }
        }

        public void CalculateOutputLayer()
        {
            for (int j = 0; j < OutputLayerLength; j++)
            {
                double result = 0;
                for (int i = 0; i < HiddenLayerLength; i++)
                {
                    result += HiddenLayer[i] * WeightsHiddenOutput[i, j];
                }

                result += BiasesOutput[j];
                OutputLayer[j] = CalculateSigmoid(result);
            }
        }

        public int GetAssumptiveResult()
        {
            double maximum = OutputLayer.Max();
            return Array.FindIndex(OutputLayer, x => x == maximum);
        }

        private double CalculateSigmoid(double x) => 1 / (1 + Math.Exp(-x));

        public double CalculateCostFunction()
        {
            double value = 0;
            foreach (var handwrittenImage in Dataset)
            {
                InputLayer = handwrittenImage.Pixels.MatrixToArray();
                CalculateHiddenLayer();
                CalculateOutputLayer();
                InitializeExpectedResults(handwrittenImage.Digit);
                for (int i = 0; i < OutputLayerLength; i++)
                {
                    value += Math.Pow(OutputLayer[i] - ExpectedOutput[i], 2);
                }
            }

            return value / Dataset.Count;
        }

        private void InitializeExpectedResults(int correctDigit)
        {
            ExpectedOutput = Enumerable.Repeat(0.0, OutputLayerLength).ToArray();
            ExpectedOutput[correctDigit] = 1;
        }

        public void Learn()
        {
            int iterationsAmount = Int32.Parse(ConfigurationManager.AppSettings["LearningIterations"]);
            for (int iteration = 0; iteration < iterationsAmount; iteration++)
            {
                foreach (var handwrittenImage in Dataset)
                {
                    InputLayer = handwrittenImage.Pixels.MatrixToArray();
                    CalculateHiddenLayer();
                    CalculateOutputLayer();
                    InitializeExpectedResults(handwrittenImage.Digit);

                    ExpectedHidden = Enumerable.Repeat(0.0, HiddenLayerLength).ToArray();
                    for (int j = 0; j < OutputLayerLength; j++)
                    {
                        double currentOutputNeuron = OutputLayer[j];
                        double DcDa = 2 * (currentOutputNeuron - ExpectedOutput[j]);
                        double DaDz = currentOutputNeuron * (1 - currentOutputNeuron);
                        for (int i = 0; i < HiddenLayerLength; i++)
                        {
                            double DzDw = HiddenLayer[i];
                            double DcDw = DcDa * DaDz * DzDw;
                            WeightsHiddenOutput[i, j] -= a * DcDw;

                            double DcDb = DcDa * DaDz;
                            BiasesOutput[j] -= a * DcDb;

                            double DzDaMinus1 = WeightsHiddenOutput[i, j];
                            double DcDaMinus1 = DcDa * DaDz * DzDaMinus1;
                            ExpectedHidden[i] -= DcDaMinus1;
                        }
                    }
                    for (int j = 0; j < HiddenLayerLength; j++)
                    {
                        double currentHiddenNeuron = HiddenLayer[j];
                        double DcDa = -2 * ExpectedHidden[j];
                        double DaDz = currentHiddenNeuron * (1 - currentHiddenNeuron);
                        for (int i = 0; i < InputLayerLength; i++)
                        {
                            double DzDw = InputLayer[i];
                            double DcDw = DcDa * DaDz * DzDw;
                            WeightsInputHidden[i, j] -= a * DcDw;

                            double DcDb = DcDa * DaDz;
                            BiasesHidden[j] -= a * DcDb;
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            string weightsAndBiases = JsonConvert.SerializeObject(WeightsInputHidden) + Environment.NewLine + 
                JsonConvert.SerializeObject(BiasesHidden) + Environment.NewLine +
                JsonConvert.SerializeObject(WeightsHiddenOutput) + Environment.NewLine +
                JsonConvert.SerializeObject(BiasesOutput);
            File.WriteAllText(StoragePath, weightsAndBiases);

            string dataset = JsonConvert.SerializeObject(Dataset);
            File.WriteAllText(DatasetPath, dataset);
        }
    }
}
