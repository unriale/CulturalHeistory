using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookingAround : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private GuardProgressBar _progressBar;

    public LookingAround(Guard guard, NavMeshAgent navMesh, GuardProgressBar prog)
    {
        _guard = guard;
        _navMeshAgent = navMesh;
        _progressBar = prog;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;

        // Show exclamation mark
        _guard.ShowExclamationMark();

        // Interupt any other acting from the other states
        _guard.StopAllCoroutines();
        _guard.ResetIsActing();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // Increase Progress
        _progressBar.IncreaseProgress(_guard.NoiseValue * 1.8f);

        // Action
        _guard.ActLookingAround(0.7f);
    }
}
