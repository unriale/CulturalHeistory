using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Distraction : IState
{
    private Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private GuardProgressBar _progressBar;

    private bool _hadPath = false;
    private bool _increaseOnce = false;

    public Distraction(Guard guard, NavMeshAgent navMesh, GuardProgressBar prog)
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
        //_guard.StopAllCoroutines();
        _guard.StopCoroutine("LookAroundWithDelay");

        _navMeshAgent.SetDestination(_guard.coinPoint);
        _guard.SetIsActing();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // Increase Progress
        if (!_increaseOnce)
        {
            _increaseOnce = true;
            _progressBar.IncreaseProgress(0.1f);
            _progressBar.SetPorgressBarValue(_progressBar.Value + 0.5f);
        }

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
