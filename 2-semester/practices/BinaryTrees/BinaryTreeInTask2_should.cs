using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace BinaryTrees;

[TestFixture]
public class BinaryTreeInTask2_should
{
	public void TestIEnumerable<T>(IEnumerable<T> _values)
		where T : IComparable
	{
		var values = _values.Shuffle();
		var orderedValues = values.OrderBy(z => z).ToList();
		var tree = new BinaryTree<T>();
		foreach (var e in values)
			tree.Add(e);

		// ReSharper disable once IsExpressionAlwaysTrue
		Assert.True(tree is IEnumerable<T>, "Your binary tree does not implement IEnumerable");
		var en = (IEnumerable<T>)tree;
		Assert.AreEqual(orderedValues, en.ToList());
	}

	[TestCase(0, 1)]
	[TestCase(0, 2)]
	[TestCase(0, 30)]
	[TestCase(20, 100)]
	[TestCase(0, 10000)]
	public void SortIntegers(int start, int end)
	{
		TestIEnumerable(Enumerable.Range(start, end));
	}

	//Эта черная магия нужна, чтобы код тестов, которые используют индексацию, компилировался 
	//при отсутствии индексатора в вашем классе.
	//Совсем скоро вы и сами научитесь писать что-то подобное.

	public static PropertyInfo GetIndexer<T>(BinaryTree<T> t)
		where T : IComparable
	{
		return t.GetType()
			.GetProperties()
			.Select(z => new { prop = z, ind = z.GetIndexParameters() })
			.SingleOrDefault(z => z.ind.Length == 1 && z.ind[0].ParameterType == typeof(int))
			?.prop;
	}

	public static Func<int, T> MakeAccessor<T>(BinaryTree<T> tree)
		where T : IComparable
	{
		var prop = GetIndexer(tree);
		var param = Expression.Parameter(typeof(int));
		return Expression.Lambda<Func<int, T>>(
				Expression.MakeIndex(Expression.Constant(tree), prop, new[] { param }),
				param)
			.Compile();
	}

	public void Test<T>(IEnumerable<T> values)
		where T : IComparable
	{
		var shuffledValues = values.Shuffle();
		var tree = new BinaryTree<T>();
		if (GetIndexer(tree) == null)
			Assert.Fail("Your BinaryTree does not implement indexing");
		foreach (var e in shuffledValues)
			tree.Add(e);
		var orderedValues = shuffledValues.OrderBy(z => z).ToList();
		var indexer = MakeAccessor(tree);
		for (var i = 0; i < orderedValues.Count; i++)
			Assert.AreEqual(orderedValues[i], indexer(i));
	}

	[TestCase(0, 0)]
	[TestCase(0, 1)]
	[TestCase(0, 2)]
	[TestCase(0, 30)]
	[TestCase(20, 100)]
	[TestCase(0, 10000)]
	public void SupportIndexersForIntValues(int start, int count)
	{
		Test(Enumerable.Range(start, count));
	}

	[TestCase(0, 0)]
	[TestCase(0, 1)]
	[TestCase(0, 2)]
	[TestCase(0, 30)]
	[TestCase(20, 100)]
	public void SupportIndexersForStringValues(int start, int count)
	{
		Test(Enumerable.Range(start, count).Select(n => n.ToString()));
	}

	[TestCase(100000)]
	public void CheckIEnumeratorIsLazy(int size)
	{
		var values = Enumerable.Range(0, size).Shuffle();
		var tree = new BinaryTree<int>();
		foreach (var e in values)
			tree.Add(e);
		RunWithTimeout(() => IEnumeratorIsLazy(tree), 500, "Your binary tree should be lazy");
	}

	public void IEnumeratorIsLazy(BinaryTree<int> tree)
	{
		var enumerable = tree as IEnumerable<int>;
		Assert.True(enumerable != null, "Your binary tree does not implement IEnumerable");
		for (var i = 0; i < 10000; ++i)
		{
			// ReSharper disable once PossibleMultipleEnumeration
			Assert.AreEqual(0, enumerable.First());
		}
	}

	[Test]
	public void WorkWithMixedAddAndContains()
	{
		RunWithTimeout(MixedTest, 750, "Your tree is too slow");
	}

	public void MixedTest()
	{
		var tree = new BinaryTree<int>();
		var indexer = MakeAccessor(tree);
		var stream = File.Open("test.txt", FileMode.Open);
		using var sr = new StreamReader(stream);
		while (!sr.EndOfStream)
		{
			var line = sr.ReadLine() ?? throw new Exception("Command expected!");
			var values = line.Split();
			var command = values.First();
			if (command == "Add")
			{
				foreach (var value in values.Skip(1))
				{
					tree.Add(int.Parse(value));
				}
			}
			else
			{
				var index = int.Parse(values[1]);
				var value = int.Parse(values[2]);
				Assert.AreEqual(value, indexer(index));
			}
		}
	}

	static void RunWithTimeout(Action work, int timeout, string message)
	{
		var task = Task.Run(work);
		if (!task.Wait(timeout))
			throw new AssertionException(message);
	}

	[Test]
	public void GetEnumeratorWorksCorrectly_WhenTreeIsEmpty()
	{
		var tree = new BinaryTree<int>();
		using var enumerator = tree.GetEnumerator();
		Assert.IsFalse(enumerator.MoveNext());
	}
}