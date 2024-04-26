using Greedy.Architecture;
using System.Collections.Generic;
using System.Linq;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var pathToGoal = new List<Point>();
            var chests = state.Chests.ToList();
            if (chests.Count < state.Goal)
                return pathToGoal;

            var pathFinder = new DijkstraPathFinder();
            var stamina = state.InitialEnergy;
            while (state.Scores < state.Goal)
            {
                var pathsToChests = pathFinder.GetPathsByDijkstra(state, state.Position, chests);
                var pathToBestChest = pathsToChests.FirstOrDefault(path => path.Path.Any());
                if (pathToBestChest == null)
                    return pathToGoal;
                stamina -= pathToBestChest.Cost;
                if (stamina < 0)
                    return pathToGoal;
                pathToGoal.AddRange(pathToBestChest.Path.Skip(1));
                state.Position = pathToBestChest.Path.Last();
                chests.Remove(state.Position);
                state.Scores++;
            }

            return pathToGoal;
        }
    }
}