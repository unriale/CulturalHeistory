using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throwing : MonoBehaviour
{
    [SerializeField] GameObject aim;
    [SerializeField] LayerMask layerForThrowing;

    private float time = 0;
    private bool isAiming = false;

    GameObject instantiatedAim = null;

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
            time = 0;
            print("Not drawing");
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
                transform.LookAt(instantiatedAim.transform.position);
            }
        }
    }

    private void DrawAimInScene()
    {
        isAiming = true;
        print("Drawing");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerForThrowing))
        {
            instantiatedAim = Instantiate(aim, hit.point, aim.transform.rotation);
        }
    }
}
