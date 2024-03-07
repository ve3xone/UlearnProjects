using System;
using System.Drawing;
using System.Linq;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var bestOrder = MakeTrivialPermutation(checkpoints.Length);
            var bestLength = checkpoints.GetPathLength(bestOrder);

            var currentRoute = new int[checkpoints.Length];
            currentRoute[0] = 0;

            bestLength = MakePermutation(currentRoute, 0, 1, bestOrder, bestLength, checkpoints);

            return bestOrder;
        }

        private static int[] MakeTrivialPermutation(int size)
        {
            return Enumerable.Range(0, size).ToArray();
        }

		private static double MakePermutation(int[] route, double length, 
											  int position, int[] bestOrder, 
											  double bestLength, Point[] checkpoints)
        {
            if (length >= bestLength)
                return bestLength;

            if (position == route.Length)
            {
                Array.Copy(route, bestOrder, route.Length);
                return length;
            }

            for (var i = 0; i < checkpoints.Length; i++)
            {
                if (Array.IndexOf(route, i, 0, position) != -1)
                    continue;

                route[position] = i;
                var step = checkpoints[i].DistanceTo(checkpoints[route[position - 1]]);
                bestLength = MakePermutation(route, length + step, position + 1, bestOrder, bestLength, checkpoints);
            }

            return bestLength;
        }
    }
}