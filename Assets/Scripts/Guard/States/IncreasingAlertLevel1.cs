using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IncreasingAlertLevel1 : IState
{
    
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent; // Atm I'm not using it, but it might be useful for some kind of movement in this state
    private GuardProgressBar _progressbar;

    public IncreasingAlertLevel1(Guard guard, NavMeshAgent navMeshAgent, GuardProgressBar progBar)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _progressbar = progBar;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;

        // Show exclamation mark
        _guard.ShowExclamationMark();

        // Interupt any other acting from the decrease level state
        _guard.StopAllCoroutines();
        _guard.ResetIsActing();

    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // Execute Actions at level 1 alert
        _guard.ActIncreaseLevel1();

        // Invoke UI
        _progressbar.IncreaseProgress(_guard.NoiseValue * 1.4f);
    }

}
