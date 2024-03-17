using System;
namespace func_rocket;

public class Physics
{
	private readonly double mass;
	private readonly double maxVelocity;
	private readonly double maxTurnRate;

	public Physics() : this(1.0, 300.0, 0.15)
	{
	}

	public Physics(double mass, double maxVelocity, double maxTurnRate)
	{
		this.mass = mass;
		this.maxVelocity = maxVelocity;
		this.maxTurnRate = maxTurnRate;
	}

	public Rocket MoveRocket(Rocket rocket, RocketForce force, Turn turn, Vector spaceSize, double dt)
	{
		var turnRate = turn switch
		{
			Turn.Left => -maxTurnRate,
			Turn.Right => maxTurnRate,
			_ => 0
		};
		
		var dir = rocket.Direction + turnRate * dt;
		var velocity = rocket.Velocity + force(rocket) * dt / mass;
		if (velocity.Length > maxVelocity) velocity = velocity.Normalize() * maxVelocity;
		var location = rocket.Location + velocity * dt;
		if (location.X < 0) velocity = velocity with { X = Math.Max(0, velocity.X) };
		if (location.X > spaceSize.X) velocity = velocity with { X = Math.Min(0, velocity.X) };
		if (location.Y < 0) velocity = velocity with { Y = Math.Max(0, velocity.Y) };
		if (location.Y > spaceSize.Y) velocity = velocity with { Y = Math.Min(0, velocity.Y) };
		return new Rocket(location.BoundTo(spaceSize), velocity, dir);
	}
}