namespace Mazes;

public static class EmptyMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        Move(robot, width - 3, Direction.Right);
        Move(robot, height - 3, Direction.Down);
    }

    static void Move(Robot robot, int stepsCount, Direction direction)
    {
        for (int i = 0; i < stepsCount; i++)
            robot.MoveTo(direction);
    }
}