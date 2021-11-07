using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REMEMBER: FoN object must be on a different layermask from the player or the collisions are fucked up
// REMEMBER: FoN object must have a rigidbody component kinematic - this allow to prevent Collision Trigger Bubbling
[RequireComponent(typeof(SphereCollider))]
public class FieldOfNoise : MonoBehaviour
{
    [SerializeField] private Movement movement;

    [HideInInspector]
    public float currentRadius;
    [HideInInspector]
    public float noiseValue; // value to add to the guard's progress bar 

    public float walkNoiseRadius;
    public float runNoiseRadius;

    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        currentRadius = GetRadius();
        ChangeNoiseValue();
        _sphereCollider.radius = currentRadius;
    }

    private void ChangeNoiseValue()
    {
        noiseValue = currentRadius == 0 ? 0 : 1 / currentRadius; // !! WRONG -> More Radius = Less Value, we want the opposite
        print($"Noise value is {noiseValue}");
    }

    private float GetRadius()
    {
        if (movement.IsWalking) return walkNoiseRadius;
        else if (movement.IsRunning) return runNoiseRadius;
        return 0;
    }
}
