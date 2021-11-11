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
    [SerializeField] private GameObject FONVisualization;
    [SerializeField] private float soundPropagationVelocity = 10.0f;

    [HideInInspector]
    public float currentRadius;
    [HideInInspector]
    public float noiseValue; // value to add to the guard's progress bar 

    public float walkNoiseRadius;
    public float runNoiseRadius;

    private SphereCollider _sphereCollider;
    private bool _canShowFoN = false; // this should be read from PlayerPreferences when we will have a settings menu

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        currentRadius = GetRadius();
        _sphereCollider.radius = _sphereCollider.radius + soundPropagationVelocity * Time.deltaTime;
        if(_sphereCollider.radius >= currentRadius)
        {
            _sphereCollider.radius = currentRadius;
        }

        // FoN visualization
        if (_canShowFoN)
        {
            FONVisualization.GetComponent<MeshRenderer>().enabled = true;
            FONVisualization.transform.localScale = new Vector3(_sphereCollider.radius*2, FONVisualization.transform.localScale.y, _sphereCollider.radius*2);
        }
        else
        {
            FONVisualization.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private float GetRadius()
    {
        if (movement.IsWalking) return walkNoiseRadius;
        else if (movement.IsRunning) return runNoiseRadius;
        return 0;
    }
}
