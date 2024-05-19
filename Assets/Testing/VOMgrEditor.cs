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
            float rhown = DistanceMgr.inst.GetPotential(voMgr.ownship, voMgr.target).relativeBearingDegrees;
            float rht = DistanceMgr.inst.GetPotential(voMgr.target, voMgr.ownship).relativeBearingDegrees;

            string output = "\nownship - " + voMgr.IsGiveWay(voMgr.ownship, voMgr.target) + " - " + rhown + "\n";
            output += "target - " + voMgr.IsGiveWay(voMgr.target, voMgr.ownship) + " - " + rht;

            Debug.Log(output);
        }

        if(GUILayout.Button("Draw VO"))
        {
            voMgr.test.InitializeVODrawing();
        }
            
    }
}