using System;

namespace Mazes;

public static class DiagonalMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        var firstDirection = (width > height) ? Direction.Right : Direction.Down;
        var secondDirection = (firstDirection == Direction.Down) ? Direction.Right : Direction.Down;

        var stepsOver = GetSteps(width, height);

        MoveToExit(robot, firstDirection, secondDirection, stepsOver);
    }

    static int GetSteps(int width, int height)
    {
        width -= 2;
        height -= 2;
        return Math.Max(width, height) / Math.Min(width, height);
    }

    private static void MoveToDirection(Robot robot, Direction dir, int steps)
    {
        for (var i = 0; i < steps; i++)
            robot.MoveTo(dir);
    }

    private static void MoveToExit(Robot robot, Direction firstDirection, Direction secondDirection, int steps)
    {
        while (!robot.Finished)
        {
            MoveToDirection(robot, firstDirection, steps);
            if (robot.Finished)
                break;
            MoveToDirection(robot, secondDirection, 1);
        }
    }
}