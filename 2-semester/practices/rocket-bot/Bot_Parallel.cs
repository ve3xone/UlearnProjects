using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
	public Rocket GetNextMove(Rocket rocket)
	{
		// TODO: распараллелить запуск SearchBestMove
		var (turn, score) = CreateTasks(rocket)
			.First()
			.GetAwaiter()
			.GetResult();
		
		return rocket.Move(turn, level);
	}
	
	public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
	{
		return new() { Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount)) };
	}
}