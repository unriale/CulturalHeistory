using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runningSpeed = 13f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] Animator animator;
    [SerializeField] GameObject thief;

    private bool _canMove = true;
    private CharacterController controller;
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
    }

    void UpdatePosition()
    {
        thief.transform.rotation = transform.rotation;
        Vector3 newPos = new Vector3(transform.position.x, thief.transform.position.y, transform.position.z);
        thief.transform.position = newPos;
    }

    public void EnableMovement() => _canMove = true;
    public void DisableMovement() => _canMove = false;

    private void Move()
    {
        if (!_canMove) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Running()) ChangeStateToRun();
        else if (Walking()) ChangeStateToWalk();
        else { ChangeStateToStay(); }

        StartMoving(horizontalInput, verticalInput);
    }



    private void StartMoving(float horizontalInput, float verticalInput)
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

    private void ChangeStateToStay()
    {
        animator.SetBool("walk", false);
        IsWalking = IsRunning = false;
        IsStaying = !IsWalking && !IsRunning;
    }

    private void ChangeStateToWalk()
    {
        animator.SetBool("walk", true);
        speed = walkSpeed;
        IsWalking = true;
        IsStaying = IsRunning = false;
    }

    private void ChangeStateToRun()
    {
        speed = runningSpeed;
        IsRunning = true;
        IsStaying = IsWalking = false;
    }

    private bool Walking()
    {
        return controller.velocity != Vector3.zero;
    }

    private bool Running()
    {
        return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && controller.velocity != Vector3.zero;
    }
}
