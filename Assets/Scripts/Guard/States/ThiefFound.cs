using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefFound : IState
{
    private readonly Guard _guard;

    public ThiefFound(Guard guard)
    {
        _guard = guard;
    }

    public void OnEnter()
    {
        _guard.StopAllCoroutines();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // TODO: what the guard should do? - invoke game over event or else
        Debug.Log("[IState ThiefFound Tick]: Guard found thief, Game Over!");
    }
}
