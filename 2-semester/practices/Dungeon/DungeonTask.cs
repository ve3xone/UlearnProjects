using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var fromStartToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);

        var allChestsVisited = map.Chests.Select(c => c.Location).ToHashSet();
        var chestsArray = allChestsVisited.Select(p => new EmptyChest(p)).ToArray();

        var fromExitToChests = BfsTask.FindPaths(map, map.Exit, chestsArray);

        var shortestPaths = CombinePaths(fromStartToChests, fromExitToChests);

        if (shortestPaths != null && shortestPaths.Any())
        {
            var path = GetMaxByChestValue(shortestPaths, map.Chests);
            if (path != null)
                return GetDirections(path).ToArray();
        }

        var fromStartToExit = FindPathFromStartToExit(map);
        if (fromStartToExit != null)
            return GetDirections(fromStartToExit.Reverse()).ToArray();

        return System.Array.Empty<MoveDirection>();
    }

    private static IEnumerable<Point>? FindPathFromStartToExit(Map map)
    {
        return BfsTask.FindPaths(map,
                                 map.InitialPosition,
                                 new Chest[] { new EmptyChest(map.Exit) })
                                .FirstOrDefault();
    }

    private static IEnumerable<IEnumerable<Point>>? CombinePaths(IEnumerable<SinglyLinkedList<Point>> fromStartToChests,
                                                                 IEnumerable<SinglyLinkedList<Point>> fromExitToChests)
    {
        return fromStartToChests
               .Join(fromExitToChests, startPath => startPath.Value, exitPath => exitPath.Value,
                 (startPath, exitPath) => startPath.Reverse().Concat(exitPath.Skip(1)))
               .GroupBy(path => path.Count())
               .MinBy(group => group.Key);
    }

    private static IEnumerable<Point>? GetMaxByChestValue(IEnumerable<IEnumerable<Point>> paths,
                                                           IEnumerable<Chest> chests)
    {
        var chestValues = chests.ToDictionary(chest => chest.Location, chest => chest.Value);
        var maxValue = byte.MinValue;
        IEnumerable<Point>? result = null;
        foreach (var path in paths)
        {
            if (path != null)
            {
                var chest = path.FirstOrDefault(cell => chestValues.ContainsKey(cell));
                if (chest != null)
                {
                    var value = chestValues[chest];
                    if (value >= maxValue)
                    {
                        maxValue = value;
                        result = path;
                    }
                }
            }
        }

        return result;
    }

    private static IEnumerable<MoveDirection> GetDirections(IEnumerable<Point> path)
    {
        Point? prev = null;
        foreach (var next in path)
        {
            if (prev != null)
                yield return Walker.ConvertOffsetToDirection(next - prev);
            prev = next;
        }
    }
}