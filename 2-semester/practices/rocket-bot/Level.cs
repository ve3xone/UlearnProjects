namespace rocket_bot;

public class Level
{
	public readonly Vector[] Checkpoints;
	public readonly Rocket InitialRocket;
	public const int MaxTicksCount = 1000;
	public readonly Physics Physics;

	public Level(Rocket rocket, Vector[] checkpoints, Physics physics)
	{
		InitialRocket = rocket;
		Checkpoints = checkpoints;
		Physics = physics;
	}

	public Level Clone() => new(InitialRocket, Checkpoints, Physics);
}