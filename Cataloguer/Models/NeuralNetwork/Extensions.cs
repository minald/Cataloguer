﻿using System;
using System.IO;
using System.Linq;

namespace lab2
{
    public static class Extensions
    {
        private static Random Random = new Random();

        public static void InitializeRandomMatrix(this double[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = Random.NextDouble() * 6 - 3;
                }
            }
        }

        public static void InitializeRandomVector(this double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = Random.NextDouble() - 0.5;
            }
        }

        public static int[] MatrixToArray(this int[,] matrix) => matrix.Cast<int>().ToArray();

        public static T[,] GetMatrixCopy<T>(this T[,] matrix)
        {
            T[,] newArray = new T[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    newArray[i, j] = matrix[i, j];
                }
            }
            
            return newArray;
        }
    }
}
