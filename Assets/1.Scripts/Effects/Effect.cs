using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    private EffectType effectType;

    public ParticleSystem particle;

    private void OnEnable()
    {
        particle.Play();
    }

    private void Update()
    {
        if (particle == null)
            return;

        if (particle.IsAlive())
            return;

        ReturnPool();
    }

    private void ReturnPool()
    {
        Game.Get<PoolManager>().Return(effectType.ToString(),this);
    }
}
