using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab2
{
    public partial class MainWindow : Window
    {
        public static int N = Int32.Parse(ConfigurationManager.AppSettings["Size"]), canvasLenght = 500;
        public int[,] Pixels = new int[N, N];
        public int CellLenght => canvasLenght / N;
        public NeuralNetwork NeuralNetwork { get; set; } = new NeuralNetwork();

        public MainWindow()
        {
            InitializeComponent();
            DrawEmptyGrid();
            InitializePixelsWithZeros();
        }

        private void CanvasConrtol_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                FillSquare(e.GetPosition(CanvasConrtol));
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DrawEmptyGrid();
            InitializePixelsWithZeros();
        }

        private void DrawEmptyGrid()
        {
            CanvasConrtol.Children.Clear();
            for (int i = 0; i <= N; i++)
            {
                Line verticalLine = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = i * CellLenght,
                    X2 = i * CellLenght,
                    Y1 = 0,
                    Y2 = canvasLenght,
                    StrokeThickness = 1
                };
                CanvasConrtol.Children.Add(verticalLine);

                Line horizontalLine = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = 0,
                    X2 = canvasLenght,
                    Y1 = i * CellLenght,
                    Y2 = i * CellLenght,
                    StrokeThickness = 1
                };
                CanvasConrtol.Children.Add(horizontalLine);
            }
        }

        private void InitializePixelsWithZeros()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Pixels[i, j] = 0;
                }
            }
        }

        private void Recognize_Click(object sender, RoutedEventArgs e)
        {
            NeuralNetwork.InputLayer = Pixels.MatrixToArray();
            NeuralNetwork.CalculateHiddenLayer();
            NeuralNetwork.CalculateOutputLayer();
            int assumptiveDigit = NeuralNetwork.GetAssumptiveResult();
            int correctDigit;
            var resultWindow = new ResultWindow(assumptiveDigit);
            if (resultWindow.ShowDialog() == true)
            {
                correctDigit = resultWindow.CorrectDigit;
                var handwrittenImage = new HandwrittenImage(Pixels, correctDigit);
                if (!handwrittenImage.IsExistsInList(NeuralNetwork.Dataset))
                {
                    NeuralNetwork.Dataset.Add(handwrittenImage);
                }
            }
        }

        private void FillSquare(Point point)
        {
            int i = Math.Min((int)(point.Y / CellLenght), N - 1);
            int j = Math.Min((int)(point.X / CellLenght), N - 1);
            Pixels[i, j] = 1;

            Rectangle rectangle = new Rectangle
            {
                Height = CellLenght,
                Width = CellLenght,
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetTop(rectangle, CellLenght * i);
            Canvas.SetLeft(rectangle, CellLenght * j);
            CanvasConrtol.Children.Add(rectangle);
        }

        private void Learn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.OK;
            while (result == MessageBoxResult.OK)
            {
                NeuralNetwork.Learn();
                var cost = NeuralNetwork.CalculateCostFunction();
                result = MessageBox.Show($"Current cost is {cost:F9}", "Learning", MessageBoxButton.OKCancel);
            }
        }

        private void TestData_Click(object sender, RoutedEventArgs e)
        {
            new TestDataWindow(NeuralNetwork.Dataset).Show();
        }

        protected override void OnClosed(EventArgs e) => NeuralNetwork.Dispose();
    }
}
