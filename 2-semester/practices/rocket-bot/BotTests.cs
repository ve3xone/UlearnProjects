using NUnit.Framework;

namespace rocket_bot;

[TestFixture]
public class BotTests : BotTests_Base
{
	private const int CorrectnessIterations = 50;
	private const int CorrectnessMoves = 4;

	private void CheckAlmostAlwaysEquals(Rocket expectedResult, Bot bot, Rocket initialRocket, int times = 1000)
	{
		var successes = 0;
		var successThreshold = 0.8 * times;
		for (var i = 0; i < times; ++i)
		{
			var actualResult = bot.GetNextMove(initialRocket);
			if (actualResult.Equals(expectedResult)) ++successes;
		}

		Assert.GreaterOrEqual(successes, successThreshold);
	}

	[TestCase(1)]
	[TestCase(2)]
	[TestCase(4)]
	public void TestCanTakeTarget(int threadCount)
	{
		var level = new Level(rocket, new[] {new Vector(100, 90), new Vector(0, 0) }, LevelsFactory.StandardPhysics);
		var expectedResult = level.InitialRocket.Move(Turn.Left, level); // поворачиваем к (0,0), через лево короче.
		RunTestCase(threadCount, level, expectedResult);
	}

	[TestCase(1)]
	[TestCase(2)]
	[TestCase(4)]
	public void TestCannotTakeTarget(int threadCount)
	{
		var level = new Level(rocket, new[] {new Vector(0, 100), new Vector(0, 0)}, LevelsFactory.StandardPhysics);
		var expectedResult = level.InitialRocket.Move(Turn.Left, level); // поворачиваем к (0, 100)
		RunTestCase(threadCount, level, expectedResult);
	}

	[TestCase(1)]
	[TestCase(2)]
	[TestCase(4)]
	public void TestGoToNextTarget(int threadCount)
	{
		var level = new Level(rocket, new[] {new Vector(99, 79), new Vector(120, 79)},
			LevelsFactory.StandardPhysics);
		var expectedResult = level.InitialRocket.Move(Turn.Right, level); // первый возьмем по инерции, поворачиваем к (120, 79)
		RunTestCase(threadCount, level, expectedResult);
	}

	[TestCase(1)]
	[TestCase(2)]
	[TestCase(3)]
	[TestCase(4)]
	public void TestGoStraight(int threadCount)
	{
		var level = new Level(rocket, new[] {new Vector(100, 90), new Vector(100, 0)},
			LevelsFactory.StandardPhysics);
		var expectedResult = level.InitialRocket.Move(Turn.None, level); // следующие два чекпоинта прямо. Летим прямо.
		RunTestCase(threadCount, level, expectedResult);
	}

	[TestCase(1)]
	[TestCase(2)]
	[TestCase(4)]
	public void TestGoToNextCheckpoint(int threadCount)
	{
		rocket = rocket.IncreaseCheckpoints();
		var level = new Level(rocket, new[] {new Vector(200, 100), new Vector(0, 100)},
			LevelsFactory.StandardPhysics);
		var expectedResult = level.InitialRocket.Move(Turn.Left, level); // Нам к (0, 100), потому что первый уже взят.
		RunTestCase(threadCount, level, expectedResult);
	}

	private void RunTestCase(int threadCount, Level level, Rocket expectedResult)
	{
		channel.AppendIfLastItemIsUnchanged(level.InitialRocket, null);
		var bot = new Bot(level, channel, CorrectnessMoves, CorrectnessIterations, random, threadCount);
		CheckAlmostAlwaysEquals(expectedResult, bot, level.InitialRocket);
	}
}