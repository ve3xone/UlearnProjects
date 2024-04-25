using System;
using System.Collections.Generic;
using System.Linq;

namespace Greedy.Architecture;

public class StateController
{
	private readonly bool isGreedyPathFinder;

	public Dictionary<Point, List<Point>> MovementsToFrom { get; } = new();
	public Queue<Point> Path { get; }
	public State State { get; }
	public HashSet<Point> Chests { get; }
	public HashSet<Point> VisitedChests { get; } = new();

	public StateController(State state, IEnumerable<Point> path, bool isGreedyPathFinder = true) : this(state)
	{
		this.isGreedyPathFinder = isGreedyPathFinder;
		Path = new Queue<Point>(path);
	}

	private StateController(State state)
	{
		State = new State(state);
		Chests = new HashSet<Point>(State.Chests);
		CollectChestIfStandingOnIt();
		SetGameOverMessageIfOutOfEnergy();
	}

	public string? GameLostMessage { get; private set; }

	public bool GameIsLost => GameLostMessage != null;
	public bool GameIsWon => State.Scores >= State.Goal && !GameIsLost;

	public void MoveThroughPathFast()
	{
		while (TryMoveOneStep()) ;
	}

	public bool TryMoveOneStep()
	{
		if (GameIsLost)
			return false;
		if (!Path.Any())
		{
			if (!GameIsWon)
				GameLostMessage = isGreedyPathFinder
					? "No more steps. Not able to reach scores goal"
					: "No more steps. Not able to collect maximum possible chests";
			return false;
		}

		var nextPosition = Path.Dequeue();
		if (!State.InsideMap(nextPosition))
		{
			GameLostMessage = $"Can't move to {nextPosition}";
			return false;
		}

		var movementDistance = DistanceBetween(State.Position, nextPosition);
		switch (movementDistance)
		{
			case 0:
				GameLostMessage = $"Player can't move from its position to same position: {State.Position}";
				return false;
			case > 1:
				GameLostMessage =
					$"Player can only step in range of one cell. Can't jump from {State.Position} to {nextPosition}: they are not neighbours!";
				return false;
		}

		MemorizeMovementFromTo(State.Position, nextPosition);
		State.Position = nextPosition;
		CollectChestIfStandingOnIt();
		if (State.IsWallAt(nextPosition))
		{
			GameLostMessage = "Crashed against wall";
			return false;
		}

		State.Energy -= State.CellCost[Convert.ToInt32(State.Position.X), Convert.ToInt32(State.Position.Y)];
		return !SetGameOverMessageIfOutOfEnergy();
	}

	private void CollectChestIfStandingOnIt()
	{
		if (!Chests.Contains(State.Position)) return;
		
		State.Scores++;
		VisitedChests.Add(State.Position);
		Chests.Remove(State.Position);
	}

	private double DistanceBetween(Point statePosition, Point nextPosition)
	{
		return Math.Abs(nextPosition.X - statePosition.X) + Math.Abs(nextPosition.Y - statePosition.Y);
	}

	private bool SetGameOverMessageIfOutOfEnergy()
	{
		if (State.Energy >= 0) return false;
		
		GameLostMessage = "Out of energy";
		return true;

	}

	private void MemorizeMovementFromTo(Point from, Point to)
	{
		if (!MovementsToFrom.ContainsKey(to))
			MovementsToFrom[to] = new List<Point> { from };
		else
			MovementsToFrom[to].Add(from);
	}
}