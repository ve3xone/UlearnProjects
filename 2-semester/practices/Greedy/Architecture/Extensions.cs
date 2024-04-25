namespace Greedy.Architecture;

public static class Extensions
{
	public static Avalonia.Point MultiplyTransform(this Point point, double factor) =>
		new(point.X * factor, point.Y * factor);
}