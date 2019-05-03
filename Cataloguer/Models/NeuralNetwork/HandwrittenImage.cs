using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class HandwrittenImage
    {
        public static int N = MainWindow.N;
        public int[,] Pixels { get; set; } = new int[N, N];
        public int Digit { get; set; }

        public HandwrittenImage(int[,] pixels, int digit)
        {
            Pixels = pixels.GetMatrixCopy();
            Digit = digit;
        }

        public bool IsExistsInList(List<HandwrittenImage> images)
        {
            return images.Select(x => String.Join("", x.Pixels.MatrixToArray())).Contains(String.Join("", Pixels.MatrixToArray()));
        }
    }
}
