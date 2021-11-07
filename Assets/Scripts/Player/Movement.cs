using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runningSpeed = 13f;
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
    }

    private void Move()
    {
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
            transform.forward = normalizedMovementDirection;
        }
    }

    private void ChangeStateToStay()
    {
        IsWalking = IsRunning = false;
        IsStaying = !IsWalking && !IsRunning;
    }

    private void ChangeStateToWalk()
    {
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
