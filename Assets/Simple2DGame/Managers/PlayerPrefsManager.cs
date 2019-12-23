using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constant;

// ReSharper disable once CheckNamespace
public static class PlayerPrefsManager
{
    private static readonly IDictionary<string, object> CacheItems = new Dictionary<string, object>();

    public static void DeletePrefs(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SetPrefs(string key, object value)
    {
        if (ItemExistsInCache(key))
        {
            SetValueKeyCache(key, value);
        }

        PlayerPrefs.SetString(key, value.ToString());
    }

    public static T RetrievePrefs<T>(string key)
    {
        var value = RetrievePrefs(key);

        return ConvertValueToT<T>(value);
    }

    public static object RetrievePrefs(string key)
    {
        if (ItemExistsInCache(key))
        {
            return RetrieveKeyCache(key);
        }
        else
        {
            var value = PlayerPrefs.GetString(key, null);

            CreateKeyCache(key, value);

            return value;
        }
    }

    private static T ConvertValueToT<T>(object value)
    {
        object tempValue = null;

        try
        {
            if (typeof(T) == typeof(int))
            {
                tempValue = Int32.Parse((string)value);
            }
            else
            {
                return default;
            }
        }
        catch (Exception)
        {
            return default;
        }

        return (T)tempValue;
    }

    private static void SetLevelCache(string key, int level)
    {
        if (!ItemExistsInCache(key))
        {
            CreateKeyCache(key, level);
        }
        else
        {
            SetValueKeyCache(key, level);
        }
    }

    private static void CreateKeyCache(string key, object value)
    {
        CacheItems.Add(key, value);
    }

    private static void SetValueKeyCache(string key, object value)
    {
        CacheItems[key] = value;
    }

    private static object RetrieveKeyCache(string key)
    {
        return CacheItems[key];
    }

    private static bool ItemExistsInCache(string key)
    {
        if (CacheItems.ContainsKey(key))
        {
            return true;
        }
        return false;
    }


}
