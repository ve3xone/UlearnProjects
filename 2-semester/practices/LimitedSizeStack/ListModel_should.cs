using System.Collections.Generic;
using NUnit.Framework;

namespace LimitedSizeStack;

[TestFixture]
public class ListModel_Should
{
	[Test]
	public void AddItems()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		Assert.AreEqual(new List<string>{"a", "bb", "ccc"}, model.Items);
	}

	[Test]
	public void RemoveFromTheEnd()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(2);
		Assert.AreEqual(new List<string> { "a", "bb" }, model.Items);
	}

	[Test]
	public void RemoveFromTheBeginning()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(0);
		Assert.AreEqual(new List<string> { "bb", "ccc" }, model.Items);
	}

	[Test]
	public void RemoveFromTheMiddle()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(1);
		Assert.AreEqual(new List<string> { "a", "ccc" }, model.Items);
		model.Undo();
		Assert.AreEqual(new List<string> { "a", "bb", "ccc" }, model.Items);
	}

	[Test]
	public void RemoveAndUndoAllItems()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.AddItem("ccc");
		model.RemoveItem(0);
		model.RemoveItem(0);
		model.RemoveItem(0);
		Assert.AreEqual(new List<string>(), model.Items);
		model.Undo();
		model.Undo();
		model.Undo();
		Assert.AreEqual(new List<string> { "a", "bb", "ccc" }, model.Items);
	}

	[Test]
	public void UndoAddOperations()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		Assert.AreEqual(true, model.CanUndo());
		model.Undo();
		Assert.AreEqual(0, model.Items.Count);
	}

	[Test]
	public void NotUndo_WhenEverythingIsUndone()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
	}

	[Test]
	public void Add_AfterUndo()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.Undo();
		model.AddItem("qq");
		Assert.AreEqual(new List<string> { "qq" }, model.Items);
	}

	[Test]
	public void Undo_AfterRemove()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.RemoveItem(1);
		model.Undo();
		Assert.AreEqual(new List<string> { "a", "bb" }, model.Items);
	}

	[Test]
	public void Remove_AfterUndo()
	{
		var model = new ListModel<string>(20);
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		model.RemoveItem(0);
		Assert.AreEqual(0, model.Items.Count);
	}

	[Test]
	public void NotUndo_WhenUndoLimitIsReached()
	{
		var model = new ListModel<string>(2);
		model.AddItem("a");
		model.AddItem("bb");
		model.RemoveItem(1);
		model.Undo();
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
		Assert.AreEqual(new List<string> {"a"}, model.Items);
	}

	[Test]
	public void CanUndo_ReturnsFalse_WhenUndoLimitIsReached()
	{
		var model = new ListModel<string>(1);
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("a");
		model.AddItem("bb");
		model.Undo();
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("ccc");
		Assert.AreEqual(true, model.CanUndo());
	}

	[Test]
	public void CanUndo_ReturnsFalse_WhenUndoLimitIsZero()
	{
		var model = new ListModel<string>(0);
		Assert.AreEqual(false, model.CanUndo());
		model.AddItem("a");
		model.AddItem("bb");
		Assert.AreEqual(false, model.CanUndo());
	}
}