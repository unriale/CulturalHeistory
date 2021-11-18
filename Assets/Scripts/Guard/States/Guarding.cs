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
    private Animator _animator;
    private float _decreaseAmount;

    private int indexPoint; // index for the circular choice of nav points (ex: 0 -> 1 -> 2 -> 0 ...)

    public Guarding(Guard guard, NavMeshAgent navMeshAgent, Transform[] navPoints, GuardProgressBar prog, float decreaseAmount, Animator anim)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _navPoints = navPoints;
        _progressBar = prog;
        _decreaseAmount = decreaseAmount;
        _animator = anim;
        indexPoint = 0;
    }

    public void OnEnter()
    {
        // Set random value so that can guard knows if the next state will be "FollowingNoise" 
        // or "LookingAround"
        int rand = Random.Range(0, 2);
        _guard.SetRandomValue(rand);

        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_navPoints[indexPoint].position);

        // Reset Exclamation Mark, so that can be displayed again
        _guard.RestExclamationMark();

        // Animations
        _animator.SetBool("isWalking", true);
    }

    public void OnExit()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;

        // Animations
        _animator.SetBool("isWalking", false);
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
