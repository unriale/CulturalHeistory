using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingNoise : IState
{
    private Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private GuardProgressBar _progressBar;

    private bool _hadPath = false;

    public FollowingNoise(Guard guard, NavMeshAgent navMesh, GuardProgressBar prog)
    {
        _guard = guard;
        _navMeshAgent = navMesh;
        _progressBar = prog;
    }

    public void OnEnter()
    {
        _hadPath = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;

        // Show exclamation mark
        _guard.ShowExclamationMark();

        // Interupt any other acting from the other states
        _guard.StopAllCoroutines();

        _navMeshAgent.SetDestination(_guard.noisePoint);
        _guard.SetIsActing();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // Increase Progress
        _progressBar.IncreaseProgress(_guard.NoiseValue * 1.8f);

        if (_navMeshAgent.hasPath)
        {
            _hadPath = true;    
        }
        else
        {
            if (_hadPath)
            {
                float dist = _navMeshAgent.remainingDistance;
                if (dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
                {
                    // Arrived
                    _guard.ResetIsActing(); // back to guarding
                }
            }
        }
        
    }
}
