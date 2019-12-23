using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class SimpleGameManager : SinglePersistentBehaviour<SimpleGameManager>
{
    private SimpleGameState _state;
    public SimpleGameState State 
    {
        get => _state;
        set => _state = value;
    }

    protected override SimpleGameManager InstanceType()
    {
        return this;
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
