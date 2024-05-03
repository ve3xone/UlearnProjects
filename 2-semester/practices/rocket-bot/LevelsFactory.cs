using System;
using System.Linq;

namespace rocket_bot;

public class LevelsFactory
{
	public static readonly Physics StandardPhysics = new();

	public static Level CreateLevel(Random random)
	{
		var checkpoints = Enumerable.Range(1, 4)
			.Select(i => new Vector(random.Next(600), random.Next(500))).ToArray();

		return new Level(new Rocket(new Vector(0, 500), Vector.Zero, -0.5 * Math.PI), checkpoints, StandardPhysics);
	}
}