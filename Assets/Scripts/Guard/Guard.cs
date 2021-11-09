using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GuardProgressBar progressBar;
    [SerializeField] private ExclamationMark exclamationMark;

    [Header("Nav Settings")]
    [SerializeField] private Transform[] points;

    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private FieldOfView _fow; // for player detection

    // Transitions Attributes
    private bool _isCollidedWithPlayer = false;
    private bool _isAlerted = false;
    private bool _isAlertResetted = false;
    private bool _isAlertFilled = false;

    [HideInInspector]
    public float NoiseValue = 0.0f; // noise value from the player (amount to add to the progressbar)
    [HideInInspector]
    public Vector3 noisePoint; // noise point

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _fow = GetComponent<FieldOfView>();

        _stateMachine = new StateMachine();

        // States instantiation
        var randomGuarding = new RandomGuarding(this, _navMeshAgent, points);
        var thiefFound = new ThiefFound(this);
        var increasingAlert = new IncreasingAlert(this,_navMeshAgent, progressBar);
        var decreasingAlert = new DecreasingAlert(this, _navMeshAgent, 0.02f, progressBar); // TBD the amount of the decrease

        // Transitions add (At) or any-transition
        At(randomGuarding, increasingAlert, IsGuardAlerted());
        At(increasingAlert, decreasingAlert, IsGuardNotAlerted());
        At(decreasingAlert, increasingAlert, IsGuardAlerted());
        At(decreasingAlert, randomGuarding, ShouldGoBackToGuarding());
        _stateMachine.AddAnyTransition(thiefFound, () => (_fow.PlayerInRange || _isCollidedWithPlayer || _isAlertFilled));

        // Redefinition of StateMachine.AddTransition method (only for better code reading)
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        // Definition of condition functions for transitions
        Func<bool> IsGuardAlerted() => () => _isAlerted;
        Func<bool> IsGuardNotAlerted() => () => _isAlerted == false;
        Func<bool> ShouldGoBackToGuarding() => () => ShouldGuarding(); // if progressbar resetted go back to guarding
        

        // Set the initial state
        _stateMachine.SetState(randomGuarding);
    }

    void Start()
    {
       
    }

    void Update()
    {
        _stateMachine.Tick();
    }

    public void OnProgressBarReset()
    {
        _isAlertResetted = true;
    }

    public void OnProgressBarFilled()
    {
        _isAlertFilled = true;
    }

    public void ShowExclamationMark()
    {
        exclamationMark.ShowExclamationMark();
    }

    public void RestExclamationMark()
    {
        exclamationMark.ResetExclamationMark();
    }

    public void LookAtNoisePoint()
    {
        Vector3 lookAtPos = noisePoint - this.transform.position;
        Quaternion newRot = Quaternion.LookRotation(lookAtPos, this.transform.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRot, Time.deltaTime * 8);
    }

    private bool ShouldGuarding()
    {
        if (_isAlertResetted)
        {
            _isAlertResetted = false;
            return true;
        }
        return false;
    }


    #region Collider's Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _isCollidedWithPlayer = true;
        }
        if (other.gameObject.tag.Equals("FoN"))
        {
            _isAlerted = true;
            noisePoint = other.gameObject.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("FoN"))
        {
            FieldOfNoise fon = other.gameObject.GetComponent<FieldOfNoise>();
            float radius = fon.currentRadius;
            NoiseValue = radius / (10 * Vector3.Distance(other.gameObject.transform.position, transform.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("FoN"))
        {
            _isAlerted = false;
        }

    }
    #endregion

}
