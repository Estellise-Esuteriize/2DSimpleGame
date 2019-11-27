using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

using Watcher = System.Diagnostics.Stopwatch;
using System.Text.RegularExpressions;

public class PoolingManager : SinglePersistentBehaviour
{
    public static PoolingManager Instance;

    public List<PoolObjects> objects;

    private IDictionary<string, List<GameObject>> poolObjects = new Dictionary<string, List<GameObject>>();
    private IDictionary<string, List<GameObject>> activePoolObjects = new Dictionary<string, List<GameObject>>();

    private Watcher watch;

    protected override Type InstanceType()
    {
        return typeof(PoolingManager);
    }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;

        watch = new Watcher();
    }

    private void Start()
    {
        InitializeObjects();
    }


    public void Reset()
    {
        Resetting();
    }

    public GameObject RetrieveGameObject(string key)
    {
        GameObject temp = null;

        if (poolObjects.ContainsKey(key))
        {
            var pool = poolObjects[key];

            if (pool.Count > 0)
            {
                temp = pool[0];

                AddObjectToActiveDictionary(activePoolObjects, temp, key);
                RemoveObjectFromPoolDictionary(poolObjects, temp, key);
            }
        }

        return temp;
    }

    private async void InitializeObjects()
    {
        await InitializeObjectsAsync();
    }

    private async Task InitializeObjectsAsync()
    {
        foreach (var temp in objects)
        {
            var total = temp.amount;
            var prefab = temp.value;
            var key = temp.SafeKeyDefaultLimit(7);

            if (prefab == null)
            {
                continue;
            }

            var prefabs = await InstantiatedGameObjectsAsync(prefab, total);

            if (!poolObjects.ContainsKey(key))
            {
                poolObjects.Add(key, prefabs);
            }
            else
            {
                AppendObjectsList(poolObjects[key], prefabs);
            }
        }   
    }

    private async Task<List<GameObject>> InstantiatedGameObjectsAsync(GameObject prefab, int total)
    {
        var prefabs = new List<GameObject>();

        for (var i = 0; i < total; i++)
        {
            var temp = Instantiate(prefab, gameObject.transform) as GameObject;
            temp.SetActive(false);

            if (temp != null)
            {
                prefabs.Add(temp);
            }
        }

        await Task.Delay(1);

        return prefabs;
    }

    private async void Resetting()
    {
        await ResettingObjectsAsync();
    }

    private async Task ResettingObjectsAsync()
    {
        foreach (var active in activePoolObjects)
        {
            var key = active.Key;
            var objects = active.Value;

            var prefabs = await ResettedObjectsAsync(objects);

            AppendObjectsDictionary(poolObjects, prefabs, key);
            RemoveKeyObjects(activePoolObjects, key);
        }
    }

    private async Task<List<GameObject>> ResettedObjectsAsync(List<GameObject> prefabs)
    {
        foreach (var temp in prefabs)
        {
            temp.SetActive(false);
            temp.transform.SetParent(gameObject.transform);
        }

        await Task.Delay(1);
        return prefabs;
    }

    private void AppendObjectsDictionary(IDictionary<string, List<GameObject>> dictionary, List<GameObject> similar, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            AppendObjectsList(dictionary[key], similar);
        }
        else
        {
            dictionary.Add(key, similar);
        }
    }

    private void AppendObjectsList(List<GameObject> original, List<GameObject> similar)
    {
        foreach (var temp in similar)
        {
            if (!original.Contains(temp))
            {
                similar.Add(temp);
            }
        }
    }

    private void RemoveKeyObjects(IDictionary<string, List<GameObject>> TKeyValue, string key)
    {
        if (TKeyValue.ContainsKey(key))
        {
            TKeyValue.Remove(key);
        }
    }

    private void RemoveObjectFromPoolDictionary(IDictionary<string, List<GameObject>> dictionary, GameObject toRemove, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key].Remove(toRemove);
        }
    }

    private void AddObjectToActiveDictionary(IDictionary<string, List<GameObject>> dictionary, GameObject toAdd, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key].Add(toAdd);
        }
        else
        {
            var tempList = new List<GameObject>() { toAdd };

            dictionary.Add(key, tempList);
        }
    }
}

[Serializable]
public struct PoolObjects
{
    public int amount;
    public string key;
    public GameObject value;


    public string SafeKeyDefault()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            if (value != null)
            {
                return value.name;
            }
            else
            {
                return KeyGenerator();
            }
        }
        else
        {
            return key;
        }
    }

    public string SafeKeyDefaultLimit(int limit)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            if (value != null)
            {
                return value.name;
            }
            else
            {
                return KeyGeneratorLimit(limit);
            }
        }
        else
        {
            return key;
        }
    }

    public string KeyGenerator()
    {
        var key = Guid.NewGuid().ToString();
        key = RemoveSpecialCharacters(key);

        return key;
    }

    public string KeyGeneratorLimit(int limit)
    {
        var key = Guid.NewGuid().ToString();
        key = RemoveSpecialCharacters(key);
        key = KeyLimitter(key, limit);

        return key;
    }

    public string KeyLimitter(string key, int limit)
    {
        if (IsSafeToLimit(key, limit))
        {
            return key.Substring(limit);
        }

        return key;
    }

    public bool IsSafeToLimit(string key, int limit)
    {
        if (key.Length - 1 < limit)
        {
            return false;
        }

        return true;
    }

    public string RemoveSpecialCharacters(string key)
    {
        return Regex.Replace(key, @"[^0-9a-zA-Z]+", "");
    }
} 