using System.Collections.Generic;
using Avalonia.Media;
using GeometryTasks;

namespace GeometryPainting;

public static class SegmentExtensions
{
    private static readonly Dictionary<Segment, Color> _dic = new();

    public static Color GetColor(this Segment seg)
    {
        return _dic.TryGetValue(seg, out var color) ? color : Colors.Black;
    }

    public static void SetColor(this Segment seg, Color color)
    {
        _dic[seg] = color;
    }
}