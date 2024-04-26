using Greedy.Architecture;
using System.Collections.Generic;
using System.Linq;

namespace Greedy
{
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
                                                            IEnumerable<Point> targets)
        {
            const int bestPrice = int.MaxValue;
            var chests = new HashSet<Point>(targets);
            var candidatesToOpen = new HashSet<Point> { start };
            var visitedNodes = new HashSet<Point>();
            var track = new Dictionary<Point, DijkstraData> { [start] = new DijkstraData(null, 0) };

            while (true)
            {
                Point? toOpen = GetToOpen(candidatesToOpen, track, bestPrice, null);

                if (toOpen == null)
                    yield break;
                if (chests.Contains(toOpen.Value))
                    yield return GetPath(track, toOpen.Value);

                UpdateNodes(toOpen.Value, state, track, candidatesToOpen, visitedNodes);
            }
        }

        private Point? GetToOpen(IEnumerable<Point> candidatesToOpen,
                                 IReadOnlyDictionary<Point, DijkstraData> track,
                                 int bestPrice,
                                 Point? toOpen)
        {
            foreach (var candidate in candidatesToOpen.Where(key => track[key].Price < bestPrice))
            {
                bestPrice = track[candidate].Price;
                toOpen = candidate;
            }

            return toOpen;
        }

        private PathWithCost GetPath(Dictionary<Point, DijkstraData> track, Point end)
        {
            var result = new List<Point>();
            Point? currentPoint = end;

            while (currentPoint != null)
            {
                result.Add(currentPoint.Value);
                currentPoint = track[currentPoint.Value].Previous;
            }

            result.Reverse();
            return new PathWithCost(track[end].Price, result.ToArray());
        }

        private readonly List<Point> Nodes = new()
        {
            new Point(0, 1),
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0)
        };

        private IEnumerable<Point> GetNodes(Point node, State state)
        {
            return Nodes.Select(point => point + node)
                        .Where(point => state.InsideMap(point) &&
                                        !state.IsWallAt(point));
        }

        private void UpdateNodes(Point nodeOpen, State state,
                                 Dictionary<Point, DijkstraData> track,
                                 HashSet<Point> candidatesToOpen,
                                 HashSet<Point> visitedNodes)
        {
            var nodes = GetNodes(nodeOpen, state);

            foreach (var node in nodes)
            {
                var price = track[nodeOpen].Price + state.CellCost[node.X, node.Y];

                if (!visitedNodes.Contains(node))
                    candidatesToOpen.Add(node);

                if (!track.ContainsKey(node) || price < track[node].Price)
                    track[node] = new DijkstraData(nodeOpen, price);
            }

            candidatesToOpen.Remove(nodeOpen);
            visitedNodes.Add(nodeOpen);
        }
    }

    public class DijkstraData
    {
        public Point? Previous { get; set; }
        public int Price { get; set; }

        public DijkstraData(Point? previous, int price)
        {
            Previous = previous;
            Price = price;
        }
    }
}