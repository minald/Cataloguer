using System;

namespace Cataloguer.Models.NeuralNetwork
{
    public static class Extensions
    {
        private static Random Random = new Random();

        public static void InitializeRandomMatrix(this float[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = (float)Random.NextDouble() * 6 - 3;
                }
            }
        }

        public static void InitializeRandomVector(this float[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = (float)Random.NextDouble() - 0.5f;
            }
        }
    }
}
