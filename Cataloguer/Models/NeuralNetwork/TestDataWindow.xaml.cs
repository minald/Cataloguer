using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace lab2
{
    public partial class TestDataWindow : Window
    {
        public TestDataWindow(List<HandwrittenImage> images)
        {
            InitializeComponent();
            TvBox.ItemsSource = images.Select(x => new ViewData(x.Pixels, x.Digit));
        }
    }

    public class ViewData
    {
        public BitmapImage Image { get; set; }
        public int Title { get; set; }

        public ViewData(int[,] pixels, int title)
        {
            Image = GetExpandedBitmapFromMatrix(pixels, 100 / pixels.GetLength(0)).ToBitmapImage();
            Title = title;
        }

        public Bitmap GetExpandedBitmapFromMatrix(int[,] pixels, int expansionDegree)
        {
            int n = pixels.GetLength(0);
            int m = pixels.GetLength(1);
            Bitmap bitmap = new Bitmap(n * expansionDegree, m * expansionDegree);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    var color = pixels[i, j] == 0 ? Color.White : Color.Black;
                    for (int iInternal = 0; iInternal < expansionDegree; iInternal++)
                    {
                        for (int jInternal = 0; jInternal < expansionDegree; jInternal++)
                        {
                            bitmap.SetPixel(j * expansionDegree + jInternal, i * expansionDegree + iInternal, color);
                        }
                    }
                }
            }

            return bitmap;
        }
    }
}
