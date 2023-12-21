using System.Collections.Generic;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var height = original.GetLength(0);
            var width = original.GetLength(1);
            var whitePixelsCount = (int)(width * height * whitePixelsFraction);

            // Возвращаем чёрное изображение, если не нужно ни одного белого пикселя.
            if (whitePixelsCount == 0)
                return new double[height, width];

            var threshold = GetThreshold(original, whitePixelsCount, width, height);

            var result = new double[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    result[y, x] = original[y, x] >= threshold ? 1 : 0;
                }

            return result;
        }

        private static double GetThreshold(double[,] original, int whitePixelsCount, int width, int height)
        {
            var pixels = new List<double>();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    pixels.Add(original[y, x]);
                }

            pixels.Sort();
            pixels.Reverse();

            return pixels[whitePixelsCount - 1];
        }
    }
}