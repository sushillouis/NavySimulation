using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpecularController)), CanEditMultipleObjects]
public class SpecularEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpecularController spec = (SpecularController)target;
        spec.SetOrientation();
    }
}