using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class Walker
{
	private static readonly Dictionary<MoveDirection, Point> directionToOffset = new()
	{
		{ MoveDirection.Up, new Point(0, -1) },
		{ MoveDirection.Down, new Point(0, 1) },
		{ MoveDirection.Left, new Point(-1, 0) },
		{ MoveDirection.Right, new Point(1, 0) }
	};

	private static readonly Dictionary<Point, MoveDirection> offsetToDirection = new()
	{
		{ new Point(0, -1), MoveDirection.Up },
		{ new Point(0, 1), MoveDirection.Down },
		{ new Point(-1, 0), MoveDirection.Left },
		{ new Point(1, 0), MoveDirection.Right }
	};

	public static readonly IReadOnlyList<Point> PossibleDirections = offsetToDirection.Keys.ToList();


	public Point Position { get; }
	public Point PointOfCollision { get; }

	public Walker(Point position)
	{
		Position = position;
		PointOfCollision = Point.Null;
	}

	private Walker(Point position, Point pointOfCollision)
	{
		Position = position;
		PointOfCollision = pointOfCollision;
	}

	public Walker WalkInDirection(Map map, MoveDirection direction)
	{
		var newPoint = Position + directionToOffset[direction];
		if (!map.InBounds(newPoint))
			return new Walker(Position, Position);
		return map.Dungeon[newPoint.X, newPoint.Y] == MapCell.Wall
			? new Walker(newPoint, newPoint)
			: new Walker(newPoint);
	}

	public static MoveDirection ConvertOffsetToDirection(Point offset)
	{
		return offsetToDirection[offset];
	}
}