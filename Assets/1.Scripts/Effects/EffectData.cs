using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Game/EffectData")]
public class EffectData : ScriptableObject, IBaseResource<EffectType>
{
    public EffectType Type;

    public EffectType Key => Type;

    public GameObject Prefab;

    public int PreloadCount = 10;
}