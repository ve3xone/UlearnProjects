using System;
using static System.Math;

namespace Fractals
{
    internal static class DragonFractalTask
    {
        private const double PiOver4 = Math.PI / 4;
        private const double ThreePiOver4 = 3 * Math.PI / 4;

        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
        {
            double x = 1;
            double y = 0;

            var random = new Random(seed);

            for (var i = 0; i < iterationsCount; i++)
            {
                var nextNumber = random.Next(2);
                if (nextNumber == 0)
                    (x, y) = GetTransForm(x, y, PiOver4);
                else
                {
                    (x, y) = GetTransForm(x, y, ThreePiOver4);
                    x += 1;
                }
                pixels.SetPixel(x, y);
            }
        }

        private static (double newX, double newY) GetTransForm(double x, double y, double angle)
        {
            var newX = (x * Cos(angle) - y * Sin(angle)) / Sqrt(2);
            var newY = (x * Sin(angle) + y * Cos(angle)) / Sqrt(2);
            return (newX, newY);
        }
    }
}