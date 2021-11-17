using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefFound : IState
{
    #region Events
    public static event Action GameOver;
    #endregion

    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private FieldOfView _fow;

    private bool _enterOnce = false;
    private bool _hadPath = false;

    public ThiefFound(Guard guard, NavMeshAgent agent, FieldOfView fow)
    {
        _guard = guard;
        _navMeshAgent = agent;
        _fow = fow;
    }

    public void OnEnter()
    {
        _guard.StopAllCoroutines();

        // Invoke GameOver event
        GameOver?.Invoke();
        
        if (_fow.PlayerInRange)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_fow.PlayerPosition);
        }
        else
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_guard.noisePoint);
        }
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        Debug.Log("[IState ThiefFound Tick]: Guard found thief, Game Over!");

        if (_navMeshAgent.hasPath)
        {
            _hadPath = true;
        }

        if (_hadPath)
        {
            float dist = _navMeshAgent.remainingDistance;
            if (dist != Mathf.Infinity && dist <= 1.6f && !_enterOnce)
            {
                _enterOnce = true;
                // Arrived
                _navMeshAgent.isStopped = true; // Stop in front of the player
            }
        }

    }
}
