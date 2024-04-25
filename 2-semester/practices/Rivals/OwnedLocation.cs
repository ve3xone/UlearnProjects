namespace Rivals;

public class OwnedLocation
{
	public readonly int Owner;
	public readonly Point Location;
	public readonly int Distance;

	public OwnedLocation(int owner, Point location, int distance)
	{
		Owner = owner;
		Location = location;
		Distance = distance;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is OwnedLocation))
			return false;
		var other = (OwnedLocation) obj;
		return Owner.Equals(other.Owner) && Location.Equals(other.Location) && Distance.Equals(other.Distance);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = Owner;
			hashCode = (hashCode * 397) ^ Location.GetHashCode();
			hashCode = (hashCode * 397) ^ Distance;
			return hashCode;
		}
	}


	public override string ToString()
	{
		return $"[Location: {Location}, Owner: {Owner}, Distance: {Distance}]";
	}
}