namespace hashes;

public class Robot
{
	public static int BatteryCapacity = 100;

	private readonly string id;
	public readonly double Battery;

	public Robot(string id)
		:this(id, BatteryCapacity)
	{
	}
	public Robot(string id, double battery)
	{
		this.id = id;
		Battery = battery;
	}

	public Robot Move(int distance, double consumptionRate)
	{
		return new Robot(id, Battery - distance / consumptionRate);
	}

	public override string ToString()
	{
		return $"RobotId {id}, battery: {Battery}";
	}

	protected bool Equals(Robot other)
	{
		return string.Equals(id, other.id) && Battery.Equals(other.Battery);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Robot) obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return ((id.GetHashCode()*397) ^ Battery.GetHashCode())*397 ^ BatteryCapacity;
		}
	}
}