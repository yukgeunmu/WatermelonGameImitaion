using UnityEngine;

public struct FruitMergedEvent
{
    public FruitData FruitData;

    public Vector3 Position;

    public FruitMergedEvent( FruitData fruitData, Vector3 position)
    {
        FruitData = fruitData;
        Position = position;
    }
}