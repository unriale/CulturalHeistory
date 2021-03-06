using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private SFXPlayerCollection _sfxs;

    [Header("Movement Settings")]
    [SerializeField] protected float walkSpeed = 6f;
    [SerializeField] private float runningSpeed = 13f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] Animator animator;
    [SerializeField] GameObject thief;

    protected bool _canMove = true;
    protected CharacterController controller;
    private float minMagnitude = 0.01f;
    private float speed = 0;

    [HideInInspector] public bool IsStaying, IsRunning, IsWalking;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        UpdatePosition();
        if (!_sfxs) return;
        if (IsWalking)
        {
            _sfxs.PlayWalkCycle();
        }
        else if (IsRunning)
        {
            _sfxs.PlayRunCycle();
        }
    }

    void UpdatePosition()
    {
        thief.transform.rotation = transform.rotation;
        Vector3 newPos = new Vector3(transform.position.x, thief.transform.position.y, transform.position.z);
        thief.transform.position = newPos;
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
        DisableMovement();
        IsWalking = false;
        IsRunning = false;
    }

    public void EnableMovement() => _canMove = true;
    public void DisableMovement() => _canMove = false;

    public void PlayStealAnimation(Vector3 pickup, GameObject pickupObj)
    {
        Vector3 initialVector = pickup - transform.position;
        Vector3 rotated = Quaternion.AngleAxis(-90, Vector3.up) * initialVector;
        Quaternion toRotation = Quaternion.LookRotation(rotated + transform.up);
        Quaternion newRotation = new Quaternion(transform.rotation.x, toRotation.y, transform.rotation.z, transform.rotation.w);
        transform.rotation = newRotation;

        //transform.LookAt(pickup);
        animator.SetTrigger("steal");
    }

    protected virtual void Move()
    {
        if (!_canMove) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Running()) ChangeStateToRun();
        else if (Walking()) ChangeStateToWalk();
        else { ChangeStateToStay(); }

        SetAnimation();

        StartMoving(horizontalInput, verticalInput);
    }

    protected void SetAnimation()
    {
        animator.SetBool("walk", IsWalking);
        animator.SetBool("run", IsRunning);
    }


    protected void StartMoving(float horizontalInput, float verticalInput)
    { 
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        controller.SimpleMove(movementDirection * speed);
        Vector3 normalizedMovementDirection = movementDirection.normalized;
        if (normalizedMovementDirection.magnitude > minMagnitude)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion wantedRotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, normalizedMovementDirection, Vector3.up), 0);
            transform.rotation = Quaternion.Lerp(currentRotation, wantedRotation, Time.deltaTime * rotationSpeed);
        }
    }

    protected void ChangeStateToStay()
    {
        IsWalking = IsRunning = false;
        IsStaying = !IsWalking && !IsRunning;
    }

    protected void ChangeStateToWalk()
    {
        speed = walkSpeed;
        IsWalking = true;
        IsStaying = IsRunning = false;
    }

    protected void ChangeStateToRun()
    {
        speed = runningSpeed;
        IsRunning = true;
        IsStaying = IsWalking = false;
    }

    protected bool Walking()
    {
        return controller.velocity != Vector3.zero;
    }

    private bool Running()
    {
        return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && controller.velocity != Vector3.zero;
    }

}
