using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMgr : MonoBehaviour
{

    public IslandsMgr islandMgr;
    public void GenerateIslands()
    {
        islandMgr.RedrawButtonIslands();
    }

    public void DeleteIslands()
    {
        islandMgr.DeleteAllIslands();
    }
}
