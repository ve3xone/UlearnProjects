namespace hashes;

public class Vector
{
	public double X { get; private set; }
	public double Y { get; private set; }

	public Vector(double x, double y)
	{
		X = x;
		Y = y;
	}

	public Vector Add(Vector other)
	{
		X += other.X;
		Y += other.Y;
		return this;
	}

	protected bool Equals(Vector other)
	{
		return X.Equals(other.X) && Y.Equals(other.Y);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Vector) obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return (X.GetHashCode()*397) ^ Y.GetHashCode();
		}
	}
}