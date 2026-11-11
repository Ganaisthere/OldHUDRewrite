using System;
using System.Collections.Generic;

namespace LegacyUIs
{
    //Copyright (c) 2024 benjaminpants
    //Licensed under the MIT License(MIT)
    public class AssetManagerIThink
    {
        public Dictionary<Type, Dictionary<string, object>> Storage = new Dictionary<Type, Dictionary<string, object>>();

        public void Add<T>(string name, T item)
        {
            if (!Storage.ContainsKey(typeof(T)))
            {
                Storage.Add(typeof(T), new Dictionary<string, object>());
            }
            Storage[typeof(T)].Add(name, item);
        }

        public T Get<T>(string name)
        {
            if (!Storage.ContainsKey(typeof(T)))
            {
                BasePlugin.Log($"No {name}.");
                return default;
            }
            if (!Storage[typeof(T)].ContainsKey(name))
            {
                BasePlugin.Log($"No {name}.");
                return default;
            }
            return (T)Storage[typeof(T)][name];
        }

        public void RemoveAll<T>()
        {
            if (Storage.ContainsKey(typeof(T)))
            {
                Storage.Remove(typeof(T));
            }
        }
    }
}
