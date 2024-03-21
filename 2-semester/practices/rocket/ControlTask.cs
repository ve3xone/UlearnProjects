namespace func_rocket;

public class ControlTask
{
    public static Turn ControlRocket(Rocket rocket, Vector target)
    {
        var targetDirection = target - rocket.Location;
        var nextRocketPosition = rocket.Location + rocket.Velocity + ForcesTask.GetThrustForce(10)(rocket);
        var nextRocketDirection = nextRocketPosition - rocket.Location;

        var angleDiff = (targetDirection.Angle - nextRocketDirection.Angle);

        return angleDiff > 0 ? Turn.Right : Turn.Left;
    }
}