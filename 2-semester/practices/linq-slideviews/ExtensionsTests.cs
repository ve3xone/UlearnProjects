using System;
using System.Linq;
using NUnit.Framework;

namespace linq_slideviews;

[TestFixture]
public class ExtensionsTests
{
	[Test]
	public void MedianOfEmptyList_Fails()
	{
		Assert.Throws<InvalidOperationException>(
			() => new double[0].Median());
	}

	[Test]
	public void MedianOfSingleItemList()
	{
		var actual = new[] { 42.0 }.Median();
		Assert.That(actual, Is.EqualTo(42.0));
	}

	[Test]
	public void MedianOfTwoItemsList()
	{
		var actual = new[] { 1.0, 2.0 }.Median();
		Assert.That(actual, Is.EqualTo(1.5).Within(1e-7));
	}

	[TestCase(new[] { 1.0, 10, 9, 2, 3 }, 3.0)]
	[TestCase(new[] { 10, 9, 2, 3.0 }, 6.0)]
	public void MedianOfUnsortedList(double[] list, double expectedMedian)
	{
		var actual = list.Median();
		Assert.That(actual, Is.EqualTo(expectedMedian));
	}

	[Test]
	public void MedianOfLongOddList()
	{
		var list = Enumerable.Range(10, 1001).Select(i => (double)i).ToList();
		var actual = list.Median();
		Assert.That(actual, Is.EqualTo(list[1001 / 2]));
	}

	[Test]
	public void MedianOfLongEvenList()
	{
		var list = Enumerable.Range(10, 1000).Select(i => (double)i).ToList();
		var actual = list.Median();
		Assert.That(actual, Is.EqualTo((list[500] + list[499]) / 2).Within(1e-7));
	}

	[Test]
	public void MedianOfThreeItemsList()
	{
		var actual = new[] { 1.0, 2.0, 3000.0 }.Median();
		Assert.That(actual, Is.EqualTo(2.0));
	}

	[Test]
	public void MedianOfSequenceWithRepetitions()
	{
		var actual = new[] { 1.0, 1.0, 1.0, 2.0, 3.0 }.Median();
		Assert.That(actual, Is.EqualTo(1.0));
	}

	[Test]
	public void BigramsOfSingleItemList()
	{
		var actual = new[] { 42.0 }.Bigrams();
		Assert.That(actual, Is.Empty);
	}

	[Test]
	public void BigramsOfTwoItemList()
	{
		var actual = new[] { 1, 2 }.Bigrams();
		Assert.That(actual, Is.EqualTo(new[] { (1, 2) }));
	}

	[Test]
	public void BigramsOfLongList()
	{
		var count = 1000;
		var actual = Enumerable.Range(100, count).Bigrams().ToList();
		Assert.That(actual, Has.Count.EqualTo(count - 1));
		for (var index = 0; index < count - 1; index++)
		{
			var tuple = actual[index];
			Assert.That(tuple.First, Is.EqualTo(100 + index));
			Assert.That(tuple.Second, Is.EqualTo(101 + index));
		}
	}

	[Test]
	public void BigramsOfReferenceTypeSequence()
	{
		var actual = new[] { "1", null, "2" }.Bigrams();
		Assert.That(actual,
			Is.EqualTo(
				new[]
				{
					("1", null),
					((string)null, "2"),
				}));
	}
}