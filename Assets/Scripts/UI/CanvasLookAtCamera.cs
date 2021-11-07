using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CanvasLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform camera;
    private Canvas _canvas;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void LateUpdate()
    {
        _canvas.transform.LookAt(camera.position);
    }
}
