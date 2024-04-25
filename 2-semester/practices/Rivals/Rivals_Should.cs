using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Rivals;

[TestFixture]
public class Rivals_Should : AbstractRivalsTest
{
	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_WithOnePlayer()
	{
		var textMap = new[]
		{
			"P ",
			"  "
		};
		var expectedOwnersMap = new[]
		{
			"00",
			"00"
		};
		var expectedDistancesMap = new[]
		{
			"01",
			"12"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}

	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_WithTwoPlayers()
	{
		var textMap = new[]
		{
			"P  ",
			"   ",
			"  P"
		};
		var expectedOwnersMap = new[]
		{
			"000",
			"001",
			"011"
		};
		var expectedDistancesMap = new[]
		{
			"012",
			"121",
			"210"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}

	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_WithMultiplePlayers()
	{
		var textMap = new[]
		{
			"P P",
			"   ",
			"P P"
		};
		var expectedOwnersMap = new[]
		{
			"001",
			"001",
			"223"
		};
		var expectedDistancesMap = new[]
		{
			"010",
			"121",
			"010"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	
	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_With_OneChest_OnePlayer()
	{
		var textMap = new[]
		{
			"P ",
			" C"
		};
		var expectedOwnersMap = new[]
		{
			"00",
			"00"
		};
		var expectedDistancesMap = new[]
		{
			"01",
			"12"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	
	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_With_OneChest_TwoPlayers()
	{
		var textMap = new[]
		{
			"P  ",
			"  C",
			"  P"
		};
		var expectedOwnersMap = new[]
		{
			"000",
			"001",
			"011"
		};
		var expectedDistancesMap = new[]
		{
			"012",
			"121",
			"210"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	
	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_With_OneChest_MultiplePlayers()
	{
		var textMap = new[]
		{
			"PCP",
			"   ",
			"P P"
		};
		var expectedOwnersMap = new[]
		{
			"001",
			"001",
			"223"
		};
		var expectedDistancesMap = new[]
		{
			"010",
			"121",
			"010"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	
	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_With_TwoChests_OnePlayer()
	{
		var textMap = new[]
		{
			"PC",
			"C "
		};
		var expectedOwnersMap = new[]
		{
			"00",
			"0 "
		};
		var expectedDistancesMap = new[]
		{
			"01",
			"1 "
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	
	[Test]
	public void ReturnCorrectResult_OnEmptyDungeon_With_MultipleChests_OnePlayer()
	{
		var textMap = new[]
		{
			"PC ",
			"  C",
			"C  "
		};
		var expectedOwnersMap = new[]
		{
			"00 ",
			"000",
			"000"
		};
		var expectedDistancesMap = new[]
		{
			"01 ",
			"123",
			"234"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	
	

	[Test]
	public void ReturnCorrectResult_OnSimpleDungeon()
	{
		var textMap = new[]
		{
			"P #",
			"# #",
			"  P"
		};
		var expectedOwnersMap = new[]
		{
			"00#",
			"#0#",
			"111"
		};
		var expectedDistancesMap = new[]
		{
			"01#",
			"#2#",
			"210"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}

	[Test]
	public void ReturnCorrectResult_WhenNoPaths()
	{
		var textMap = new[]
		{
			"P#",
			"##",
			"#P"
		};
		var expectedOwnersMap = new[]
		{
			"0#",
			"##",
			"#1"
		};
		var expectedDistancesMap = new[]
		{
			"0#",
			"##",
			"#0"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}
	[Test]
	public void ReturnCorrectResult_WhenSomeCellsAreUnreachable()
	{
		var textMap = new[]
		{
			"P ",
			"##",
			"  "
		};
		var expectedOwnersMap = new[]
		{
			"00",
			"##",
			"  "
		};
		var expectedDistancesMap = new[]
		{
			"01",
			"##",
			"  "
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}

	[Test]
	public void ReturnResult_WithCorrectOrder()
	{
		var textMap = new[]
		{
			"   ",
			" P ",
			"  P"
		};
		var expectedOwnersMap = new[]
		{
			"000",
			"000",
			"001"
		};
		var expectedDistancesMap = new[]
		{
			"212",
			"101",
			"210"
		};
		var expected = ParseMaps(expectedOwnersMap, expectedDistancesMap);

		var actual = RivalsTask.AssignOwners(Map.FromLines(textMap));

		AssertResult(expected, actual);
	}

	private static List<OwnedLocation> ParseMaps(string[] ownersMap, string[] distancesMap)
	{
		if (ownersMap.Length != distancesMap.Length)
			throw new ArgumentException("Checker error. Maps should have the same sizes");
		var points = new List<OwnedLocation>();
		for (var y = 0; y < ownersMap.Length; y++)
		{
			for (var x = 0; x < ownersMap[0].Length; x++)
			{
				if (ownersMap[y][x] == '#' || ownersMap[y][x] == ' ')
					continue;
				var owner = int.Parse(ownersMap[y][x].ToString());
				var distance = int.Parse(distancesMap[y][x].ToString());
				points.Add(new OwnedLocation(owner, new Point(x, y), distance));
			}
		}
		return points;
	}
}