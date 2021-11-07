using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private FieldOfView _fow; // for player detection

    private bool _isCollidedWithPlayer = false;
    private bool _isAlerted = false;
    private bool _isAlertResetted = false;
    private bool _isAlertFilled = false;
    [HideInInspector]
    public float NoiseValue = 0.0f; // noise value from the player (amount to add to the progressbar)

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _fow = GetComponent<FieldOfView>();

        _stateMachine = new StateMachine();

        // States instantiation
        var randomGuarding = new RandomGuarding(this, _navMeshAgent, points);
        var thiefFound = new ThiefFound(this);
        var increasingAlert = new IncreasingAlert(this,_navMeshAgent);
        var decreasingAlert = new DecreasingAlert(this, _navMeshAgent, 0.02f); // TBD the amount of the decrease

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

    private void OnEnable()
    {
        // Listen to Events
        GuardProgressBar.ProgressBarResetted += OnProgressBarReset;
        GuardProgressBar.ProgressBarFilled += OnProgressBarFilled;
    }

    private void OnDisable()
    {
        // Stop Listening to Events
        GuardProgressBar.ProgressBarResetted -= OnProgressBarReset;
        GuardProgressBar.ProgressBarFilled -= OnProgressBarFilled;
    }

    void Start()
    {
       
    }

    void Update()
    {
        _stateMachine.Tick();
    }

    private void OnProgressBarReset()
    {
        _isAlertResetted = true;
    }

    private void OnProgressBarFilled()
    {
        _isAlertFilled = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _isCollidedWithPlayer = true;
        }
        if (other.gameObject.tag.Equals("FoN"))
        {
            _isAlerted = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("FoN"))
        {
            FieldOfNoise fon = other.gameObject.GetComponent<FieldOfNoise>();
            NoiseValue = fon.noiseValue;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("FoN"))
        {
            _isAlerted = false;
        }

    }
}
