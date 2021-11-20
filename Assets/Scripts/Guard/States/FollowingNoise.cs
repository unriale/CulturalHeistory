using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingNoise : IState
{
    private Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private GuardProgressBar _progressBar;
    private Animator _animator;
    private SFXGuardCollection _sfxs;

    private bool _hadPath = false;

    public FollowingNoise(Guard guard, NavMeshAgent navMesh, GuardProgressBar prog, Animator anim, SFXGuardCollection sfxs)
    {
        _guard = guard;
        _navMeshAgent = navMesh;
        _progressBar = prog;
        _animator = anim;
        _sfxs = sfxs;
    }

    public void OnEnter()
    {
        // Play Suspect SFX
        _sfxs.PlaySFX(0);
        _sfxs.PlayHearNoiseVoice();

        _hadPath = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;

        // Show exclamation mark
        _guard.ShowExclamationMark();

        // Interupt any other acting from the other states
        //_guard.StopAllCoroutines();
        _guard.StopCoroutine("LookAroundWithDelay");

        _navMeshAgent.SetDestination(_guard.noisePoint);
        _guard.SetIsActing();

        // Animations
        _animator.SetBool("isWalking", true);
    }

    public void OnExit()
    {
        // Animations
        _animator.SetBool("isWalking", false);
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
