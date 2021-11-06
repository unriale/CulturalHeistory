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
        if (Input.GetKey(KeyCode.LeftShift))
            speed = runningSpeed;
        else
            speed = walkSpeed;

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        controller.SimpleMove(movementDirection * speed);

        Vector3 normalizedMovementDirection = movementDirection.normalized;
        if (normalizedMovementDirection.magnitude > minMagnitude) 
        {
            transform.forward = normalizedMovementDirection;
        }
    }
}
