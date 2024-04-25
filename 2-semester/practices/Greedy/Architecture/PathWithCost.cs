using System.Collections.Generic;
using System.Linq;

namespace Greedy.Architecture;

public class PathWithCost
{
	public List<Point> Path { get; }
	public int Cost { get; }

	public PathWithCost(int cost, params Point[] path)
	{
		Cost = cost;
		Path = path.ToList();
	}

	public Point Start => Path.First();
	public Point End => Path.Last();

	public override string ToString()
	{
		var result = $"Cost: {Cost}, Path: {string.Join(" ", Path.Select(p => p.ToString()))}";
		return result;
	}
}