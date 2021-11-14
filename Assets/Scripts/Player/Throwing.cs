using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Movement))]
public class Throwing : MonoBehaviour
{
    [SerializeField] private GameObject aim;
    [SerializeField] private LayerMask layerForThrowing;
    [SerializeField] private float rotationSpeed = 3.5f;
    [Tooltip("Reference to a hand which will throw a coin")] 
    [SerializeField] Transform hand;

    private float time = 0;
    private bool isAiming = false;
    private Movement mover;

    GameObject instantiatedAim = null;

    private void Awake()
    {
        mover = GetComponent<Movement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (time <= 1f)
                time += Time.deltaTime;
            else if (!isAiming)
                DrawAimInScene();
            FollowMouse();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(time >= 1f) 
                FindObjectOfType<Coin>().ThrowFrom(hand.position, instantiatedAim.transform.position, this.transform);
            time = 0;
            mover.EnableMovement();
            isAiming = false;
            Destroy(instantiatedAim);
            instantiatedAim = null; 
        }
    }

    private void FollowMouse()
    {
        if (!instantiatedAim) return;
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerForThrowing))
            {
                instantiatedAim.transform.position = new Vector3(hit.point.x, hit.point.y + 0.05f, hit.point.z);
                Vector3 initialVector = instantiatedAim.transform.position - transform.position;
                Vector3 rotated = Quaternion.AngleAxis(-90, Vector3.up) * initialVector;
                Quaternion toRotation = Quaternion.LookRotation(rotated + transform.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void DrawAimInScene()
    {
        // TODO: Play animation (arm is up, aiming)
        isAiming = true;
        mover.DisableMovement();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerForThrowing))
        {
            instantiatedAim = Instantiate(aim, hit.point, aim.transform.rotation);
        }
    }
}
