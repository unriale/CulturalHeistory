using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guarding : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private Transform[] _navPoints;
    private GuardProgressBar _progressBar;
    private float _decreaseAmount;

    private int indexPoint; // index for the circular choice of nav points (ex: 0 -> 1 -> 2 -> 0 ...)

    public Guarding(Guard guard, NavMeshAgent navMeshAgent, Transform[] navPoints, GuardProgressBar prog, float decreaseAmount)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _navPoints = navPoints;
        _progressBar = prog;
        _decreaseAmount = decreaseAmount;
        indexPoint = 0;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_navPoints[indexPoint].position);

        // Reset Exclamation Mark, so that can be displayed again
        _guard.RestExclamationMark();
    }

    public void OnExit()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
    }

    public void Tick()
    {
        // Decrease progress when guarding
        _progressBar.DecreaseProgress(_decreaseAmount);

        float dist = _navMeshAgent.remainingDistance;
        if(dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
        {
            // Arrived
            indexPoint = (indexPoint + 1) % _navPoints.Length; // select the next point
            _navMeshAgent.SetDestination(_navPoints[indexPoint].position); // go to the next point
        }
    }
}
