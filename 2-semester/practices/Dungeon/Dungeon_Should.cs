using System.Collections.Generic;
using Avalonia;
using Dungeon.Dungeons;
using NUnit.Framework;

namespace Dungeon;

[TestFixture]
public class Dungeon_Should
{
	[Test]
	public void ReturnPathToExit_IfChestIsUnreachable()
	{
		var textMap = new[]
		{
			"PE#",
			"###",
			"C  "
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new[] { MoveDirection.Right }, path);
	}

	[Test]
	public void ReturnPathToExit_IfNoChests()
	{
		var textMap = new[]
		{
			"PE",
			"  "
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new[] { MoveDirection.Right }, path);
	}

	[Test]
	public void ReturnEmptyPath_WhenNoPathsToChestAndExit()
	{
		var textMap = new[]
		{
			"P ",
			"##",
			"CE"
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new MoveDirection[0], path);
	}

	[Test]
	public void ReturnEmptyPath_WhenHasPathToChestButNoPathToExit()
	{
		var textMap = new[]
		{
			"PC",
			"##",
			"CE"
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new MoveDirection[0], path);
	}

	[Test]
	public void ReturnEmptyPath_WhenNoPathToExit()
	{
		var textMap = new[]
		{
			"P#",
			"#E"
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new MoveDirection[0], path);
	}

	[Test]
	public void ReturnCorrectPath_OnEmptyDungeon()
	{
		var textMap = new[]
		{
			"P ",
			"CE"
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new[] {MoveDirection.Down, MoveDirection.Right}, path);
	}

	[Test]
	public void ReturnCorrectPath_OnSimpleDungeon()
	{
		var textMap = new[]
		{
			"P #",
			"#C#",
			"E  "
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new[] {MoveDirection.Right, MoveDirection.Down, MoveDirection.Down, MoveDirection.Left}, path);
	}

	[Test]
	public void Return_ShortestPath1()
	{
		var textMap = new[]
		{
			"   ",
			" P ",
			" CE"
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new[] {MoveDirection.Down, MoveDirection.Right}, path);
	}

	[Test]
	public void Return_ShortestPath2()
	{
		var textMap = new[]
		{
			"ECC",
			" P ",
			"CCC"
		};
		var map = Map.FromLines(textMap);

		var path = DungeonTask.FindShortestPath(map);

		Assert.AreEqual(new[] {MoveDirection.Up, MoveDirection.Left}, path);
	}

	[Test]
	public void ReturnShortestPath3()
	{
		var map = Map.FromText(DungeonsLoader.Load(DungeonsName.BigDungeon));
		var expectedLength = 211;

		var path = DungeonTask.FindShortestPath(map);

		IsValidPath(map, path, expectedLength);
	}

	private void IsValidPath(Map map, MoveDirection[] path, int expectedPathLength)
	{
		var chestTaken = false;
		var chestSet = new HashSet<Point>(map.Chests);
		var walker = new Walker(map.InitialPosition);
		foreach (var step in path)
		{
			walker = walker.WalkInDirection(map, step);
			if (!walker.PointOfCollision.IsNull)
				Assert.Fail($"Collided with wall at {walker.PointOfCollision}");
			if (chestSet.Contains(walker.Position))
				chestTaken = true;
		}
		Assert.True(chestTaken, "Player did not take any chest.");
		Assert.AreEqual(map.Exit, walker.Position, "Player did not reach the exit.");
		Assert.GreaterOrEqual(path.Length, expectedPathLength, "Hmm.... Seems to be an error in the checker. Please notify us in the comments below.");
		Assert.AreEqual(expectedPathLength, path.Length, "Path must be shortest.");
	}
}