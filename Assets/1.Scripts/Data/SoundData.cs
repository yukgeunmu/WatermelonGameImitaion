using UnityEngine;

[CreateAssetMenu(menuName = "Game/Sound Data")]
public class SoundData : ScriptableObject, IBaseResource<SoundType>
{
    public SoundType Type;
    public SoundType Key => Type;

    public AudioClip Clip;

    public float Volume { get; private set; } = 1f;
}
