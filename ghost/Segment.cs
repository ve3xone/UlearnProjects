namespace hashes;

public class Segment
{
	public Vector Start { get; }
	public Vector End { get; }

	public Segment(Vector start, Vector end)
	{
		Start = start;
		End = end;
	}

	protected bool Equals(Segment other)
	{
		return Start.Equals(other.Start) && End.Equals(other.End);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Segment)obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return (Start.GetHashCode() * 397) ^ End.GetHashCode();
		}
	}
}