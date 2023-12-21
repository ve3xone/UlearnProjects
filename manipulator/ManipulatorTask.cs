using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var wristX = x - Manipulator.Palm * Math.Cos(alpha);
            var wristY = y + Manipulator.Palm * Math.Sin(alpha);
            var fromStartToWrist = Math.Sqrt(wristX * wristX + wristY * wristY);
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, fromStartToWrist);
            var firstPart = TriangleTask.GetABAngle(Manipulator.UpperArm, fromStartToWrist, Manipulator.Forearm);
            var secondPart = Math.Atan2(wristY, wristX);
            var shoulder = firstPart + secondPart;
            var wrist = -alpha - shoulder - elbow;

            if (double.IsNaN(wrist) || double.IsNaN(shoulder) || double.IsNaN(elbow))
                return new[] { double.NaN, double.NaN, double.NaN };

            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            Random random = new();

            for (int i = 0; i < 10; i++)
            {
                var x = random.Next();
                var y = random.Next();
                var angle = 2 * Math.PI * random.NextDouble();
                var angles = ManipulatorTask.MoveManipulatorTo(x, y, angle);

                if (!Double.IsNaN(angles[0]))
                {
                    var coordinates = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2]);
                    Assert.AreEqual(x, coordinates[2].X, 1e-5);
                    Assert.AreEqual(y, coordinates[2].Y, 1e-5);
                }
            }
        }
    }
}