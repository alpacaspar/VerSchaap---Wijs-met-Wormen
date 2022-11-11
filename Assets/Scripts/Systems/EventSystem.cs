using UnityEngine;
using System.Collections.Generic;

/// <summary>
///     Event system that works without arguments
/// </summary>
public static class EventSystem
{
    /// <summary>
    ///     Collection of listeners
    /// </summary>
    private static Dictionary<EventType, System.Action> eventDictionary = new Dictionary<EventType, System.Action>();

    /// <summary>
    ///     Adds a listener to the collection
    /// </summary>
    /// <param name="type">
    ///     To what event does this action listen
    /// </param>
    /// <param name="function">
    ///     Which action should be invoked
    /// </param>
    public static void AddListener(EventType type, System.Action function)
    {
        if (!eventDictionary.ContainsKey(type))
        {
            eventDictionary.Add(type, null);
        }

        eventDictionary[type] += (function);
    }

    /// <summary>
    ///     Removes a listener to the collection
    /// </summary>
    /// <param name="type">
    ///     To what event did this action listen
    /// </param>
    /// <param name="function">
    ///     Which action should be removed from the collection
    /// </param>
    public static void RemoveListener(EventType type, System.Action function)
    {
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type] -= (function);
        }
    }

    /// <summary>
    ///     Invoke action for all listeners based on the event type
    /// </summary>
    /// <param name="type">
    ///     Which event type should be invoked
    /// </param>
    public static void InvokeEvent(EventType type)
    {
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type]?.Invoke();
        }
    }
}

/// <summary>
///     Event system that takes arguments
/// </summary>
/// <typeparam name="T">
///     Arguments are generic
/// </typeparam>
public static class EventSystem<T>
{
    /// <summary>
    ///     Collection of listeners
    /// </summary>
    private static Dictionary<EventType, System.Action<T>> eventDictionary = new Dictionary<EventType, System.Action<T>>();

    /// <summary>
    ///     Adds a listener to the collection
    /// </summary>
    /// <param name="type">
    ///     To what event does this action listen
    /// </param>
    /// <param name="function">
    ///     Which action should be invoked and which argument should be passed
    /// </param>
    public static void AddListener(EventType type, System.Action<T> function)
    {
        if (!eventDictionary.ContainsKey(type))
        {
            eventDictionary.Add(type, null);
        }

        eventDictionary[type] += (function);
    }

    /// <summary>
    ///     Removes a listener to the collection
    /// </summary>
    /// <param name="type">
    ///     To what event did this action listen
    /// </param>
    /// <param name="function">
    ///     Which action should be removed from the collection and which argument should be passed
    /// </param>
    public static void RemoveListener(EventType type, System.Action<T> function)
    {
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type] -= (function);
        }
    }

    /// <summary>
    ///     Invoke action for all listeners based on the event type
    /// </summary>
    /// <param name="type">
    ///     Which event type should be invoked
    /// </param>
    /// <param name="arg">
    ///     What argument is passed
    /// </param>
    public static void InvokeEvent(EventType type, T arg)
    {
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type]?.Invoke(arg);
        }
    }
}