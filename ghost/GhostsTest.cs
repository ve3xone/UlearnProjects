using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace hashes;

[TestFixture]
public class GhostsTest
{
	[Test]
	public void GhostVectorBreaksHashSet()
	{
        AssertThatDoMagicBreaksHashSet<Vector, GhostsTask>();
	}

	[Test]
	public void GhostSegmentBreaksHashSet()
	{
        AssertThatDoMagicBreaksHashSet<Segment, GhostsTask>();
	}

	[Test]
	public void GhostCatBreaksHashSet()
	{
        AssertThatDoMagicBreaksHashSet<Cat, GhostsTask>();
	}

	[Test]
	public void GhostDocumentBreaksHashSet()
	{
        AssertThatDoMagicBreaksHashSet<Document, GhostsTask>();
	}

	[Test]
	public void GhostRobotBreaksHashSet()
	{
        AssertThatDoMagicBreaksHashSet<Robot, GhostsTask>();
	}

	private static void AssertThatDoMagicBreaksHashSet<TItem, TGhostsTask>() where TGhostsTask : IMagic, IFactory<TItem>, new()
	{
		TGhostsTask task = new();
		var ghostItem = task.Create();

		var set = new HashSet<TItem> { ghostItem };
		Assert.IsTrue(set.Contains(task.Create()));

		task.DoMagic();
		Assert.AreEqual(1, set.Count, "HashSet still contains some element after DoMagic()");
		Assert.AreEqual(ghostItem, set.ToArray()[0], "The single item in HashSet equals ghostItem after DoMagic()");
		Assert.IsFalse(set.Contains(ghostItem), "ghostItem should disappear from HashSet after DoMagic()");

		set.Add(ghostItem);
		Assert.AreEqual(2, set.Count, "HashSet Add and Count should work incorrectly after DoMagic()");
	}
}