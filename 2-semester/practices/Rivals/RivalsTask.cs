using System.Collections.Generic;
using System.Linq;

namespace Rivals
{
    public class RivalsTask
    {
        private static readonly Point[] DirectionsPoints =
        {
            new Point(1, 0),
            new Point(-1, 0),
            new Point(0, 1),
            new Point(0, -1)
        };

        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var chests = map.Chests.ToHashSet();
            var points = new Dictionary<Point, OwnedLocation>();
            var queue = new Queue<Point>();

            InitializePlayers(map, points, queue);
            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                ExploreNeighbors(point, map, points, queue, chests);
            }

            return points.Values;
        }

        private static void InitializePlayers(Map map,
                                              Dictionary<Point, OwnedLocation> points,
                                              Queue<Point> queue)
        {
            for (int i = 0; i < map.Players.Length; i++)
            {
                var player = map.Players[i];
                points.Add(player, new OwnedLocation(i, player, 0));
                queue.Enqueue(player);
            }
        }

        private static void ExploreNeighbors(Point point, Map map,
                                             Dictionary<Point, OwnedLocation> points,
                                             Queue<Point> queue, HashSet<Point> chests)
        {
            foreach (var nextPoint in GetNextPoints(point, map, points))
            {
                if (IsValidMove(nextPoint, map, points))
                {
                    points.Add(nextPoint, new OwnedLocation(points[point].Owner,
                                                            nextPoint,
                                                            points[point].Distance + 1));
                    if (!chests.Contains(nextPoint))
                        queue.Enqueue(nextPoint);
                }
            }
        }

        private static IEnumerable<Point> GetNextPoints(Point point, Map map,
                                                        Dictionary<Point, OwnedLocation> points)
        {
            return DirectionsPoints.Select(direction => direction + point)
                                   .Where(nextPoint => IsValidMove(nextPoint, map, points));
        }

        private static bool IsValidMove(Point point, Map map,
                                        Dictionary<Point, OwnedLocation> points)
        {
            return !points.ContainsKey(point) &&
                   map.InBounds(point) &&
                   map.Maze[point.X, point.Y] != MapCell.Wall;
        }
    }
}