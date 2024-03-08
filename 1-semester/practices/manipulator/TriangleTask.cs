using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double a, double b, double c)
        {
            if (!IsValidTriangle(a, b, c))
                return double.NaN;

            var angle = CalculateCosineValue(a, b, c);

            return Math.Acos(angle);
        }

        private static bool IsValidTriangle(double a, double b, double c)
        {
            return (a + b) >= c && (b + c) >= a && (c + a) >= b;
        }

        private static double CalculateCosineValue(double a, double b, double c)
        {
            return (a * a + b * b - c * c) / (2 * a * b);
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)] // Прямоугольный треугольник, угол равен pi/2
        [TestCase(1, 1, 1, Math.PI / 3)] // Равносторонний прямоугольник, угол равен pi/3
        [TestCase(0, 2, 3, double.NaN)] // Вырожденный треугольник с a < 0
        [TestCase(2, 0, 5, double.NaN)] // Вырожденный треугольник с b < 0
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            Assert.AreEqual(TriangleTask.GetABAngle(a, b, c), expectedAngle, 1e-9);
        }
    }
}