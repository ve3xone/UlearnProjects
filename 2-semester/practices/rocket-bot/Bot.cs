using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace rocket_bot;

public partial class Bot
{
	private readonly Channel<Rocket> channel;
	protected readonly int iterationsCount;
	protected readonly Level level;
	private readonly int movesCount;
	protected readonly Random random;
	protected readonly int threadsCount;

	public Bot(Level level, Channel<Rocket> channel, int movesCount, int iterationsCount, Random random,
		int threadsCount)
	{
		this.level = level;
		this.channel = channel;
		this.movesCount = movesCount;
		this.iterationsCount = iterationsCount;
		this.random = random;
		this.threadsCount = threadsCount;
	}

	public void RunInfiniteLoop()
	{
		while (true)
		{
			var rocket = channel.LastItem();
			if (rocket == null) return;
			if (rocket.IsCompleted(level))
			{
				// Чтобы не тратить 100% одного ядра на ничего не делающий цикл.
				Thread.Sleep(100);
			}
			else
			{
				var newRocket = GetNextMove(rocket);
				channel.AppendIfLastItemIsUnchanged(newRocket, rocket);
			}
		}
		// ReSharper disable once FunctionNeverReturns
	}

	public (Turn Turn, double Score) SearchBestMove(Rocket rocket, Random searchRandom, int searchIterationsCount)
	{
		var initialRocketTime = rocket.Time;
		List<Turn> bestStrategy = null;
		double bestScore = 0;
		var i = 0;
		do
		{
			var moves = GetRandomTwoPhaseStrategy(movesCount, searchRandom).ToList();
			//var moves = GetRandomStrategy(movesCount, random).ToList();
			var score = EstimateMoves(rocket, moves);
			if (bestStrategy == null || score > bestScore)
			{
				//Trace.WriteLine($"Iteration #{i}. Found better strategy. Score: {score} (+{score - bestScore})");
				bestScore = score;
				bestStrategy = moves;
			}

			i++;
		} while (!ChannelStateIsChanged(initialRocketTime) && i < searchIterationsCount);
		//Trace.WriteLine($"{i} iterations used");

		return (bestStrategy.First(), bestScore);
	}

	private bool ChannelStateIsChanged(int initialRocketTime)
	{
		return channel.LastItem().Time != initialRocketTime;
	}

	private double EstimateMoves(Rocket rocket, IEnumerable<Turn> moves)
	{
		foreach (var turn in moves)
			rocket = rocket.Move(turn, level);
		return rocket.TakenCheckpointsCount + 1 / (GetDistanceToNextCheckpoint(rocket) + 1);
	}

	private double GetDistanceToNextCheckpoint(Rocket rocket)
	{
		return (rocket.Location - rocket.GetNextRocketCheckpoint(level)).Length;
	}

	private IEnumerable<Turn> GetRandomTwoPhaseStrategy(int steps, Random rnd)
	{
		// move1Duration ходов отдаём одну команду, потом ещё move2Duration ходов отдаём другую.
		var move1Duration = rnd.Next(steps);
		var move2Duration = steps - move1Duration;
		var move1 = (Turn) (rnd.Next(3) - 1);
		var move2 = (Turn) (rnd.Next(3) - 1);
		return Enumerable.Repeat(move1, move1Duration).Concat(
			Enumerable.Repeat(move2, move2Duration));
	}

	private IEnumerable<Turn> GetRandomStrategy(int steps, Random rnd)
	{
		return Enumerable.Range(0, steps).Select(i => (Turn) (rnd.Next(3) - 1));
	}
}