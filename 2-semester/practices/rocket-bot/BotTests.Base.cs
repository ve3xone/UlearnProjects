using System;
using NUnit.Framework;

namespace rocket_bot;

public class BotTests_Base
{
	protected Random random;
	protected Channel<Rocket> channel;
	protected Rocket rocket;
	
	[SetUp]
	public void Init()
	{
		random = new Random(223243);
		channel = new Channel<Rocket>();
		rocket = new Rocket(new Vector(100, 100), Vector.Zero, -0.5 * Math.PI);
	}
}