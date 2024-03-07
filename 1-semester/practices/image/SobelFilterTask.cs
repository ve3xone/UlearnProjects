using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var filterSize = sx.GetLength(0);

            var result = new double[width, height];

            for (int x = filterSize / 2; x < width - filterSize / 2; x++)
                for (int y = filterSize / 2; y < height - filterSize / 2; y++)
                {
                    result[x, y] = CalculateGradient(g, sx, x, y, filterSize);
                }

            return result;
        }

        private static double CalculateGradient(double[,] g, double[,] filter, int x, int y, int filterSize)
        {
            double gx = 0;
            double gy = 0;

            for (int i = 0; i < filterSize; i++)
                for (int j = 0; j < filterSize; j++)
                {
                    gx += filter[i, j] * g[x - filterSize / 2 + i, y - filterSize / 2 + j];
                    gy += filter[j, i] * g[x - filterSize / 2 + i, y - filterSize / 2 + j];
                }

            return Math.Sqrt(gx * gx + gy * gy);
        }
    }
}