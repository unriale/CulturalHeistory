using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// REMEMBER: FoN object must be on a different layermask from the coin or the collisions are fucked up
// REMEMBER: FoN object must have a rigidbody component kinematic - this allow to prevent Collision Trigger Bubbling
[RequireComponent(typeof(SphereCollider))]
public class CoinFoN : MonoBehaviour
{
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float noiseDuration = 0.5f;

    private SphereCollider _sphereCollider;
    private bool _isTimerActive = false;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = 0.02f;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.ToString().Equals("12")) // Floor layer
        {
            _sphereCollider.radius = radius;
            if (!_isTimerActive)
            {
                _isTimerActive = true;
                StartCoroutine(CoinNoiseTimer(noiseDuration));
            }
        }
    }

    private IEnumerator CoinNoiseTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        _sphereCollider.radius = 0.0f;
    }
}
