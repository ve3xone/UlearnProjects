using NUnit.Framework;
using System.Numerics;

namespace Tickets;

[TestFixture]
public class TicketsTask_should
{
	private static void MakeTest(int halfLen, int totalSum, string answer)
	{
		var result = BigInteger.Parse(answer);
		Assert.AreEqual(result, TicketsTask.Solve(halfLen, totalSum));
	}

	[Test]
	public void Test1() { MakeTest(2, 2, "4"); }
	[Test]
	public void Test2() { MakeTest(2, 5, "0"); }
	[Test]
	public void Test6() { MakeTest(10, 10, "4008004"); }
	[Test]
	public void Test7() { MakeTest(20, 20, "401200499400100"); }
}