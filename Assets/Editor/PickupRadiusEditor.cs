using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pickup))]
public class PickupRadiusEditor : Editor
{
    private void OnSceneGUI()
    {
        Pickup pickup = (Pickup)target;
        Handles.color = Color.green;
        Handles.DrawWireArc(pickup.transform.position, Vector3.up, Vector3.forward, 360, pickup.PickupRadius);
    }
}
