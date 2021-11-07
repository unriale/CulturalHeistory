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

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _fow = GetComponent<FieldOfView>();

        _stateMachine = new StateMachine();

        // States instantiation
        var randomGuarding = new RandomGuarding(this, _navMeshAgent, points);
        var thiefFound = new ThiefFound(this);

        // Transitions add (At) or any-transition
        _stateMachine.AddAnyTransition(thiefFound, () => (_fow.PlayerInRange || _isCollidedWithPlayer));

        // redefinition of StateMachine.AddTransition method (only for better code reading)
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        // TODO: Definition of condition functions for transitions

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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("FoN"))
        {
            _isAlerted = false;
        }
    }
}
