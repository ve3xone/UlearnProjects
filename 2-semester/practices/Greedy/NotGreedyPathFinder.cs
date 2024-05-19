using Greedy.Architecture;
using System.Collections.Generic;
using System.Linq;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    private List<Point> bestPath = new();
    private int bestChestsCount;
    private int chestCount;
    private int maxEnergy;

    public List<Point> FindPathToCompleteGoal(State state)
    {
        chestCount = state.Chests.Count;
        maxEnergy = state.Energy;
        var allPaths = ComputeAllPaths(state);
        SearchOptimalPath(state, state.Position, 0, 0,
                          state.Chests.ToList(),
                          new List<Point>(),
                          new HashSet<Point>(),
                          new Dictionary<(Point, int, int), (int, List<Point>)>(),
                          allPaths);
        return bestPath;
    }

    private Dictionary<(Point, Point), PathWithCost> ComputeAllPaths(State state)
    {
        var finder = new DijkstraPathFinder();
        var allPaths = new Dictionary<(Point, Point), PathWithCost>();

        var points = state.Chests.Append(state.Position).ToList();
        foreach (var start in points)
            foreach (var end in points)
                if (start != end)
                {
                    var path = finder.GetPathsByDijkstra(state,
                                                         start,
                                                         new List<Point> { end }).FirstOrDefault();
                    if (path != null)
                        allPaths[(start, end)] = path;
                }

        return allPaths;
    }

    private void SearchOptimalPath(State state,
                                   Point start,
                                   int energyUsed,
                                   int foundChestsCount,
                                   List<Point> availableChests,
                                   List<Point> pathTravelled,
                                   HashSet<Point> visited,
                                   Dictionary<(Point, int, int), (int, List<Point>)> memo,
                                   Dictionary<(Point, Point), PathWithCost> allPaths)
    {
        UpdateBestPathIfNeeded(foundChestsCount, pathTravelled);

        if (ShouldTerminateSearch(foundChestsCount, energyUsed))
            return;

        var key = (start, energyUsed, foundChestsCount);
        if (IsMemoizedStateWorse(memo, key, energyUsed))
            return;
        else
            memo[key] = (energyUsed, pathTravelled);

        var pathsToChests = GetPathsToChests(start,
                                             energyUsed,
                                             availableChests,
                                             visited, allPaths);

        foreach (var (chest, path) in pathsToChests.OrderBy(p => p.Value.Cost))
            ExplorePath(state, energyUsed, foundChestsCount, availableChests,
                        pathTravelled, visited, memo, allPaths, chest, path);
    }

    private void UpdateBestPathIfNeeded(int foundChestsCount, List<Point> pathTravelled)
    {
        if (foundChestsCount > bestChestsCount)
        {
            bestChestsCount = foundChestsCount;
            bestPath = pathTravelled.ToList();
        }
    }

    private bool ShouldTerminateSearch(int foundChestsCount, int energyUsed)
    {
        return foundChestsCount == chestCount ||
               bestChestsCount == chestCount ||
               energyUsed >= maxEnergy;
    }

    private bool IsMemoizedStateWorse(Dictionary<(Point, int, int), (int, List<Point>)> memo,
                                      (Point, int, int) key, int energyUsed)
    {
        return memo.TryGetValue(key, out var memoValue) &&
               memoValue.Item1 <= energyUsed;
    }

    private Dictionary<Point, PathWithCost> GetPathsToChests(Point start,
                                                             int energyUsed,
                                                             List<Point> availableChests,
                                                             HashSet<Point> visited,
                                                             Dictionary<(Point, Point), PathWithCost> allPaths)
    {
        var pathsToChests = new Dictionary<Point, PathWithCost>();

        foreach (var chest in availableChests)
            if (!visited.Contains(chest) &&
                allPaths.TryGetValue((start, chest), out var path) &&
                path.Cost + energyUsed <= maxEnergy)
                pathsToChests[chest] = path;

        return pathsToChests;
    }

    private void ExplorePath(State state,
                             int energyUsed,
                             int foundChestsCount,
                             List<Point> availableChests,
                             List<Point> pathTravelled,
                             HashSet<Point> visited,
                             Dictionary<(Point, int, int), (int, List<Point>)> memo,
                             Dictionary<(Point, Point), PathWithCost> allPaths,
                             Point chest,
                             PathWithCost path)
    {
        availableChests.Remove(chest);
        visited.Add(chest);

        SearchOptimalPath(state,
                          path.End,
                          path.Cost + energyUsed,
                          foundChestsCount + 1,
                          availableChests,
                          pathTravelled.Concat(path.Path.Skip(1)).ToList(),
                          visited,
                          memo,
                          allPaths);

        availableChests.Add(chest);
        visited.Remove(chest);
    }
}