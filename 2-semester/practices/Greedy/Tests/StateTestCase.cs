using System.Collections.Generic;
using Greedy.Architecture;

namespace Greedy.Tests;

public class StateTestCase
{
	public string Name { get; set; }
	public string InputData { get; set; }
	public List<Point> ExpectedPath { get; set; }

	public override string ToString()
	{
		return Name;
	}
}