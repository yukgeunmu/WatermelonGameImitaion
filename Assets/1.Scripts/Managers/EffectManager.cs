using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectManager : IManager
{
    private Dictionary<EffectType, EffectData> effectDictionary = new();

    public void Initialize()
    {
        GameEventBus.Subscribe<FruitMergedEvent>(OnFruitMerged);
    }

    public void Dispose()
    {
        GameEventBus.Unsubscribe<FruitMergedEvent>(OnFruitMerged);
    }

    public void CreateEffectPools()
    {

        var effectList = Game.Get<ResourceManager>().GetAllResource<EffectData, EffectType>();

        foreach (EffectData data in effectList)
        {
            effectDictionary[data.Key] = data;

            Game.Get<PoolManager>().CreatePool<Effect>(data.Type.ToString(),
                () =>
                {
                    return Object.Instantiate(data.Prefab).GetComponent<Effect>();
                },
                data.PreloadCount);
        }

    }

    private void OnFruitMerged(FruitMergedEvent evt)
    {
        PlayEffect( EffectType.Merge,evt.Position);

        CameraShake.Instance.Shake();
    }

    public void PlayEffect( EffectType type,Vector3 position)
    {
        if (!effectDictionary.TryGetValue(type, out EffectData data))
            return;

        Effect effect =Game.Get<PoolManager>().Get<Effect>(data.Type.ToString());

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.identity;
    }
}