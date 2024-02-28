using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        double value = 0;
        bool isFirst = true;

        foreach (var point in data)
        {
            if (isFirst)
            {
                isFirst = false;
                value = point.OriginalY;
            }
            else
            {
                value = alpha * point.OriginalY + (1 - alpha) * value;
            }

            yield return point.WithExpSmoothedY(value);
        }
    }
}