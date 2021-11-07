using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfNoise))]
public class FieldOfNoiseEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfNoise fon = (FieldOfNoise)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fon.transform.position, Vector3.up, Vector3.forward, 360, fon.currentRadius);
    }
}
