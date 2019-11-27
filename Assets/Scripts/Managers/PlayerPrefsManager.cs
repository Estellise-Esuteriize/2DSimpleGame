using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constant;

public static class PlayerPrefsManager
{
    private static IDictionary<string, object> cacheItems = new Dictionary<string, object>();

    internal static void DeletePrefs(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    internal static void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    internal static void SetLevel(int level)
    {
        if (ItemExistsInCache(PREFS_LEVEL))
        {
            SetKeyCache(PREFS_LEVEL, level);
        }

        PlayerPrefs.SetInt(PREFS_LEVEL, level);
    }

    internal static int GetLevel()
    {
        if (ItemExistsInCache(PREFS_LEVEL))
        {
            return (int)RetrieveKeyCache(PREFS_LEVEL);
        }
        else
        {
            var level = PlayerPrefs.GetInt(PREFS_LEVEL, 1);

            CreateKeyCache(PREFS_LEVEL, level);

            return level;
        }
    }

    private static void SetLevelCache(string key, int level)
    {
        if (!ItemExistsInCache(key))
        {
            CreateKeyCache(key, level);
        }
        else
        {
            SetKeyCache(key, level);
        }
    }

    private static void CreateKeyCache(string key, object value)
    {
        cacheItems.Add(key, value);
    }

    private static void SetKeyCache(string key, object value)
    {
        cacheItems[key] = value;
    }

    private static object RetrieveKeyCache(string key)
    {
        return cacheItems[key];
    }

    private static bool ItemExistsInCache(string key)
    {
        if (cacheItems.ContainsKey(key))
        {
            return true;
        }
        return false;
    }


}
