using System;
using System.Collections.Generic;

public static class GameEventBus
{
    private static Dictionary<Type, Delegate> eventDictionary = new();

    public static void Subscribe<T>(Action<T> callback)
    {
        Type type = typeof(T);

        if (eventDictionary.TryGetValue(type, out Delegate existing))
        {
            eventDictionary[type] = Delegate.Combine(existing, callback);
        }
        else
        {
            eventDictionary[type] = callback;
        }
    }

    public static void Unsubscribe<T>(Action<T> callback)
    {
        Type type = typeof(T);

        if (!eventDictionary.TryGetValue(type, out Delegate existing))
            return;

        Delegate current = Delegate.Remove(existing, callback);

        if (current == null)
        {
            eventDictionary.Remove(type);
        }
        else
        {
            eventDictionary[type] = current;
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type type = typeof(T);

        if (!eventDictionary.TryGetValue(type, out Delegate del))
            return;

        if (del is Action<T> callback)
        {
            callback.Invoke(eventData);
        }
    }
}