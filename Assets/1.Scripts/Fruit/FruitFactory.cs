using UnityEngine;

public static class FruitFactory
{
    public static Fruit CreateFruit( FruitData fruitData, Vector3 position)
    {
        Fruit fruit = Game.Get<PoolManager>().Get<Fruit>(fruitData.Type.ToString());

        fruit.Initialize(fruitData);

        fruit.transform.SetPositionAndRotation(position, Quaternion.identity);

        return fruit;
    }
}