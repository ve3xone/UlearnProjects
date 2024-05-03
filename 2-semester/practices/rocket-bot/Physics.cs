namespace rocket_bot;

public class Physics
{
	private readonly double mass;
	private readonly double maxTurnRate;

	public Physics() : this(1.0, 0.25)
	{
	}

	public Physics(double mass, double maxTurnRate)
	{
		this.mass = mass;
		this.maxTurnRate = maxTurnRate;
	}

	public Rocket MoveRocket(Rocket rocket, double forceValue, Turn turn, double dt)
	{
		var turnRate = turn == Turn.Left ? -maxTurnRate : turn == Turn.Right ? maxTurnRate : 0;
		var dir = rocket.Direction + turnRate * dt;
		var force = new Vector(forceValue, 0).Rotate(rocket.Direction);
		var velocity = rocket.Velocity + force * dt / mass;
		var location = rocket.Location + velocity * dt;
		velocity = velocity * (1 - 0.01 * dt);
		return new Rocket(location, velocity, dir, rocket.Time + 1, rocket.TakenCheckpointsCount);
	}
}