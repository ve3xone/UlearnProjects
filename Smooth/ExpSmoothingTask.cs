using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        double value = double.NaN;

        foreach (var point in data)
        {
            if (double.IsNaN(value))
            {
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