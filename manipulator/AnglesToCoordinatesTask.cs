using System;
using Avalonia;
using NUnit.Framework;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
    public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
    {
        var angle = shoulder;
        var x = UpperArm * Math.Cos(shoulder);
        var y = UpperArm * Math.Sin(shoulder);
        var elbowPos = new Point((float)x, (float)y);

        angle += elbow - Math.PI;
        x += Forearm * Math.Cos(angle);
        y += Forearm * Math.Sin(angle);
        var wristPos = new Point((float)x, (float)y);

        angle += wrist - Math.PI;
        x += Palm * Math.Cos(angle);
        y += Palm * Math.Sin(angle);
        var palmEndPos = new Point((float)x, (float)y);

        return new[]
        {
           elbowPos,
           wristPos,
           palmEndPos
          };
    }
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Forearm + Palm, UpperArm)]
    [TestCase(Math.PI / 2, Math.PI, 3 * Math.PI, 0, Forearm + UpperArm + Palm)]
    [TestCase(Math.PI / 2, 3 * Math.PI / 2, 3 * Math.PI / 2, -Forearm, UpperArm - Palm)]
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2, Forearm, UpperArm - Palm)]
    public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
        Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
        Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");

        var distanceShoulderToElbow = Distance(joints[0], new Point(0, 0));
        var distanceElbowToWrist = Distance(joints[1], joints[0]);
        var distanceWristToPalmEnd = Distance(joints[2], joints[1]);

        Assert.AreEqual(UpperArm, distanceShoulderToElbow, 1e-5, "upperArm length");
        Assert.AreEqual(Forearm, distanceElbowToWrist, 1e-5, "forearm length");
        Assert.AreEqual(Palm, distanceWristToPalmEnd, 1e-5, "palm length");
    }

    public static double Distance(Point first, Point second)
    {
        var dx = second.X - first.X;
        var dy = second.Y - first.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}