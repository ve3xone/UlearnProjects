using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Digger;

public class GameState
{
	public const int SizeOfElement = 1 << 5;
	public List<CreatureAnimation> Animations = new();

	public void BeginAct()
	{
		Animations.Clear();
		for (var i = 0; i < Game.MapWidth; i++)
		for (var j = 0; j < Game.MapHeight; j++)
		{
			var creature = Game.Map[i, j];
			if (creature == null)
				continue;
			var cmd = creature.Act(i, j);

			if (i + cmd.DeltaX < 0 || i + cmd.DeltaX >= Game.MapWidth || j + cmd.DeltaY < 0 ||
			    j + cmd.DeltaY >= Game.MapHeight)
				throw new Exception();

			Animations.Add(
				new CreatureAnimation
				{
					Command = cmd,
					Creature = creature,
					Location = new Point(i * SizeOfElement, j * SizeOfElement),
					TargetLogicalLocation = new Point(i + cmd.DeltaX, j + cmd.DeltaY)
				});
		}

		Animations = Animations
			.OrderByDescending(z => z.Creature.GetDrawingPriority())
			.ToList();
	}

	public void EndAct()
	{
		var creaturesPerLocation = GetCandidates();
		for (var i = 0; i < Game.MapWidth; i++)
		{
			for (var j = 0; j < Game.MapHeight; j++)
			{
				Game.Map[i, j] = SelectWinnerCandidate(creaturesPerLocation, i, j);
			}
		}
	}

	private static ICreature SelectWinnerCandidate(List<ICreature>[,] creatures, int i, int j)
	{
		var candidates = creatures[i, j];
		var alive = candidates.ToList();
		foreach (var candidate in candidates)
		{
			foreach (var rival in candidates)
			{
				if (rival != candidate && candidate.DeadInConflict(rival))
				{
					alive.Remove(candidate);
				}
			}
		}

		if (alive.Count > 1)
			throw new Exception();

		return alive.FirstOrDefault();
	}

	private List<ICreature>[,] GetCandidates()
	{
		var creatures = new List<ICreature>[Game.MapWidth, Game.MapHeight];
		for (var i = 0; i < Game.MapWidth; i++)
		{
			for (var j = 0; j < Game.MapHeight; j++)
			{
				creatures[i, j] = new List<ICreature>();
			}
		}

		foreach (var e in Animations)
		{
			var i = e.TargetLogicalLocation.X;
			var j = e.TargetLogicalLocation.Y;
			var next = e.Command.TransformTo ?? e.Creature;
			creatures[i, j].Add(next);
		}

		return creatures;
	}
}