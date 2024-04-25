using System.Collections.Generic;

namespace Greedy.Architecture;

public class State
{
	public string MazeName { get; }
	public int[,] CellCost { get; }
	public HashSet<Point> Chests { get; }
	public int InitialEnergy { get; }

	/// <summary>
	/// Количество сундуков, которые нужно собрать.
	/// Для последней задачи (нежадного алгоритма) всегда равен -1 — там нужно собрать максимально возможное количество сундуков.
	/// </summary>
	public int Goal { get; private set; }

	public readonly int MapWidth;
	public readonly int MapHeight;

	public Point Position;
	public int Energy;
	public int Scores;

	public State(
		string mazeName,
		int initialEnergy,
		Point initialPosition,
		int[,] cellCost,
		int goal,
		IEnumerable<Point> chests)
	{
		MazeName = mazeName;
		InitialEnergy = Energy = initialEnergy;
		Position = initialPosition;
		Goal = goal;
		CellCost = cellCost;
		MapWidth = CellCost.GetLength(0);
		MapHeight = CellCost.GetLength(1);
		Chests = new HashSet<Point>(chests);
	}

	public State(State state) : this(state.MazeName, state.InitialEnergy, state.Position, state.CellCost, state.Goal,
		state.Chests)
	{
		Energy = state.Energy;
		Scores = state.Scores;
	}

	public bool IsWallAt(Point point)
		=> IsWallAt(point.X, point.Y);

	public bool IsWallAt(int x, int y)
		=> CellCost[x, y] == 0;

	public bool IsChestAt(Point point)
		=> Chests.Contains(point);

	public bool InsideMap(Point point)
		=> point.X >= 0 && point.X < MapWidth && point.Y >= 0 && point.Y < MapHeight;


	public void RemoveGoal()
		=> Goal = -1;
}