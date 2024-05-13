using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(IslandsMgr))]
public class IslandMgrEditor : Editor
{

    public override void OnInspectorGUI()
    {
        IslandsMgr islandMgr = (IslandsMgr)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            islandMgr.RedrawIslands();
        }

        if (GUILayout.Button("Delete"))
        {
            islandMgr.DeleteAllIslands();
        }
    }
}