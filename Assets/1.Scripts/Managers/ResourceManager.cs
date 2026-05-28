using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : IManager
{
    private readonly Dictionary<Type, object> resourceSO = new();

    private readonly Dictionary<string, object> resourcesAsset = new();

    private readonly Dictionary<Type, AsyncOperationHandle> handleSO = new();

    private readonly Dictionary<string, AsyncOperationHandle> handlesAsset = new();

    public void Initialize()
    {
    }

    public void Dispose()
    {
        resourceSO.Clear();
        resourcesAsset.Clear();
    }

    public async Task LoadDataAsync<T, TKey>(string label) where T : IBaseResource<TKey>
    {
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);

        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
            return;

        Dictionary<TKey, T> dictionary = new();

        foreach (T asset in handle.Result)
        {
            dictionary[asset.Key] = asset;
        }

        resourceSO[typeof(T)] = dictionary;

        handleSO[typeof(T)] = handle;
    }

    public T GetResource<T, TKey>(TKey key) where T : IBaseResource<TKey>
    {
        if (!resourceSO.TryGetValue(typeof(T), out object obj))
        {
            return default;
        }

        Dictionary<TKey, T> dictionary = (Dictionary<TKey, T>)obj;

        return dictionary[key];
    }

    public List<T> GetAllResource<T, TKey>() where T : IBaseResource<TKey>
    {
        if (!resourceSO.TryGetValue(typeof(T), out object obj))
        {
            return new List<T>();
        }

        Dictionary<TKey, T> dictionary = (Dictionary<TKey, T>)obj;

        return new List<T>(dictionary.Values);
    }


    public async Task LoadAssetsAsync<T>(string label) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);

        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
            return;
        
        Dictionary<string, T> dict = new();

        foreach (T asset in handle.Result)
        {
            dict[asset.name] = asset;
        }

        resourcesAsset[label] = dict;

        handlesAsset[label] = handle;
    }

    public T GetAsset<T>(string label,string key) where T : UnityEngine.Object
    {
        if (!resourcesAsset.TryGetValue( label, out object obj))
        {
            Debug.LogError( $"Resource Type Not Found : {typeof(T)}");

            return null;
        }

        Dictionary<string, T> dict =  obj as Dictionary<string, T>;


        if (dict == null)
            return null;

        if (!dict.TryGetValue( key,  out T asset))
        {
            Debug.LogError( $"Asset Not Found : {key}");

            return null;
        }

        return asset;
    }


    public void ReleaseAsset(string label)
    {
        if (handlesAsset.TryGetValue(label, out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);

            handlesAsset.Remove(label);
        }

        if (resourcesAsset.ContainsKey(label))
        {
            resourcesAsset.Remove(label);
        }
    }



    public void ReleaseData<T>()
    {
        if (handleSO.TryGetValue(typeof(T), out AsyncOperationHandle handle))
        {
            Addressables.Release(handle);

            handleSO.Remove(typeof(T));
        }

        if (resourceSO.ContainsKey(typeof(T)))
        {
            resourceSO.Remove(typeof(T));
        }
    }

}