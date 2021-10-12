using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class FixedSizeQueue<T> where T : class
{
    List<T> queue = null;
    int size = 0;

    public FixedSizeQueue(int size)
    {
        queue = new List<T>();
        this.size = size;
    }

    public T Enqueue(T obj)
    {
        T res = null;
        if (queue.Count > size)
            res = queue.Dequeue();

        queue.Enqueue(obj);
        return res;
    }

    public T Dequeue()
    {
        return queue.Dequeue();
    }

    public int Count()
    {
        return queue.Count;
    }

    public void Clear()
    {
        queue.Clear();
    }

    public T this[int index]
    {
        get { return queue[index]; }
    }
}