using System;
using System.Collections.Generic;

public static class Game
{
    private static Dictionary<Type, object> managers = new();

    public static void Register<T>(T manager)
    {
        managers[typeof(T)] = manager;
    }

    public static T Get<T>()
    {
        return (T)managers[typeof(T)];
    }

    public static void Initialize()
    {
        Register(new ResourceManager());
        Register(new PoolManager());
        Register(new SoundManager());
        Register(new GameManager());
        Register(new EffectManager());
        Register(new UIManager());
        Register(new ComboManager());
        Register(new ScoreManager());
        Register(new MergeManager());

        foreach (object manager in managers.Values)
        {
            if (manager is IManager initManager)
            {
                initManager.Initialize();
            }
        }
    }

    public static void Dispose()
    {
        foreach (object manager in managers.Values)
        {
            if (manager is IManager disposeManager)
            {
                disposeManager.Dispose();
            }
        }

        managers.Clear();
    }
}