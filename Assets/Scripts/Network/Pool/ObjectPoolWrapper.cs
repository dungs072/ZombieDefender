using System;
using System.Collections.Generic;
using UnityEngine.Pool;

public class ObjectPoolWrapper<T> where T : class
{
    private readonly ObjectPool<T> _innerPool;
    private readonly HashSet<T> _activeSet;

    public ObjectPoolWrapper(ObjectPool<T> innerPool)
    {
        _innerPool = innerPool ?? throw new ArgumentNullException(nameof(innerPool));
        _activeSet = new HashSet<T>();
    }

    public T Get()
    {
        T obj = _innerPool.Get();
        _activeSet.Add(obj);
        return obj;
    }

    public void Release(T obj)
    {
        if (!_activeSet.Contains(obj))
        {
            return;
        }

        _activeSet.Remove(obj);
        _innerPool.Release(obj);
    }

    public void Clear()
    {
        _activeSet.Clear();
        _innerPool.Clear();
    }
}

