namespace Greedy.Architecture;

public readonly struct Point
{
	public int X { get; }
	public int Y { get; }

	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
	
	public static bool operator ==(Point point, Point other) => point.X == other.X && point.Y == other.Y;
	public static bool operator !=(Point point, Point other) => point.X != other.X || point.Y != other.Y;
	public static Point operator +(Point point, Point other) => new(point.X + other.X, point.Y + other.Y);
	public static Point operator -(Point point, Point other) => new(point.X - other.X, point.Y - other.Y);

	public override int GetHashCode()
		=> X * Y + X;
	
	public override string ToString()
		=> $"x: {X}; y: {Y};";
	
	public bool Equals(Point other)
	{
		return X == other.X && Y == other.Y;
	}

	public override bool Equals(object? obj)
	{
		return obj is Point other && Equals(other);
	}
}