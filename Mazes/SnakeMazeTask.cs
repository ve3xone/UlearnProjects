namespace Mazes;

public static class SnakeMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        var needStepsToHorizont = width - 3;

        while (!robot.Finished)
        {
            Move(robot, needStepsToHorizont, Direction.Right);
            Move(robot, 2, Direction.Down);
            Move(robot, needStepsToHorizont, Direction.Left);
            if (!robot.Finished)
                Move(robot, 2, Direction.Down);
        }
    }

    private static void Move(Robot robot, int steps, Direction direction)
    {
        for (var i = 0; i < steps; i++)
            robot.MoveTo(direction);
    }
}