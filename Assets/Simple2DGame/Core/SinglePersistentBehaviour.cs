using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public abstract class SinglePersistentBehaviour<T> : MonoBehaviour
{

    public static T Instance 
    {
        get 
        {
            singleBehaviours.TryGetValue(typeof(T), out var value);
            return value;
        }
    }

    private static readonly IDictionary<Type, T> singleBehaviours = new Dictionary<Type, T>();

    protected virtual void Awake()
    {
        var instance = InstanceType();

        if (singleBehaviours.ContainsKey(instance.GetType()))
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            singleBehaviours.Add(instance.GetType(), instance);
        }
    }

    protected virtual void Start()
    {
        
    }

    protected abstract T InstanceType();
    }

