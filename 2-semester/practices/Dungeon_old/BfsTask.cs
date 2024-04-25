using System.Collections.Generic;

namespace Dungeon;

public static class BfsTask
{
    /// <summary>
    /// Находит пути от начальной точки до сундуков на карте с использованием алгоритма обхода в ширину
    /// </summary>
    /// <param name="map">Карта подземелья</param>
    /// <param name="start">Начальная точка</param>
    /// <param name="chests">Массив сундуков</param>
    /// <returns>Перечисление связных списков точек, представляющих найденные пути</returns>
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        var visited = new HashSet<Point> { start };
        var chestSet = new HashSet<Point>(chests);
        queue.Enqueue(new SinglyLinkedList<Point>(start));

        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();
            if (chestSet.Contains(currentPoint.Value))
                yield return currentPoint;

            foreach (var direction in Walker.PossibleDirections)
            {
                var nextPoint = new Point(currentPoint.Value.X + direction.X, currentPoint.Value.Y + direction.Y);

                if (IsValidNextPoint(map,nextPoint,visited))
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