using NUnit.Framework;

namespace LimitedSizeStack;

[TestFixture]
public class LimitedSizeStack_PerformanceTest
{
	[Test, Timeout(500), Description("Push должен работать быстро, даже при большом лимите на размер стека")]
	public void Push_ShouldTakeConstantTime()
	{
		var undoLimit = 100000;
		var stack = new LimitedSizeStack<int>(undoLimit);
		for (var i = 0; i < 5 * undoLimit; ++i)
		{
			stack.Push(0);
		}
		Assert.AreEqual(undoLimit, stack.Count);
	}

	[Test, Timeout(500), Description("Pop должен работать быстро, даже при большом лимите на размер стека")]
	public void Pop_ShouldTakeConstantTime()
	{
		var undoLimit = 200000;
		var stack = new LimitedSizeStack<int>(undoLimit);
		for (var i = 0; i < undoLimit; ++i)
		{
			stack.Push(0);
		}
		Assert.AreEqual(undoLimit, stack.Count);
		for (var i = 0; i < undoLimit; ++i)
		{
			stack.Pop();
		}
		Assert.AreEqual(0, stack.Count);
	}
}