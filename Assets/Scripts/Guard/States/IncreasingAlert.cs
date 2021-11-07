using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IncreasingAlert : IState
{
    #region UI Events
    public static event Action<float> IncreaseAlertValue;
    #endregion

    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent; // Atm I'm not using it, but it might be useful for some kind of movement in this state

    public IncreasingAlert(Guard guard, NavMeshAgent navMeshAgent)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // Invoke UI Event
        IncreaseAlertValue(_guard.NoiseValue);
    }

}
