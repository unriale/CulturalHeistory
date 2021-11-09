using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DecreasingAlert : IState
{
    #region UI Events
    public static event Action<float> DecreaseAlertValue;
    #endregion

    private readonly Guard _guard;
    private NavMeshAgent _navMeshAgent; // Atm I'm not using it, but it might be useful for some kind of movement in this state
    private float _decreaseAmount;
    private GuardProgressBar _progressbar;

    public DecreasingAlert(Guard guard, NavMeshAgent navMeshAgent, float decreaseAmount, GuardProgressBar progBar)
    {
        _guard = guard;
        _navMeshAgent = navMeshAgent;
        _decreaseAmount = decreaseAmount;
        _progressbar = progBar;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        // Reset Exclamation Mark, so that can be displayed again
        _guard.RestExclamationMark();

        // Invoke UI
        _progressbar.DecreaseProgress(_decreaseAmount);

    }
}
