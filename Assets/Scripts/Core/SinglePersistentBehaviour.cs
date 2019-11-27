using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SinglePersistentBehaviour : MonoBehaviour
{

    private static List<Type> singleBehaviours = new List<Type>();

    protected virtual void Awake()
    {
        var instance = InstanceType();

        if (singleBehaviours.Contains(instance))
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            singleBehaviours.Add(instance);
        }
        
    }
    

    protected abstract Type InstanceType();


}
