using System.Collections.Generic;
using System.Linq;

namespace rocket_bot;
public class Channel<T> where T : class
{
    private readonly List<T> _items = new();
    
    public T this[int index]
    {
        get
        {
            lock (_items)
            {
                if (index >= 0 && index < _items.Count)
                    return _items[index];
                return null;
            }
        }
        set
        {
            lock (_items)
            {
                if (index >= _items.Count)
                {
                    while (_items.Count <= index)
                        _items.Add(null);
                }
                _items[index] = value;
                _items.RemoveRange(index + 1, _items.Count - index - 1);
            }
        }
    }
    
    public T? LastItem()
    {
        lock (_items)
        {
            if (_items.Count > 0)
                return _items.Last();
            return null;
        }
    }
    
    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
    {
        lock (_items)
        {
            if (LastItem() == knownLastItem)
                _items.Add(item);
        }
    }
    
    public int Count
    {
        get
        {
            lock (_items)
            {
                return _items.Count;
            }
        }
    }
}