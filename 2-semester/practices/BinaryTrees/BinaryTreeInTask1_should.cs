using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryTrees;

[TestFixture]
public class BinaryTreeInTask1_should
{
	private readonly Random rnd = new();

	private void TestAddContains<T>(IEnumerable<T> values)
		where T : IComparable
	{
		var shuffledValues = values.Shuffle();
		var toAdd = shuffledValues.Where(z => rnd.NextDouble() > 0.7).ToList();
		var tree = new BinaryTree<T>();
		foreach (var e in toAdd)
			tree.Add(e);
		foreach (var e in shuffledValues)
			Assert.AreEqual(toAdd.Contains(e), tree.Contains(e));
	}

	[Test]
	public void EmptyTreeDoesNotContainDefaultValue()
	{
		var intTree = new BinaryTree<int>();
		Assert.IsFalse(intTree.Contains(0));
		var stringTree = new BinaryTree<string>();
		Assert.IsFalse(stringTree.Contains(null));
	}

	[Test]
	public void WorkWithIntegers()
	{
		TestAddContains(Enumerable.Range(0, 20));
	}


	[Test]
	public void WorkWithManyIntegers()
	{
		RunWithTimeout(() => TestAddContains(Enumerable.Range(0, 2000)), 500, "Your tree is too slow");
	}

	[Test]
	public void WorkWithStrings()
	{
		TestAddContains(Enumerable.Range(1, 20).Select(z => new string((char)('a' + z), z)));
	}

	[Test]
	public void WorkWithTuples()
	{
		TestAddContains(Enumerable.Range(1, 20).Select(z => Tuple.Create(z / 3, new string((char)('a' + z), z))));
	}

	protected static void RunWithTimeout(Action work, int timeout, string message)
	{
		var task = Task.Run(work);
		if (!task.Wait(timeout))
			throw new AssertionException(message);
	}
}