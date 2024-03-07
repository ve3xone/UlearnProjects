using System.Collections.Generic;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var windowMaxs = new LinkedList<double>();
        var window = new Queue<double>();

        foreach (var dataPoint in data)
        {
            if (window.Count == windowWidth)
            {
                var removed = window.Dequeue();
                if (windowMaxs.First.Value.Equals(removed)) windowMaxs.RemoveFirst();
            }

            while (windowMaxs.Count > 0 && windowMaxs.Last.Value < dataPoint.OriginalY)
                windowMaxs.RemoveLast();

            windowMaxs.AddLast(dataPoint.OriginalY);
            window.Enqueue(dataPoint.OriginalY);

            yield return dataPoint.WithMaxY(windowMaxs.First.Value);
        }
    }
}