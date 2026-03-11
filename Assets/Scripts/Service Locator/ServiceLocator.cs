using UnityEngine;
using System.Collections.Generic;
using System;

public class ServiceLocator
{
    private readonly Dictionary<string, IService> _services = new();

    public static ServiceLocator Current { get; private set; }

    public static void Initialize()
    {
        Current = new ServiceLocator();
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service that already in dictionary: {key}.");
            return;
        }

        _services.Add(key, service);
    }

    public void Unregister<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister service that is not in dictionary: {key}.");
            return;
        }

        _services.Remove(key);
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to get service that is not in dictionary: {key}.");
            throw new InvalidOperationException();
        }
        
        return (T)_services[key];
    }
}
