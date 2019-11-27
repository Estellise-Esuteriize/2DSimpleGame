using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Level;

    private GameState _state;
    private GameState State 
    {
        get { return _state; }
        set { _state = State; }
    }

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

}
