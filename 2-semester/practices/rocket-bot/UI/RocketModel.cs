using System.Collections.Generic;
using System.Linq;

namespace rocket_bot.UI;

public class RocketModel
{
	public Level CurrentLevel { get; private set; }
	public Channel<Rocket> Channel { get; private set; }
	public Rocket Rocket { get; private set; }

	private readonly HashSet<Turn> manualControls = new();

	private int skipTurns = 1;

	public (Level currentLevel, Rocket rocket, Channel<Rocket> channel ) Data => (CurrentLevel, Rocket, Channel);

	public RocketModel(Level level, Channel<Rocket> channel)
	{
		SetLevel(level.Clone(), channel);
	}
	
	private void SetLevel(Level level, Channel<Rocket> channel)
	{
		Channel = channel;
		CurrentLevel = level;
		Rocket = level.InitialRocket;
	}

	public void MoveRocket()
	{
		if (!manualControls.Any())
		{
			RewindTo(Rocket.Time + skipTurns);
			return;
		}

		if (Rocket.IsCompleted(CurrentLevel))
			return;
		
		var control = manualControls.First();
		for (var i = 0; i < skipTurns; ++i)
		{
			Rocket = Rocket.Move(control, CurrentLevel);
			Channel[Rocket.Time] = Rocket;
		}
	}

	public void RewindTo(double time)
	{
		var prevRocket = Channel[(int)time];

		if (prevRocket == null || Equals(Rocket, prevRocket))
			return;

		Rocket = prevRocket;
	}
	
	public void SetPlaySpeed(int speed)
	{
		skipTurns = speed;
	}
	
	public void SetManualControl(Turn control, bool down)
	{
		if (down) manualControls.Add(control);
		else manualControls.Remove(control);
	}
}