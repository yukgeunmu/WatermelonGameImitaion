using TMPro;
using UnityEngine;

public class ComboManager : IManager
{

    private int currentCombo;

    private float comboTimer;

    private const float ComboDuration = 3f;

    public void Initialize()
    {
        GameEventBus.Subscribe<FruitMergedEvent>(OnFruitMerged);
    }

    public void Dispose()
    {
        GameEventBus.Unsubscribe<FruitMergedEvent>(OnFruitMerged);
    }

    public void Tick(float deltaTime)
    {
        if (currentCombo <= 0)
            return;

        comboTimer -= deltaTime;

        if (comboTimer <= 0f)
        {
            ResetCombo();
        }
    }


    private void OnFruitMerged(FruitMergedEvent evt)
    {
        AddCombo();
    }


    public void AddCombo()
    {
        currentCombo++;

        comboTimer = ComboDuration;

        GameEventBus.Publish( new ComboChangedEvent(currentCombo));
    }

    public void ResetCombo()
    {
        currentCombo = 0;

        comboTimer = 0f;

        GameEventBus.Publish(new ComboChangedEvent(0));
    }

    public float GetComboMultiplier()
    {
        return 1f + (currentCombo * 0.1f);
    }



}