using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomGuarding : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private Transform[] _navPoints;
    private GuardProgressBar _progressBar;
    private float _decreaseAmount;

    private int indexPoint; // index for random point choice

    public RandomGuarding(Guard guard, NavMeshAgent navMeshAgent, Transform[] navPoints, GuardProgressBar prog, float decreaseAmount)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _navPoints = navPoints;
        _progressBar = prog;
        indexPoint = 0;
        _decreaseAmount = decreaseAmount;
    }

    public void OnEnter()
    {
        // Set random value so that can guard knows if the next state will be "FollowingNoise" 
        // or "LookingAround"
        int rand = Random.Range(0, 2);
        _guard.SetRandomValue(rand);

        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        indexPoint = Random.Range(0, _navPoints.Length);
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
        if (dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
        {
            // Arrived
            indexPoint = Random.Range(0, _navPoints.Length); // select the next point
            _navMeshAgent.SetDestination(_navPoints[indexPoint].position); // go to the next point
        }
    }
}
