using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGameManager : SinglePersistentBehaviour
{
    public static SimpleGameManager Instance;

    private SimpleGameState _state;
    public SimpleGameState State 
    {
        get { return _state; }
        set { _state = value; }
    }

    protected override Type InstanceType()
    {
        return typeof(SimpleGameManager);
    }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }
}
