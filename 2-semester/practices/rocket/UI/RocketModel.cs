namespace func_rocket.UI;

public class RocketModel
{
	public Level? CurrentLevel { get; private set; }
	public bool Right { get; set; }
	public bool Left { get; set; }

	public readonly Vector SpaceSize = new(800, 600);

	public void MoveRocket()
	{
		if (CurrentLevel == null) return;

		var control = Left ? Turn.Left : (Right ? Turn.Right : Turn.None);
		if (control == Turn.None)
			control = ControlTask.ControlRocket(CurrentLevel.Rocket, CurrentLevel.Target);
		CurrentLevel.Move(SpaceSize, control);
	}

	public void SetLevel(Level level)
	{
		CurrentLevel = level;
		CurrentLevel.Reset();
	}
}