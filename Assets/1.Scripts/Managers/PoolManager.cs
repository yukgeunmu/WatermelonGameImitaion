using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : IManager
{
    private readonly Dictionary<string, IPool> pools = new();

    private Transform poolRoot;

    public void Initialize()
    {
    }

    public void Dispose()
    {
        foreach (IPool pool in pools.Values)
        {
            pool.Clear();
        }

        pools.Clear();
    }

    public void SetRoot(Transform root)
    {
        poolRoot = root;
    }

    public void CreatePool<T>(string key, Func<T> createFunc, int preloadCount, Transform root = null) where T : Component
    {
        if (pools.ContainsKey(key))
            return;

        GameObject childRootObj = new GameObject($"{key}_Pool");

        if (poolRoot != null)
        {
            childRootObj.transform.SetParent(poolRoot, false);
        }

        GenericPool<T> pool = new(createFunc, preloadCount, root == null ? childRootObj.transform : root);

        pools.Add(key, pool);
    }

    public T Get<T>(string key) where T : Component
    {
        if (!pools.TryGetValue(key, out IPool pool))
        {
            Debug.LogError($"Pool Not Found : {key}");

            return null;
        }

        if (pool is GenericPool<T> genericPool)
        {
            return genericPool.Get();
        }

        Debug.LogError($"Pool Type Mismatch. Key '{key}' is not of type {typeof(T).Name}");
        return null;
    }

    public void Return<T>(string key, T item) where T : Component
    {
        if (!pools.TryGetValue(key, out IPool pool))
        {
            Debug.LogError($"Pool Not Found : {key}");
            UnityEngine.Object.Destroy(item.gameObject);
            return;
        }


        if (pool is GenericPool<T> genericPool)
        {
            genericPool.Return(item);
        }
        else
        {
            Debug.LogError($"Pool Type Mismatch. Cannot return {typeof(T).Name} to {key}. Destroying item.");
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }
}