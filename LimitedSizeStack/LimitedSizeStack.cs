using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private int size;
    private LinkedList<T> items = new();

    public int Count
    {
        get
        {
            if (items.Count != null)
                return items.Count;
            else
                return 0; //Если лист (стек) пустой
        }
    }

    public LimitedSizeStack(int limit) => size = limit;

    public void Push(T item)
    {
        if (size == 0) //Если не задан размер то выходим
            return;
        if (items.Count >= size)
            items.RemoveFirst();
        items.AddLast(item);
    }

    public T Pop()
    {
        if (items.Count == 0) // И вот если лист пустой то выводим Exception 
            throw new InvalidOperationException("Стек пустой");
        var lastValue = items.Last.Value;
        items.RemoveLast();
        return lastValue;
    }
}