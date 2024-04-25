using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Greedy.Architecture;

namespace Greedy.Tests;

public static class TestsHelper
{
	public static StateController LoadStateFromInputData_And_MoveThroughPath(string stateInputData,
		IPathFinder pathFinder, string stateName = "")
	{
		var isGreedyPathFinder = pathFinder is GreedyPathFinder;
		var path = FindPath(pathFinder, StatesLoader.LoadStateFromInputData(stateInputData, stateName), isGreedyPathFinder);
		return MoveThroughPath(isGreedyPathFinder, StatesLoader.LoadStateFromInputData(stateInputData, stateName), path);
	}

	public static StateController MoveThroughPath(bool isGreedyPathFinder, State stateForController, List<Point> path)
	{
		var controller = new StateController(stateForController, path, isGreedyPathFinder);
		controller.MoveThroughPathFast();
		return controller;
	}

	private static List<Point> FindPath(IPathFinder pathFinder, State stateForStudent, bool isGreedyPathFinder)
	{
		if (!isGreedyPathFinder)
		{
			stateForStudent.RemoveGoal();
		}
		var path = pathFinder.FindPathToCompleteGoal(stateForStudent);
		return path;
	}
	
	public static string ReadFile(string stateName)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var name = Assembly.GetCallingAssembly().GetName().Name;
		var resourceName = name + ".states_for_students.";
		using var stream = assembly.GetManifestResourceStream(resourceName + stateName + ".txt");
		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}
}