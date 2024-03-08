using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class HistoryItem<TItem>
{
    public TItem Value;
    public Actions Action;
    public int Index;
}

public enum Actions
{
    Add,
    Remove
}

public class ListModel<TItem>
{
    private readonly LimitedSizeStack<HistoryItem<TItem>>   History;
    public readonly List<TItem> Items;
    public int UndoLimit;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit){ }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
        History = new LimitedSizeStack<HistoryItem<TItem>>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        Items.Add(item);
        History.Push(new HistoryItem<TItem> { Value = item, Action = Actions.Add });
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= Items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var removedItem = Items[index];
        Items.RemoveAt(index);
        History.Push(new HistoryItem<TItem> { Value = removedItem, Action = Actions.Remove, Index = index });
    }

    public bool CanUndo()
    {
        return History.Count > 0;
    }

    public void Undo()
    {
        if (CanUndo())
        {
            var lastAction = History.Pop();
            if (lastAction.Action == Actions.Add)
            {
                Items.Remove(lastAction.Value);
            }
            else if (lastAction.Action == Actions.Remove)
            {
                if (lastAction.Index >= 0 && lastAction.Index < Items.Count)
                {
                    Items.Insert(lastAction.Index, lastAction.Value);
                }
                else
                {
                    Items.Add(lastAction.Value);
                }
            }
        }
    }
}