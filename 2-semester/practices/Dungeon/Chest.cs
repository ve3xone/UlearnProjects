namespace Dungeon;

public class Chest
{
	public const byte MaxValue = 9;
	public byte Value { get; private set; }
    public Point Location { get; private set; }

    public Chest(Point location, byte value)
    {
        Location = location;
        if (value <= MaxValue) { Value = value; }
    }
}

public class EmptyChest : Chest
{
	public EmptyChest(Point location) : base(location, byte.MinValue) { }
}