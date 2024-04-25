using System.Collections.Generic;

namespace Greedy.Architecture;

public interface IPathFinder
{
	List<Point> FindPathToCompleteGoal(State state);
}