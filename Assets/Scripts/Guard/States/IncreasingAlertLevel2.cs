using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IncreasingAlertLevel2 : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private GuardProgressBar _progressbar;

    public IncreasingAlertLevel2(Guard guard, NavMeshAgent navMeshAgent, GuardProgressBar progBar)
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

        // Interupt any other acting from the level1 state
        _guard.StopAllCoroutines();
        _guard.ResetIsActing();

        // Move to the noise point
        _navMeshAgent.SetDestination(_guard.noisePoint);
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        Debug.Log("LEVEL 2");
        // Invoke UI
        _progressbar.IncreaseProgress(_guard.NoiseValue * 1.4f);

        float dist = _navMeshAgent.remainingDistance;
        if (dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
        {
            // Arrived
            // Act
            _guard.ActIncreaseLevel2(); // actually not needed
        }
    }
}
