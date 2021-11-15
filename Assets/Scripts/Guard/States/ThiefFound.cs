using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefFound : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;

    public ThiefFound(Guard guard, NavMeshAgent agent)
    {
        _guard = guard;
        _navMeshAgent = agent;
    }

    public void OnEnter()
    {
        _guard.StopAllCoroutines();

        // For now, adjust based on the gameover action
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        // TODO: what the guard should do? - invoke game over event or else
        Debug.Log("[IState ThiefFound Tick]: Guard found thief, Game Over!");
    }
}
