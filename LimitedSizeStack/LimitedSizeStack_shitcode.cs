using System;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private Element<T> Head { get; set; }
    private Element<T> Tail { get; set; }
    private int Limit { get; set; }
    public int Count { get; private set; }

    public LimitedSizeStack(int undoLimit)
    {
        Limit = undoLimit;
    }

    public void Push(T item)
    {
        if (IsListEmpty())
        {
            if (Limit > 0)
            {
                AddFirstElement(item);
            }
        }
        else
        {
            if (Limit == Count)
            {
                RemoveFirstElement();
            }

            AddToTail(item);
        }
    }

    private bool IsListEmpty()
    {
        return Head == null && Tail == null;
    }

    private void AddFirstElement(T item)
    {
        Head = Tail = new Element<T> { Value = item };
        Count++;
    }

    private void RemoveFirstElement()
    {
        Head = Head.Next;
        if (Head != null)
        {
            Head.Prev = null;
        }
        Count--;
    }

    private void AddToTail(T item)
    {
        var newItem = new Element<T> { Value = item, Prev = Tail };
        Tail.Next = newItem;
        Tail = newItem;
        Count++;
    }

    public T Pop()
    {
        if (Count == 0 || Limit == 0)
        {
            throw new Exception();
        }

        var result = Tail.Value;
        if (Count == 1)
        {
            Head = null;
            Tail = null;
            Count--;
        }
        else
        {
            Tail = Tail.Prev;
            Count--;
        }

        return result;
    }
}

public class Element<T>
{
    public T Value;
    public Element<T> Next;
    public Element<T> Prev;
}