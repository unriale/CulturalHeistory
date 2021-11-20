using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookingAround : IState
{
    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent;
    private GuardProgressBar _progressBar;
    private Animator _animator;
    private SFXGuardCollection _sfxs;

    public LookingAround(Guard guard, NavMeshAgent navMesh, GuardProgressBar prog, Animator anim, SFXGuardCollection sfxs)
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

        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;

        // Show exclamation mark
        _guard.ShowExclamationMark();

        // Interupt any other acting from the other states
        //_guard.StopAllCoroutines();
        _guard.StopCoroutine("LookAroundWithDelay");
        _guard.ResetIsActing();

        // Animations
        _animator.SetBool("isWalking", false);
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
