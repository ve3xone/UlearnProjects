using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = CreateTasks(rocket);
        var completedTasks = Task.WhenAll(tasks).Result;

        if (completedTasks != null && completedTasks.Any())
        {
            var bestResult = completedTasks.MaxBy(taskResult => taskResult.Score);
            return rocket.Move(bestResult.Turn, level);
        }
        else
        {
            throw new InvalidOperationException("No completed tasks were found.");
        }
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn Turn, double Score)>>();
        var iterationsCountPerThread = iterationsCount / threadsCount;

        Parallel.For(0, threadsCount, i =>
        {
            var randomInstance = new Random();

            tasks.Add(Task.Run(() =>
            {
                return SearchBestMove(rocket, randomInstance, iterationsCountPerThread);
            }));
        });

        return tasks;
    }
}