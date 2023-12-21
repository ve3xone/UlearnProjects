using System;
using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double[,] MedianFilter(double[,] original)
        {
            int height = original.GetLength(0);
            int width = original.GetLength(1);
            var result = new double[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var neighbours = GetNeighbours(original, x, y, width, height);
                    var median = GetMedian(neighbours);
                    result[y, x] = median;
                }
            }

            return result;
        }

        private static List<double> GetNeighbours(double[,] original, int x, int y, int width, int height)
        {
            var result = new List<double>();

            int left = Math.Max(0, x - 1);
            int right = Math.Min(width - 1, x + 1);
            int top = Math.Max(0, y - 1);
            int bottom = Math.Min(height - 1, y + 1);

            for (int dy = top; dy <= bottom; dy++)
            {
                for (int dx = left; dx <= right; dx++)
                {
                    result.Add(original[dy, dx]);
                }
            }

            return result;
        }

        private static double GetMedian(List<double> values)
        {
            values.Sort();

            int middleIndex = values.Count / 2;

            if (values.Count % 2 == 1)
            {
                return values[middleIndex];
            }

            return (values[middleIndex] + values[middleIndex - 1]) / 2;
        }
    }
}
