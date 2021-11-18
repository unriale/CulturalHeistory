using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float speed = 20f; 
    private void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
