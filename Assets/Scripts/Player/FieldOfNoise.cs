using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfNoise : MonoBehaviour
{
    [HideInInspector]
    public float currentRadius;
    [HideInInspector]
    public float noiseValue; // value to add to the guard's progress bar 

    public float walkNoiseRadius;
    public float runNoiseRadius;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        currentRadius = GetRadius();
        ChangeNoiseValue();
    }

    private void ChangeNoiseValue()
    {
        noiseValue = currentRadius == 0 ? 0 : 1 / currentRadius;
        print($"Noise value is {noiseValue}");
    }

    private float GetRadius()
    {
        if (movement.IsWalking) return walkNoiseRadius;
        else if (movement.IsRunning) return runNoiseRadius;
        return 0;
    }
}
