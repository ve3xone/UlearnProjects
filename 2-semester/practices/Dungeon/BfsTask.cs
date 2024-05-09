using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public static class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        var visited = new HashSet<Point>();
        var chestLocations = chests.Select(chest => chest.Location);
        visited.Add(start);
        queue.Enqueue(new SinglyLinkedList<Point>(start));

        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();
            if (chestLocations.Contains(currentPoint.Value))
                yield return currentPoint;

            foreach (var direction in Walker.PossibleDirections)
            {
                var nextPoint = new Point(currentPoint.Value.X + direction.X, currentPoint.Value.Y + direction.Y);

                if (IsValidNextPoint(map, nextPoint, visited))
                    continue;

                visited.Add(nextPoint);
                queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, currentPoint));
            }
        }
    }

    private static bool IsValidNextPoint(Map map, Point nextPoint, HashSet<Point> visitedPoints)
    {
        return !map.InBounds(nextPoint) ||
                  visitedPoints.Contains(nextPoint) ||
                  map.Dungeon[nextPoint.X, nextPoint.Y] != MapCell.Empty;
    }
}