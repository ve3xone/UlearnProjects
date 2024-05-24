using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    private static readonly Random globalRandom = new();
    private static readonly object randomLock = new();

    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = CreateTasks(rocket);
        var completedTasks = Task.WhenAll(tasks).Result;

        if (completedTasks != null && completedTasks.Any())
        {
            var bestResult = completedTasks.OrderByDescending(taskResult => taskResult.Score).First();
            return rocket.Move(bestResult.Turn, level);
        }
        else
        {
            throw new InvalidOperationException("No completed tasks found.");
        }
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn Turn, double Score)>>();
        var iterationsCountPerThread = iterationsCount / threadsCount;

        Parallel.For(0, threadsCount, i =>
        {
            Random randomInstance;
            lock (randomLock)
            {
                randomInstance = new Random(globalRandom.Next());
            }

            tasks.Add(Task.Run(() =>
            {
                return SearchBestMove(rocket, randomInstance, iterationsCountPerThread);
            }));
        });

        return tasks;
    }
}