using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IManager
{

    public GameState CurrentState { get; private set; }


    public FruitSpawner Spawner;

    public void Initialize()
    {
    }

    public void Dispose()
    {
    }

    public void InitializeRuntime()
    {
        InitializePools();

        CurrentState = GameState.Playing;
    }


    private void InitializePools()
    {
        List<FruitData> fruitDatas = Game.Get<ResourceManager>().GetAllResource<FruitData, FruitType>();


        foreach (FruitData data in fruitDatas)
        {
            Game.Get<PoolManager>().CreatePool<Fruit>(data.Type.ToString(),
                () => { return Object.Instantiate(data.Prefab); }, 10);

        }

        Game.Get<UIManager>().CreatePopupUIPool<ResultPopupUI>("InGame");
        Game.Get<UIManager>().CreatePopupUIPool<OptionPopupUI>("InGame");
        Game.Get<EffectManager>().CreateEffectPools();

    }


    public void GameOver()
    {
        if (CurrentState == GameState.GameOver)
            return;

        CurrentState = GameState.GameOver;

        GameEventBus.Publish(new GameOverEvent(Game.Get<ScoreManager>().CurrentScore));

    }

    public void RestartGame()
    {
        ClearFruits();

        Game.Get<ComboManager>().ResetCombo();

        Game.Get<ScoreManager>().ResetScore();

        Game.Get<UIManager>().CloseAllPopup();

        Spawner.Initialize();

        CurrentState = GameState.Playing;

    }

    private void ClearFruits()
    {
        Fruit[] fruits =Object.FindObjectsByType<Fruit>( FindObjectsSortMode.None);

        foreach (Fruit fruit in fruits)
        {
            fruit.ReturnPool();
        }
    }



}