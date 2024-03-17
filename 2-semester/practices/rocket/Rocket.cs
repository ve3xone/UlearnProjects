namespace func_rocket;

public class Rocket
{
	public Rocket(Vector location, Vector velocity, double direction)
	{
		Location = location;
		Velocity = velocity;
		Direction = direction;
	}

	public readonly Vector Location;
	public readonly Vector Velocity;
	public readonly double Direction;
}