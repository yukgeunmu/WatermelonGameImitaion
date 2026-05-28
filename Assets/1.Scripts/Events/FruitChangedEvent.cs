using UnityEngine;

public struct FruitChangedEvent
{
    public FruitData FruitData;

    public FruitChangedEvent(FruitData fruitData)
    {
        this.FruitData = fruitData;
    }
    
}
