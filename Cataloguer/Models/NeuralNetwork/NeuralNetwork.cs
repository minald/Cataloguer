using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cataloguer.Models.NeuralNetwork
{
    public class NeuralNetwork
    {
        #region Properties

        private Repository Repository { get; set; }

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
        public List<DatasetItem> Dataset { get; set; } = new List<DatasetItem>();
        public List<Bias> Biases { get; set; } = new List<Bias>();
        public List<Weight> Weights { get; set; } = new List<Weight>();

        #endregion

        public NeuralNetwork(Repository repository)
        {
            Repository = repository;
            a = 0.08f;
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
                List<Weight> weightsInputHidden = Weights.Where(w => w.FromLayer == 0).ToList();
                for (int i = 0; i < InputLayerLength; i++)
                {
                    for (int j = 0; j < HiddenLayerLength; j++)
                    {
                        WeightsInputHidden[i, j] = weightsInputHidden
                            .FirstOrDefault(w => w.FromNumber == i && w.ToNumber == j)?.Value ?? 0;
                    }
                }

                List<Weight> weightsHiddenOutput = Weights.Where(w => w.FromLayer == 1).ToList();
                for (int i = 0; i < HiddenLayerLength; i++)
                {
                    List<Weight> weightsFromI = weightsHiddenOutput.Where(w => w.FromNumber == i).ToList();
                    for (int j = 0; j < OutputLayerLength; j++)
                    {
                        WeightsHiddenOutput[i, j] = weightsFromI
                            .FirstOrDefault(w => w.ToNumber == j)?.Value ?? 0;
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

        public List<KeyValuePair<Track, float>> GetAssumptiveRating(ApplicationUser user)
        {
            InputLayer = user.GetInputData();
            CalculateHiddenLayer();
            CalculateOutputLayer();

            Dictionary<Track, float> assumptiveRating = new Dictionary<Track, float>();
            List<Track> tracks = Repository.GetTracks().OrderBy(t => t.Id).ToList();
            for (int i = 0; i < OutputLayerLength; i++)
            {
                assumptiveRating.Add(tracks[i], OutputLayer[i]);
            }

            return assumptiveRating.OrderByDescending(r => r.Value).Take(20).ToList();
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
            int iterationsAmount = 10000; // This parameter can be configurable
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

        public /*async Task*/void SaveAsync()
        {
            Repository.RemoveAllBiasesAndWeights();

            List<Bias> biasesHidden = ConvertFromArray(BiasesHidden, 1).ToList();
            List<Weight> weightsInputHidden = ConvertFromMatrix(WeightsInputHidden, 0).ToList();
            /*await*/ Repository.AddBiasesAndWeightsAsync(biasesHidden, weightsInputHidden);

            List<Bias> biasesOutput = ConvertFromArray(BiasesOutput, 2).ToList();
            List<Weight> weightsHiddenOutput = ConvertFromMatrix(WeightsHiddenOutput, 1).ToList();
            /*await*/ Repository.AddBiasesAndWeightsAsync(biasesOutput, weightsHiddenOutput);
        }

        private IEnumerable<Bias> ConvertFromArray(float[] biases, byte layer)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                yield return new Bias(layer, i, biases[i]);
            }
        }

        private IEnumerable<Weight> ConvertFromMatrix(float[,] weights, byte layer)
        {
            int n = weights.GetLength(0);
            int m = weights.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    yield return new Weight(layer, i, j, weights[i, j]);
                }
            }
        }
    }
}
