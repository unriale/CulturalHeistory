using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IncreasingAlert : IState
{
    
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent; // Atm I'm not using it, but it might be useful for some kind of movement in this state
    private GuardProgressBar _progressbar;

    public IncreasingAlert(Guard guard, NavMeshAgent navMeshAgent, GuardProgressBar progBar)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _progressbar = progBar;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // Look at noise Point
        _guard.LookAtNoisePoint();

        // Invoke UI
        _progressbar.IncreaseProgress(_guard.NoiseValue);
    }

}
