using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(IslandsMgr))]
public class IslandMgrEditor : Editor
{

    public override void OnInspectorGUI()
    {
        IslandsMgr islandMgr = (IslandsMgr)target;

        if (DrawDefaultInspector())
        {
            if (islandMgr.autoUpdate)
            {
                islandMgr.RedrawIslands();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            islandMgr.RedrawIslands();
        }
    }
}