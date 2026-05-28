using UnityEngine;

public class MergeManager : IManager
{
    public void Initialize()
    {
    }
    public void Dispose()
    {
    }

    public void Merge(Fruit firstFruit, Fruit secondFruit, Vector3 position)
    {

        if(firstFruit.Data.Type == FruitType.Watermelon && secondFruit.Data.Type == FruitType.Watermelon)
        {
            GameEventBus.Publish(new FruitMergedEvent(firstFruit.Data, position));

            firstFruit.ReturnPool();

            secondFruit.ReturnPool();

            return;
        }

        FruitType nextType = firstFruit.Data.NextFruit;

        FruitData nextFruitData = Game.Get<ResourceManager>().GetResource<FruitData, FruitType>(nextType);

        FruitFactory.CreateFruit(nextFruitData, position);

        GameEventBus.Publish(new FruitMergedEvent(nextFruitData, position));

        firstFruit.ReturnPool();

        secondFruit.ReturnPool();
    }

}