using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMgr : MonoBehaviour
{

    public IslandsMgr islandMgr;
    public TreeMgr treeMgr;
    public TreeMaker treeMaker;
    public Button GenerateButton;
    public Button DeleteButton;

    [ContextMenu("TestIsland")]
    public void TestIsland()
    {
        DeleteIslands();
        islandMgr.RedrawIslands();
        if(treeMaker.clusterNum > 0)
        {
            treeMaker.CreateClusters();
            treeMgr.clusterMover();
        }
    }

    [ContextMenu("TestDelete")]
    public void TestDelete()
    {
        DeleteIslands();
    }

    private void Start()
    {
        DeleteIslands();
    }

    public void GenerateIslands()
    {
        StartCoroutine(IslandGenerate());
    }

    public IEnumerator IslandGenerate()
    {
        DeleteIslands();
        yield return null;  //Wait a frame before raycasting
        islandMgr.RedrawIslands();
        if(treeMaker.clusterNum > 0)
        {
            treeMaker.CreateClusters();
            treeMgr.clusterMover();
        }
    }

    public void DeleteIslands()
    {
        islandMgr.DeleteAllIslands();
        treeMaker.DeleteAllTrees();
    }
}
