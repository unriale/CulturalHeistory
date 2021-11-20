using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private SFXCollection sfxs;

    [Header("UI References")]
    [SerializeField] private GuardProgressBar progressBar;
    [SerializeField] private ExclamationMark exclamationMark;

    [Header("Nav Settings")]
    [SerializeField] private bool isFollowingRandomPath;
    [SerializeField] private Transform[] points;

    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private FieldOfView _fow; // for player detection

    // Transitions Attributes
    private bool _isCollidedWithPlayer = false;
    private bool _isAlerted = false;
    private bool _isAlertResetted = false;
    private bool _isAlertFilled = false;
    private bool _isActing = false; // if gurd is doing an action ex. looking around or else
    private bool _isAlertedByCoin = false;

    // Other Attributes
    private const float TIME_TIMER = 2.0f;
    private float _alertTimer = TIME_TIMER;
    private int _random = 0; // choose if the guard has to look around or follow the noise
    private float _refreshTimeStuckCheck = 3.0f;

    [HideInInspector]
    public float NoiseValue = 0.0f; // noise value from the player (amount to add to the progressbar)
    [HideInInspector]
    public Vector3 noisePoint; // noise point
    [HideInInspector]
    public Vector3 coinPoint; // coin point

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Randomize the collision avoidance priority
        int collsionPrio = UnityEngine.Random.Range(0, 51);
        _navMeshAgent.avoidancePriority = collsionPrio;

        _fow = GetComponent<FieldOfView>();

        _stateMachine = new StateMachine();

        // States instantiation
        IState guarding;
        if (isFollowingRandomPath)
        {
            guarding = new RandomGuarding(this, _navMeshAgent, points, progressBar, 0.04f, animator);
        }
        else
        {
            guarding = new Guarding(this, _navMeshAgent, points, progressBar, 0.04f, animator);
        }
        var thiefFound = new ThiefFound(this, _navMeshAgent, _fow, animator,sfxs);
        var lookingAround = new LookingAround(this, _navMeshAgent, progressBar,animator,sfxs);
        var followingNoise = new FollowingNoise(this, _navMeshAgent, progressBar,animator, sfxs);
        var distraction = new Distraction(this, _navMeshAgent, progressBar); 

        // Transitions and Any-Transitions
        At(guarding, lookingAround, ShouldLookAround());
        At(lookingAround, guarding, IsNotInAction());
        At(guarding, followingNoise, ShouldFollowNoise());
        At(followingNoise, guarding, IsNotInAction());
        At(distraction, guarding, IsNotInAction());
        
        _stateMachine.AddAnyTransition(thiefFound, () => (_fow.PlayerInRange || _isCollidedWithPlayer || _isAlertFilled));
        _stateMachine.AddAnyTransition(distraction, () => (_isAlertedByCoin));

        // Redefinition of StateMachine.AddTransition method (only for better code reading)
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        // Definition of condition functions for transitions
        Func<bool> IsNotInAction() => () => !_isActing;
        Func<bool> ShouldLookAround() => () => (_isAlerted && _random == 0);
        Func<bool> ShouldFollowNoise() => () => (_isAlerted && _random == 1);

        // Set the initial state
        _stateMachine.SetState(guarding);
    }

    void Start()
    {
        StartCoroutine(CheckNavMeshStuck(_refreshTimeStuckCheck));
    }

    void Update()
    {
        _stateMachine.Tick();
    }

    private void OnEnable()
    {
        ThiefFound.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        ThiefFound.GameOver -= OnGameOver;
    }
    private void OnGameOver()
    {
        StopAllCoroutines();
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.isStopped = true;
            animator.SetBool("isWalking", false);
        }
    }

    #region Public region
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

    public void LookAtNoisePointOverTime()
    {
        Vector3 lookAtPos = noisePoint - this.transform.position;
        Quaternion newRot = Quaternion.LookRotation(lookAtPos, this.transform.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRot, Time.deltaTime * 8); // over time
    }

    public void LookAtNoisePointInstant()
    {
        Vector3 lookAtPos = noisePoint - this.transform.position;
        Quaternion newRot = Quaternion.LookRotation(lookAtPos, this.transform.up);
        this.transform.rotation = newRot; // instant
    }

    public void ActLookingAround(float delay) // Action looking around
    {
        if (!_isActing)
        {
            _isActing = true;
            StartCoroutine(LookAroundWithDelay(delay));
        }
    }

    public void ResetIsActing()
    {
        _isActing = false;
    }

    public void SetIsActing()
    {
        _isActing = true;
    }
    public bool GetIsActing()
    {
        return _isActing;
    }
    public void SetRandomValue(int rand)
    {
        _random = rand;
    }

    #endregion

    #region Coroutine
    private IEnumerator LookAroundWithDelay(float delay)
    {
        List<String> directions = new List<String>();
        directions.Add("right");
        directions.Add("left");
        directions.Add("forward");
        directions.Add("back");

        // Shuffle
        for (int i = 0; i < directions.Count; i++)
        {
            string temp = directions[i];
            int randomIndex = UnityEngine.Random.Range(i, directions.Count);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        // Look at noise point
        this.LookAtNoisePointInstant();
        yield return new WaitForSeconds(delay);

        // Execute the first 3 directions in the list
        for(int i=0; i<3; ++i)
        {
            Vector3 point = new Vector3();
            if (directions[i].Equals("right"))
            {
                point = this.transform.right + this.transform.position;
            }
            else if (directions[i].Equals("left"))
            {
                point = this.transform.right * -1 + this.transform.position;
            }
            else if (directions[i].Equals("back"))
            {
                point = this.transform.forward * -1 + this.transform.position;
            }
            else
            {
                point = this.transform.forward + this.transform.position;
            }
            _navMeshAgent.SetDestination(point);
            yield return new WaitForSeconds(delay);
        }

        _isActing = false;
    }

    private IEnumerator CheckNavMeshStuck(float refreshTime)
    {
        Vector3 lastPosition = this.transform.position;
        while (true)
        {
            lastPosition = this.transform.position;
            yield return new WaitForSeconds(refreshTime);
            //Debug.Log("Remaining distance by "+this.gameObject.name + ": " + _navMeshAgent.remainingDistance);
            if (_navMeshAgent.hasPath && _navMeshAgent.remainingDistance > 0.0f)
            {
                //Debug.Log("I'm in the if");
                Vector3 currentPosition = this.transform.position;
                //Debug.Log(this.gameObject.name+": "+Vector3.Distance(currentPosition, lastPosition).ToString());
                if (Vector3.Distance(currentPosition, lastPosition) < 1.0f)
                {
                    //Debug.Log("[Guard CheckNavMeshStuck]: Agent Stuck - Changing avoidance priority");
                    int randPrio = UnityEngine.Random.Range(0, 51);
                    _navMeshAgent.avoidancePriority = randPrio;
                }
            }
        }
    }

    #endregion

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
            _alertTimer = TIME_TIMER; // Set Timer
            noisePoint = other.gameObject.transform.position;
        }
        if (other.gameObject.tag.Equals("Coin"))
        {
            _isAlertedByCoin = true;
            coinPoint = other.gameObject.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("FoN"))
        {
            FieldOfNoise fon = other.gameObject.GetComponent<FieldOfNoise>();
            float radius = fon.currentRadius;
            NoiseValue = radius / (10 * Vector3.Distance(other.gameObject.transform.position, transform.position));

            // Decrease Timer
            _alertTimer -= Time.deltaTime;
            // Check if timer is expired
            if(_alertTimer <= 0.0f)
            {
                // Game Over
                progressBar.Value = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("FoN"))
        {
            _isAlerted = false;
        }
        if (other.gameObject.tag.Equals("Coin"))
        {
            _isAlertedByCoin = false;
        }
    }
    #endregion

}
