using System;
using System.Diagnostics;
using NUnit.Framework;

namespace rocket_bot;

[TestFixture]
public class BotTests_Multithread : BotTests_Base
{
	private const int PerformanceIterations = 200;
	private const int PerformanceMoves = 35;

	private void ComparePerformances(Bot singleThreadBot, Bot multiThreadBot, Rocket initialRocket, int times = 40)
	{
		var successes = 0;
		var successThreshold = 0.8 * times;
		var stopWatch = new Stopwatch();
		for (var i = 0; i < times; ++i)
		{
			GC.Collect();
			stopWatch.Restart();
			singleThreadBot.GetNextMove(initialRocket);
			stopWatch.Stop();
			var singleThreadBotTime = stopWatch.Elapsed;

			GC.Collect();
			stopWatch.Restart();
			multiThreadBot.GetNextMove(initialRocket);
			stopWatch.Stop();
			var multiThreadBotTime = stopWatch.Elapsed;

			if (multiThreadBotTime <= singleThreadBotTime) ++successes;
		}

		Assert.GreaterOrEqual(successes, successThreshold,
			$"Ваше решение в два потока должно работать быстрее решения в один поток в {successThreshold} случаев из {times}");
	}

	[TestCase(2)]
	[TestCase(4)]
	public void TestCanTakeTarget(int threadCount)
	{
		var level = new Level(rocket, new[] { new Vector(100, 90), new Vector(0, 0) }, LevelsFactory.StandardPhysics);
		RunTestCase(threadCount, level);
	}

	[TestCase(2)]
	[TestCase(4)]
	public void TestCannotTakeTarget(int threadCount)
	{
		var level = new Level(rocket, new[] { new Vector(0, 100), new Vector(0, 0) }, LevelsFactory.StandardPhysics);
		RunTestCase(threadCount, level);
	}

	[TestCase(2)]
	[TestCase(4)]
	public void TestGoToNextTarget(int threadCount)
	{
		var level = new Level(rocket, new[] { new Vector(99, 79), new Vector(120, 79) },
			LevelsFactory.StandardPhysics);
		RunTestCase(threadCount, level);
	}

	[TestCase(2)]
	[TestCase(4)]
	public void TestGoStraight(int threadCount)
	{
		var level = new Level(rocket, new[] { new Vector(100, 90), new Vector(100, 0) },
			LevelsFactory.StandardPhysics);
		RunTestCase(threadCount, level);
	}

	[TestCase(2)]
	[TestCase(4)]
	public void TestGoToNextCheckpoint(int threadCount)
	{
		rocket = rocket.IncreaseCheckpoints();
		var level = new Level(rocket, new[] { new Vector(200, 100), new Vector(0, 100) },
			LevelsFactory.StandardPhysics);
		RunTestCase(threadCount, level);
	}

	private void RunTestCase(int threadCount, Level level)
	{
		channel.AppendIfLastItemIsUnchanged(level.InitialRocket, null);

		ComparePerformances(
			new Bot(level, channel, PerformanceMoves, PerformanceIterations, random, 1),
			new Bot(level, channel, PerformanceMoves, PerformanceIterations, random, threadCount),
			level.InitialRocket);
	}
}