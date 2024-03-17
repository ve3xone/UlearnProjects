namespace func_rocket;

public class Level
{
	public Level(string name, Rocket rocket, Vector target, Gravity gravity, Physics physics)
	{
		Name = name;
		Rocket = rocket;
		InitialRocket = rocket;
		Target = target;
		this.physics = physics;
		Gravity = gravity;
	}

	public readonly string Name;
	public readonly Rocket InitialRocket;
	private readonly Physics physics;
	public Rocket Rocket;
	public Vector Target;

	public Gravity Gravity { get; }

	public bool IsCompleted => (Rocket.Location - Target).Length < 20;

	public void Move(Vector spaceSize, Turn turn)
	{
		var force = ForcesTask.Sum(ForcesTask.GetThrustForce(1.0), ForcesTask.ConvertGravityToForce(Gravity, spaceSize));
		Rocket = physics.MoveRocket(Rocket, force, turn, spaceSize, 0.3);
	}

	public void Reset()
	{
		Rocket = InitialRocket;
	}
}