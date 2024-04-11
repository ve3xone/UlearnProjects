using System;
using System.Collections.Generic;

namespace Dungeon;

public class Map
{
	public readonly MapCell[,] Dungeon;
	public readonly Point InitialPosition;
	public readonly Point Exit;
	public readonly Point[] Chests;

	private Map(MapCell[,] dungeon, Point initialPosition, Point exit, Point[] chests)
	{
		Dungeon = dungeon;
		InitialPosition = initialPosition;
		Exit = exit;
		Chests = chests;
	}

	public static Map FromText(string text)
	{
		var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		return FromLines(lines);
	}

	public static Map FromLines(string[] lines)
	{
		var dungeon = new MapCell[lines[0].Length, lines.Length];
		var initialPosition = new Point();
		var exit = new Point();
		var chests = new List<Point>();
		for (var y = 0; y < lines.Length; y++)
		{
			for (var x = 0; x < lines[0].Length; x++)
			{
				switch (lines[y][x])
				{
					case '#':
						dungeon[x, y] = MapCell.Wall;
						break;
					case 'P':
						dungeon[x, y] = MapCell.Empty;
						initialPosition = new Point(x, y);
						break;
					case 'C':
						dungeon[x, y] = MapCell.Empty;
						chests.Add(new Point(x, y));
						break;
					case 'E':
						dungeon[x, y] = MapCell.Empty;
						exit = new Point(x, y);
						break;
					default:
						dungeon[x, y] = MapCell.Empty;
						break;
				}
			}
		}

		return new Map(dungeon, initialPosition, exit, chests.ToArray());
	}

	public bool InBounds(Point point)
		=> point is { X: >= 0, Y: >= 0 }
		   && Dungeon.GetLength(0) > point.X
		   && Dungeon.GetLength(1) > point.Y;
}