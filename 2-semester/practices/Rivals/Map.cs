using System;
using System.Collections.Generic;

namespace Rivals;

public class Map
{
	public readonly MapCell[,] Maze;
	public readonly Point[] Players;
	public readonly Point[] Chests;

	private Map(MapCell[,] maze, Point[] players, Point[] chests)
	{
		Maze = maze;
		Players = players;
		Chests = chests;
	}

	public static Map FromText(string text)
	{
		var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		return FromLines(lines);
	}

	public static Map FromLines(string[] lines)
	{
		var dungeon = new MapCell[lines[0].Length, lines.Length];
		var players = new List<Point>();
		var chests = new List<Point>();
		for (var y = 0; y < lines.Length; y++)
		{
			for (var x = 0; x < lines[y].Length; x++)
			{
				switch (lines[y][x])
				{
					case '#':
						dungeon[x, y] = MapCell.Wall;
						break;
					case 'P':
						dungeon[x, y] = MapCell.Empty;
						players.Add(new Point(x, y));
						break;
					case 'C':
						dungeon[x, y] = MapCell.Empty;
						chests.Add(new Point(x, y));
						break;
					case ' ':
						dungeon[x, y] = MapCell.Empty;
						break;
				}
			}
		}
		return new Map(dungeon, players.ToArray(), chests.ToArray());
	}

	public bool InBounds(Point point)
		=> point is { X: >= 0, Y: >= 0 }
		   && Maze.GetLength(0) > point.X
		   && Maze.GetLength(1) > point.Y;
}