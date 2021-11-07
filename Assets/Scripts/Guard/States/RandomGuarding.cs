using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomGuarding : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private Transform[] _navPoints;

    private int indexPoint; // index for random point choice

    public RandomGuarding(Guard guard, NavMeshAgent navMeshAgent, Transform[] navPoints)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _navPoints = navPoints;
        indexPoint = 0;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        indexPoint = Random.Range(0, _navPoints.Length);
        _navMeshAgent.SetDestination(_navPoints[indexPoint].position);
    }

    public void OnExit()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
    }
    public void Tick()
    {
        float dist = _navMeshAgent.remainingDistance;
        if (dist != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && dist == 0)
        {
            // Arrived
            indexPoint = Random.Range(0, _navPoints.Length); // select the next point
            _navMeshAgent.SetDestination(_navPoints[indexPoint].position); // go to the next point
        }
    }
}
