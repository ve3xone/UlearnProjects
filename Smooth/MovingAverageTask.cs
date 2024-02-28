using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        Queue<double> queue = new Queue<double>();
        double value = 0;

        foreach (var point in data)
        {
            queue.Enqueue(point.OriginalY);
            value += point.OriginalY;

            if (queue.Count > windowWidth)
            {
                value -= queue.Dequeue();
            }

            yield return point.WithAvgSmoothedY(value / queue.Count);
        }
    }
}