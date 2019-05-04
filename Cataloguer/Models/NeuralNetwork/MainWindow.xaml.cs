﻿namespace lab2
{
    public partial class MainWindow
    {
        public static int N = 10; //Int32.Parse(ConfigurationManager.AppSettings["Size"]);
        public static int canvasLenght = 500;
        public int[,] Pixels = new int[N, N];
        public int CellLenght => canvasLenght / N;
        public NeuralNetwork NeuralNetwork { get; set; } = new NeuralNetwork();

        private void Recognize_Click(object sender)
        {
            NeuralNetwork.InputLayer = Pixels.MatrixToArray();
            NeuralNetwork.CalculateHiddenLayer();
            NeuralNetwork.CalculateOutputLayer();
            int assumptiveDigit = NeuralNetwork.GetAssumptiveResult();
            int correctDigit = 1; // Something that a human think
            var handwrittenImage = new HandwrittenImage(Pixels, correctDigit);
            if (!handwrittenImage.IsExistsInList(NeuralNetwork.Dataset))
            {
                NeuralNetwork.Dataset.Add(handwrittenImage);
            }
        }

        private void Learn_Click(object sender)
        {
            NeuralNetwork.Learn();
            var cost = NeuralNetwork.CalculateCostFunction();
        }
    }
}
