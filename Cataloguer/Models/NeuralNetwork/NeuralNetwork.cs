using Cataloguer.Models;
using Cataloguer.Models.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class NeuralNetwork
    {
        #region Properties

        /// <summary>
        /// Learning rate
        /// </summary>
        public float a;
        public int InputLayerLength;
        public int HiddenLayerLength;
        public int OutputLayerLength;
        public float[] InputLayer;
        public float[] HiddenLayer;
        public float[] ExpectedHidden;
        public float[] OutputLayer;
        public float[] ExpectedOutput;
        public float[,] WeightsInputHidden;
        public float[] BiasesHidden;
        public float[,] WeightsHiddenOutput;
        public float[] BiasesOutput;
        public List<DatasetItem> Dataset = new List<DatasetItem>();
        public List<Bias> Biases { get; set; } = new List<Bias>();
        public List<Weight> Weights { get; set; } = new List<Weight>();

        #endregion

        public NeuralNetwork(Repository repository)
        {
            a = 0.05f;
            InputLayerLength = 7;
            HiddenLayerLength = 100;
            OutputLayerLength = repository.GetTracksAmount();
            InputLayer = new float[InputLayerLength];
            HiddenLayer = new float[HiddenLayerLength];
            ExpectedHidden = new float[HiddenLayerLength];
            OutputLayer = new float[OutputLayerLength];
            ExpectedOutput = new float[OutputLayerLength];

            Biases = repository.GetBiases();
            Weights = repository.GetWeights();
            LoadBiases();
            LoadWeights();
            List<Rating> ratings = repository.GetRatings();
            List<Track> tracks = repository.GetTracks();
            LoadDataset(ratings, tracks);
        }

        private void LoadBiases()
        {
            BiasesHidden = new float[HiddenLayerLength];
            BiasesOutput = new float[OutputLayerLength];
            if (Biases.Count == 0)
            {
                BiasesHidden.InitializeRandomVector();
                BiasesOutput.InitializeRandomVector();
            }
            else
            {
                BiasesHidden = Biases.Where(b => b.Layer == 1).OrderBy(b => b.Number).Select(b => b.Value).ToArray();
                BiasesOutput = Biases.Where(b => b.Layer == 2).OrderBy(b => b.Number).Select(b => b.Value).ToArray();
            }
        }

        private void LoadWeights()
        {
            WeightsInputHidden = new float[InputLayerLength, HiddenLayerLength];
            WeightsHiddenOutput = new float[HiddenLayerLength, OutputLayerLength];
            if (Weights.Count == 0)
            {
                WeightsInputHidden.InitializeRandomMatrix();
                WeightsHiddenOutput.InitializeRandomMatrix();
            }
            else
            {
                IEnumerable<Weight> weightsInputHidden = Weights.Where(w => w.FromLayer == 0);
                for (int i = 0; i < InputLayerLength; i++)
                {
                    for (int j = 0; j < HiddenLayerLength; j++)
                    {
                        WeightsInputHidden[i, j] = weightsInputHidden
                            .FirstOrDefault(w => w.FromNumber == i && w.ToNumber == j)?.Value ?? 0;
                    }
                }

                IEnumerable<Weight> weightsHiddenOutput = Weights.Where(w => w.FromLayer == 1);
                for (int i = 0; i < HiddenLayerLength; i++)
                {
                    for (int j = 0; j < OutputLayerLength; j++)
                    {
                        WeightsHiddenOutput[i, j] = weightsHiddenOutput
                            .FirstOrDefault(w => w.FromNumber == i && w.ToNumber == j)?.Value ?? 0;
                    }
                }
            }
        }

        private void LoadDataset(List<Rating> ratings, List<Track> tracks)
        {
            IEnumerable<ApplicationUser> users = ratings.Select(r => r.ApplicationUser).Distinct();
            foreach (ApplicationUser user in users)
            {
                List<Rating> userRatings = ratings.Where(r => r.ApplicationUser.Id == user.Id).ToList();
                Dataset.Add(new DatasetItem(user, userRatings, tracks, OutputLayerLength));
            }
        }

        public void CalculateHiddenLayer()
        {
            for (int j = 0; j < HiddenLayerLength; j++)
            {
                float result = 0;
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
                float result = 0;
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

        private float CalculateSigmoid(float x) => 1 / (float)(1 + Math.Exp(-x));

        public double CalculateCostFunction()
        {
            double value = 0;
            foreach (var datasetItem in Dataset)
            {
                InputLayer = datasetItem.InputData;
                CalculateHiddenLayer();
                CalculateOutputLayer();
                ExpectedOutput = datasetItem.OutputData;
                for (int i = 0; i < OutputLayerLength; i++)
                {
                    value += Math.Pow(OutputLayer[i] - ExpectedOutput[i], 2);
                }
            }

            return value / Dataset.Count;
        }

        public void Learn()
        {
            int iterationsAmount = 100; // This parameter can be configurable
            for (int iteration = 0; iteration < iterationsAmount; iteration++)
            {
                foreach (var datasetItem in Dataset)
                {
                    InputLayer = datasetItem.InputData;
                    CalculateHiddenLayer();
                    CalculateOutputLayer();
                    ExpectedOutput = datasetItem.OutputData;

                    ExpectedHidden = Enumerable.Repeat(0.0f, HiddenLayerLength).ToArray();
                    for (int j = 0; j < OutputLayerLength; j++)
                    {
                        float currentOutputNeuron = OutputLayer[j];
                        float DcDa = 2 * (currentOutputNeuron - ExpectedOutput[j]);
                        float DaDz = currentOutputNeuron * (1 - currentOutputNeuron);
                        for (int i = 0; i < HiddenLayerLength; i++)
                        {
                            float DzDw = HiddenLayer[i];
                            float DcDw = DcDa * DaDz * DzDw;
                            WeightsHiddenOutput[i, j] -= a * DcDw;

                            float DcDb = DcDa * DaDz;
                            BiasesOutput[j] -= a * DcDb;

                            float DzDaMinus1 = WeightsHiddenOutput[i, j];
                            float DcDaMinus1 = DcDa * DaDz * DzDaMinus1;
                            ExpectedHidden[i] -= DcDaMinus1;
                        }
                    }
                    for (int j = 0; j < HiddenLayerLength; j++)
                    {
                        float currentHiddenNeuron = HiddenLayer[j];
                        float DcDa = -2 * ExpectedHidden[j];
                        float DaDz = currentHiddenNeuron * (1 - currentHiddenNeuron);
                        for (int i = 0; i < InputLayerLength; i++)
                        {
                            float DzDw = InputLayer[i];
                            float DcDw = DcDa * DaDz * DzDw;
                            WeightsInputHidden[i, j] -= a * DcDw;

                            float DcDb = DcDa * DaDz;
                            BiasesHidden[j] -= a * DcDb;
                        }
                    }
                }
            }
        }

        //public void Dispose()
        //{
        //    string weightsAndBiases = JsonConvert.SerializeObject(WeightsInputHidden) + Environment.NewLine + 
        //        JsonConvert.SerializeObject(BiasesHidden) + Environment.NewLine +
        //        JsonConvert.SerializeObject(WeightsHiddenOutput) + Environment.NewLine +
        //        JsonConvert.SerializeObject(BiasesOutput);
        //    File.WriteAllText(StoragePath, weightsAndBiases);

        //    string dataset = JsonConvert.SerializeObject(Dataset);
        //    File.WriteAllText(DatasetPath, dataset);
        //}
    }
}
