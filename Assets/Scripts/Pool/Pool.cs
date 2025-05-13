using System;
using System.Collections.Generic;

public class Pool<T> where T : IPollable
{
    private readonly Queue<T> _items = new();
    private readonly Func<T> _createFunc;

    public Pool(Func<T> createFunc)
    {
        createFunc.ThrowIfNull();
        _createFunc = createFunc;
    }

    public int ReleaseCount { get; private set; }

    public T Get()
    {
        T item = _items.Count == Constants.Zero ? _createFunc.Invoke() : _items.Dequeue();

        item.Enable();
        ReleaseCount++;

        return item;
    }

    public void Return(T item)
    {
        item.ThrowIfNull();

        item.Disable();
        _items.Enqueue(item);
        ReleaseCount--;
    }
}
