using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Greedy.Architecture;

public static class StatesLoader
{
	public static State LoadStateFromFolder(DirectoryInfo folder, string stateName)
	{
		var stateFile = folder.GetFiles($"{stateName}.txt", SearchOption.AllDirectories).Single();
		var stateInputData = File.ReadAllText(stateFile.FullName);
		return LoadStateFromInputData(stateInputData, stateName);
	}

	public static IEnumerable<string> LoadAllStateNames(DirectoryInfo folder)
	{
		return folder.GetFiles("*", SearchOption.AllDirectories)
			.Where(info => info.Name.EndsWith(".txt"))
			.Select(file => Path.GetFileNameWithoutExtension(file.Name));
	}

	public static State LoadStateFromInputData(string inputData, string stateName = "")
	{
		return LoadStateFromLines(inputData.Split('|'), stateName);
	}

	private static State LoadStateFromLines(string[] mapGoalEnergyPositionChests, string stateName)
	{
		var map = mapGoalEnergyPositionChests[0].Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		var height = map.Length;
		var width = map.Any() ? map[0].Length : 0;
		var cellCost = new int[width, height];
		try
		{
			for (var y = 0; y < height; y++)
			for (var x = 0; x < width; x++)
			{
				cellCost[x, y] = int.Parse(map[y][x].ToString());
			}
		}
		catch (Exception e)
		{
			throw new ArgumentException("Make sure input map is rectangular and consists only of 0 to 9 digits", e);
		}

		var goalEnergy =
			mapGoalEnergyPositionChests[1].Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[1]
				.Split(' ');

		var scoresGoal = int.Parse(goalEnergy[0]);

		var initialEnergy = int.Parse(goalEnergy[1]);

		var positionXY =
			mapGoalEnergyPositionChests[2].Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[1]
				.Split(' ');
		var position = new Point(int.Parse(positionXY[0]), int.Parse(positionXY[1]));

		var chestsXYs = mapGoalEnergyPositionChests[3].Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		var chests = chestsXYs
			.Select(xy => xy.Split(' ').Select(int.Parse).ToArray())
			.Select(xy => new Point(xy[0], xy[1]))
			.ToHashSet();

		return new State(stateName, initialEnergy, position, cellCost, scoresGoal, chests);
	}
}