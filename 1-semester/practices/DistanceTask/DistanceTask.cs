using System;

namespace DistanceTask;

public static class DistanceTask
{
    // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        var projection = GetProjection(ax, ay, bx, by, x, y);

        // Если проекция находится внутри отрезка, расстояние до точки - это высота треугольника
        if (projection >= 0 && projection <= 1)
        {
            var projectionX = ax + projection * (bx - ax);
            var projectionY = ay + projection * (by - ay);
            var distanceSquaredX = (x - projectionX) * (x - projectionX);
            var distanceSquaredY = (y - projectionY) * (y - projectionY);
            return Math.Sqrt(distanceSquaredX + distanceSquaredY);
        }
        else
        {
            // Если проекция находится за пределами отрезка, расстояние до ближайшего конца отрезка
            var distanceToASquared = CalculateDistance(x, y, ax, ay);
            var distanceToBSquared = CalculateDistance(x, y, bx, by);
            return Math.Min(distanceToASquared, distanceToBSquared);
        }
    }

    public static double GetProjection(double ax, double ay, double bx, double by, double x, double y)
    {
        var segmentLength = (bx - ax) * (bx - ax) + (by - ay) * (by - ay);

        // Если отрезок вырожденный (длина равна нулю)
        if (segmentLength == 0)
        {
            return Math.Sqrt((x - ax) * (x - ax) + (y - ay) * (y - ay));
        }

        // Вычисляем проекцию точки на отрезок
        return ((x - ax) * (bx - ax) + (y - ay) * (by - ay)) / segmentLength;
    }

    private static double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
}