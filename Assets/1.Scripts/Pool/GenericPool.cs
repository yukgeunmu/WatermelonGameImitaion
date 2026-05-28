using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericPool<T> : IPool where T : Component
{
    private readonly Queue<T> pool = new();

    private readonly Func<T> createFunc;

    private readonly Transform root;

    public GenericPool( Func<T> createFunc, int preloadCount,  Transform root)
    {
        this.createFunc = createFunc;
        this.root = root;

        Preload(preloadCount);
    }

    private void Preload(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T item = Create();

            Return(item);
        }
    }

    private T Create()
    {
        T item = createFunc.Invoke();

        return item;
    }

    public T Get()
    {
        T item;

        if (pool.Count > 0)
        {
            item = pool.Dequeue();
        }
        else
        {
            item = Create();
        }

        item.gameObject.SetActive(true);

        return item;
    }
    public void Return(T item)
    {
        item.gameObject.SetActive(false);

        item.transform.SetParent(root, false);

        pool.Enqueue(item);
    }


    public void Clear()
    {
        while (pool.Count > 0)
        {
            T item = pool.Dequeue();
            if (item != null) // 유니티 오브젝트가 이미 파괴되었을 수 있으므로 null 체크
            {
                UnityEngine.Object.Destroy(item.gameObject);
            }
        }
        // 큐는 Dequeue를 돌면서 자연스럽게 비워집니다.
    }
}