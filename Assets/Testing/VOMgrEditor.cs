using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VOMgr))]
public class VOMgrEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VOMgr voMgr = (VOMgr)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Get Bearings"))
        {
            Debug.Log("ownship - " + voMgr.isGiveWay(voMgr.ownship, voMgr.target));
            Debug.Log("target - " + voMgr.isGiveWay(voMgr.target, voMgr.ownship));
        }
    }
}