namespace Dungeon;

public record Point(int X = 0, int Y = 0)
{
	public static Point Null = new(-1, -1);
	public static Point operator +(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
	public static Point operator -(Point p1, Point p2) => new(p1.X - p2.X, p1.Y - p2.Y);

	public override string ToString()
		=> $"{{X={X},Y={Y}}}";

	public virtual bool Equals(Point? other)
	{
		return other != null && X == other.X && Y == other.Y;
	}

	public bool IsNull
		=> X == -1 && Y == -1;

	public bool HasValue => !IsNull;
}