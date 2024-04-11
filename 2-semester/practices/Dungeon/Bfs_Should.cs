using System.Collections.Generic;
using System.Linq;
using Dungeon.Dungeons;
using NUnit.Framework;

namespace Dungeon;

[TestFixture]
public class Bfs_Should
{
	[Test, Order(0)]
	public void ReturnNoPaths_WhenNoPathsToChests()
	{
		var textMap = new[]
		{
			"P ",
			"##",
			"C "
		};
		var map = Map.FromLines(textMap);

		var paths = GetPaths(map);

		Assert.IsEmpty(paths);
	}

	[Test, Order(1)]
	public void ReturnCorrectPaths_OnEmptyDungeon()
	{
		var textMap = new[]
		{
			"P ",
			"C "
		};
		var map = Map.FromLines(textMap);
		var expectedLengths = new[] { 2 };

		var paths = GetPaths(map);

		AssertPaths(paths, map, expectedLengths);
	}

	[Test, Order(2)]
	public void ReturnCorrectPaths_OnSimpleDungeon()
	{
		var textMap = new[]
		{
			"P #",
			"# #",
			"C  "
		};
		var map = Map.FromLines(textMap);
		var expectedLengths = new[] { 5 };

		var paths = GetPaths(map);

		AssertPaths(paths, map, expectedLengths);
	}

	[Test, Order(3)]
	public void Return_ShortestPaths1()
	{
		var textMap = new[]
		{
			"   ",
			" P ",
			" C "
		};
		var map = Map.FromLines(textMap);
		var expectedLengths = new[] { 2 };

		var paths = GetPaths(map);

		AssertPaths(paths, map, expectedLengths);
	}

	[Test, Order(4)]
	public void Return_ShortestPaths2()
	{
		var textMap = new[]
		{
			" C ",
			"CPC",
			" C "
		};
		var map = Map.FromLines(textMap);
		var expectedLengths = new[] { 2, 2, 2, 2 };

		var paths = GetPaths(map);

		AssertPaths(paths, map, expectedLengths);
	}

	[Test, Order(5)]
	public void Return_ShortestPaths3()
	{
		var textMap = new[]
		{
			"CC",
			"CP",
		};
		var map = Map.FromLines(textMap);
		var expectedLengths = new[] { 3, 2, 2 };

		var paths = GetPaths(map)
			.OrderByDescending(x => x.Count)
			.ToArray();

		AssertPaths(paths, map, expectedLengths);
	}

	[Test, Order(6)]
	public void Return_ShortestPaths_OnBigTestDangeon()
	{
		var map = Map.FromText(DungeonsLoader.Load(DungeonsName.BigDungeon));
		var expectedLengths = new[] { 170, 156, 144, 137, 84 };

		var paths = GetPaths(map)
			.OrderByDescending(x => x.Count)
			.ToArray();

		AssertPaths(paths, map, expectedLengths);
	}

	[Test, Order(7)]
	public void WorksCorrect_ShortestPaths_OnManyCalls()
	{
		var miniMap = Map.FromLines(new[]

		{
			" C ",
			"CPC",
			" C "
		});
		var miniPaths = GetPaths(miniMap);

		AssertPaths(miniPaths, miniMap, new[] { 2, 2, 2, 2 });

		var map = Map.FromText(DungeonsLoader.Load(DungeonsName.BigDungeon));
		var paths = GetPaths(map)
			.OrderByDescending(x => x.Count)
			.ToArray();

		AssertPaths(paths, map, new[] { 170, 156, 144, 137, 84 });
	}

	private static List<Point>[] GetPaths(Map map)
	{
		var paths = BfsTask.FindPaths(map, map.InitialPosition, map.Chests)
			.Select(x => x.ToList())
			.ToArray();
		return paths;
	}

	private void AssertPaths(List<Point>[] paths, Map map, int[] expectedLengths)
	{
		var uniqueEndpoints = paths
			.Select(x => x[0])
			.Distinct()
			.ToArray();

		var expectedPathCount = expectedLengths.Length;

		Assert.AreEqual(expectedPathCount, paths.Length, $"The number of returned paths should be {expectedPathCount}");
		for (var i = 0; i < paths.Length; i++)
		{
			AssertPath(map, paths[i], expectedLengths[i]);
		}
		Assert.AreEqual(expectedPathCount, uniqueEndpoints.Length, "Each path must lead to unique chest");
	}

	private void AssertPath(Map map, List<Point> path, int expectedLength)
	{
		var directions = Walker.PossibleDirections.ToList();
		Assert.IsNotEmpty(path, "path should not be empty");
		Assert.Contains(path[0], map.Chests, $"The first point in the path should be one of the chest, but was {path[0]}");
		for (var i = 0; i < path.Count - 1; i++)
		{
			var offset = path[i + 1] - path[i];
			Assert.IsTrue(directions.Contains(offset), $"Incorrect step #{i} in your path: from {path[i]} to {path[i + 1]}");
			Assert.AreNotEqual(MapCell.Wall, map.Dungeon[path[i + 1].X, path[i + 1].Y], $"Collided with wall at {i}th path point: {path[i + 1]}");
		}
		Assert.AreEqual(map.InitialPosition, path.Last(), "The last point in path must be 'start'");
		Assert.GreaterOrEqual(path.Count, expectedLength, "Checker bug?! Leave a comment above this slide, to notify task authors, please");
		Assert.AreEqual(expectedLength, path.Count, "Your path is not the shortest one");
	}
}