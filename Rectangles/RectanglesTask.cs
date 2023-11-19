using System;

namespace Rectangles;
public static class RectanglesTask
{
    // Пересекаются ли два прямоугольника (пересечение по границе и внутренность считаются пересечением)
    public static bool AreIntersected(Rectangle r1, Rectangle r2)
    {
        // Прямоугольники пересекаются, если один из них находится справа от другого
        // или если один из них находится выше другого. Если условие не выполняется, прямоугольники не пересекаются.
        return r1.Left <= r2.Right && r1.Right >= r2.Left && r1.Top <= r2.Bottom && r1.Bottom >= r2.Top;
    }

    // Площадь пересечения прямоугольников
    public static int IntersectionSquare(Rectangle r1, Rectangle r2)
    {
        // Если прямоугольники не пересекаются, площадь пересечения равна 0
        if (!AreIntersected(r1, r2)) return 0;

        // Найдем координаты левого верхнего угла и правого нижнего угла пересекающейся области
        // Вычислим ширину и высоту пересекающейся области
        (int width, int height) = GetWidthAndHeight(r1, r2);

        // Площадь пересечения равна произведению ширины и высоты
        return width * height;
    }

    // Если один из прямоугольников целиком находится внутри другого, вернуть номер внутреннего. Иначе вернуть -1
    public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
    {
        // Если координаты внутреннего прямоугольника находятся внутри координат внешнего прямоугольника,
        // значит, внутренний прямоугольник полностью находится внутри внешнего.
        // Проверяем оба случая (r1 внутри r2 и r2 внутри r1).
        if (IsInside(r1, r2))
        {
            return 0;
        }
        else if (IsInside(r2, r1))
        {
            return 1;
        }

        // Прямоугольники не вложены друг в друга
        return -1;
    }

    private static bool IsInside(Rectangle inner, Rectangle outer)
    {
        return inner.Left >= outer.Left && inner.Right <= outer.Right &&
               inner.Top >= outer.Top && inner.Bottom <= outer.Bottom;
    }

    private static (int Width, int Height) GetWidthAndHeight(Rectangle r1, Rectangle r2)
    {
        int width = Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left);
        int height = Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top);
        return (width, height);
    }
}