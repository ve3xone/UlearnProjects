using NUnit.Framework;

namespace Greedy.Tests;

[TestFixture]
public class NotGreedyPathFinder_Should
{
	[TestCase("not_greedy_no_walls_1")]
	[TestCase("not_greedy_no_walls_2")]
	[TestCase("not_greedy_no_walls_3")]
	[TestCase("not_greedy_no_walls_4")]
	[TestCase("not_greedy_maze_1")]
	public void WinGame_With_State(string stateName)
	{
		var controller =
			TestsHelper.LoadStateFromInputData_And_MoveThroughPath(
				TestsHelper.ReadFile("not_greedy_states." + stateName),
				new NotGreedyPathFinder());

		Assert.True(controller.GameIsWon, controller.GameLostMessage);
	}
}