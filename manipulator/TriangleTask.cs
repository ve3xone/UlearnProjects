using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double a, double b, double c)
        {
            if (a < 0 || b < 0 || c < 0)
                return double.NaN;
            var angle = ((a * a) + (b * b) - (c * c)) / (2 * a * b);
            return Math.Acos(angle);
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