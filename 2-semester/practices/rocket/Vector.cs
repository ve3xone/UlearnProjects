using System;

namespace func_rocket;

public record Vector(double X, double Y)
{
	public static readonly Vector Zero = new(0, 0);

	public double Length => Math.Sqrt(X * X + Y * Y);

	public double Angle => Math.Atan2(Y, X);

	public override string ToString() => $"X: {X}, Y: {Y}";

	public override int GetHashCode()
	{
		unchecked
		{
			return (X.GetHashCode() * 397) ^ Y.GetHashCode();
		}
	}

	public static Vector operator -(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y);

	public static Vector operator *(Vector a, double k) => new(a.X * k, a.Y * k);

	public static Vector operator /(Vector a, double k) => new(a.X / k, a.Y / k);

	public static Vector operator *(double k, Vector a) => a * k;

	public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);

	public Vector Normalize() => Length > 0 ? this * (1 / Length) : this;

	public Vector Rotate(double angle) =>
		new(X * Math.Cos(angle) - Y * Math.Sin(angle), X * Math.Sin(angle) + Y * Math.Cos(angle));

	public Vector BoundTo(Vector size) => new(Math.Max(0, Math.Min(size.X, X)), Math.Max(0, Math.Min(size.Y, Y)));
}