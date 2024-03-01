using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace hashes;

[TestFixture]
public class ReadonlyBytesTests
{
	[Test]
	public void Creation()
	{
		new ReadonlyBytes(1, 2, 3);
	}

	[Test]
	public void CreationException()
	{
		Assert.Throws<ArgumentNullException>(() => { new ReadonlyBytes(null); });
	}

	[Test]
	public void Length()
	{
		var items = new ReadonlyBytes(1, 2, 3);
		Assert.AreEqual(3, items.Length);
	}

	[Test]
	public void Indexer()
	{
		var items = new ReadonlyBytes(4, 1, 2);
		Assert.AreEqual(4, items[0]);
		Assert.AreEqual(1, items[1]);
		Assert.AreEqual(2, items[2]);
	}

	[Test]
	public void IndexOutOfRangeException()
	{
		var items = new ReadonlyBytes(4, 1, 2);
		// Обращение к индексу за границами исходного массива должно приводить 
		// к исключению типа IndexOutOfRangeException
		Assert.Throws<IndexOutOfRangeException>(() =>
		{
			var z = items[100500];
		});
	}

	[Test]
	public void Enumeration()
	{
		var data = new ReadonlyBytes(1, 2, 3);

		var list = new List<byte>();
		foreach (var x in data)
		{
			list.Add(x);
		}

		Assert.AreEqual(new byte[] { 1, 2, 3 }, list);
	}

	[Test]
	public void EqualOnSameBytes()
	{
		// ReSharper disable EqualExpressionComparison
		Assert.IsTrue(new ReadonlyBytes(new byte[0]).Equals(new ReadonlyBytes(new byte[0])));
		Assert.IsTrue(new ReadonlyBytes(100).Equals(new ReadonlyBytes(100)));
		Assert.IsTrue(new ReadonlyBytes(1, 2, 3).Equals(new ReadonlyBytes(1, 2, 3)));
		var items = new ReadonlyBytes(4, 2, 67, 1);
		Assert.IsTrue(items.Equals(items));
		// ReSharper restore EqualExpressionComparison
	}

	[Test]
	public void NotEqualIfBytesDiffer()
	{
		Assert.IsFalse(new ReadonlyBytes(1, 2, 3).Equals(new ReadonlyBytes(1, 2, 4)));
		Assert.IsFalse(new ReadonlyBytes(1, 2, 3).Equals(new ReadonlyBytes(1, 2, 3, 4)));
		Assert.IsFalse(new ReadonlyBytes(1, 2, 3).Equals(new ReadonlyBytes(1, 2)));
		Assert.IsFalse(new ReadonlyBytes(1, 2, 3).Equals(new ReadonlyBytes(3, 2, 1)));
	}

	[TestCase(new byte[] { })]
	[TestCase(new byte[] { 1, 2, 4 })]
	[TestCase(new byte[] { 0 })]
	public void NotEqualWithOtherTypes(byte[] bytes)
	{
		Assert.IsFalse(new ReadonlyBytes(bytes).Equals(null));
		Assert.IsFalse(new ReadonlyBytes(bytes).Equals("string"));
		Assert.IsFalse(new ReadonlyBytes(bytes).Equals(new DerivedFromReadonlyBytes()));
	}

	[Test]
	public void HashCode()
	{
		Assert.AreEqual(new ReadonlyBytes().GetHashCode(), new ReadonlyBytes().GetHashCode());
		Assert.AreEqual(new ReadonlyBytes(1, 2, 3).GetHashCode(), new ReadonlyBytes(1, 2, 3).GetHashCode());
		Assert.AreNotEqual(new ReadonlyBytes(1, 2, 3).GetHashCode(), new ReadonlyBytes(1, 2, 3, 4).GetHashCode());
		Assert.AreNotEqual(new ReadonlyBytes(1, 2, 3).GetHashCode(), new ReadonlyBytes(1, 2, 4).GetHashCode());
		Assert.AreNotEqual(new ReadonlyBytes(1, 2, 3).GetHashCode(), new ReadonlyBytes(1, 2).GetHashCode());
		Assert.AreNotEqual(new ReadonlyBytes(1, 2, 3).GetHashCode(), new ReadonlyBytes(3, 2, 1).GetHashCode());
		Assert.AreNotEqual(new ReadonlyBytes(1, 0).GetHashCode(), new ReadonlyBytes(0, 31).GetHashCode());

		var items = new ReadonlyBytes(4, 2, 67, 1);
		Assert.AreEqual(items.GetHashCode(), items.GetHashCode());
	}

	[Test]
	public void ToStringIsOverridden()
	{
		Assert.AreEqual("[2]", new ReadonlyBytes(2).ToString());
		Assert.AreEqual("[]", new ReadonlyBytes().ToString());
		Assert.AreEqual("[2, 3, 5]", new ReadonlyBytes(2, 3, 5).ToString());
	}

	[Test]
	public void MassiveCallToGetHashCodeOfLargeBytesArray()
	{
		var items = new ReadonlyBytes(Enumerable.Repeat((byte)6, 100000).ToArray());
		var hash = items.GetHashCode();
		for (int i = 0; i < 100000; i++)
			Assert.AreEqual(hash, items.GetHashCode());
	}

	public class DerivedFromReadonlyBytes : ReadonlyBytes
	{
		public DerivedFromReadonlyBytes() : base()
		{
		}
	}
}