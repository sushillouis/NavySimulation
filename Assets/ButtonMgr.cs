using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMgr : MonoBehaviour
{

    public IslandsMgr islandMgr;
    public TreeMgr treeMgr;
    public TreeMaker treeMaker;

    [ContextMenu("TestIsland")]
    public void TestIsland()
    {
        StartCoroutine(IslandGenerate());
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

    public void GenerateRandomIslands()
    {
        StartCoroutine(RandomIslandGenerate());
    }

    public IEnumerator IslandGenerate()
    {
        DeleteIslands();
        yield return null;  //Wait a frame before raycasting
        islandMgr.RedrawIslands();
        if(treeMaker.clusterNum > 0)
        {
            print("madeithere");
            treeMaker.CreateClusters();
            treeMgr.multipleIslands();
        }
    }

    public void DeleteIslands()
    {
        islandMgr.DeleteAllIslands();
        treeMaker.DeleteAllTrees();
    }

    public IEnumerator RandomIslandGenerate()
    {
        DeleteIslands();
        yield return null;  //Wait a frame before raycasting
        int randomSize = Random.Range(0, 2);
        islandMgr.islandSizeMenu = randomSize;
        int randomCluster = Random.Range(0, 10);
        treeMaker.clusterNum = randomCluster;
        int maxValue = 0;
        int minValue = 0;
        switch(randomSize)
        {
            case 0:
                maxValue = 20;
                minValue = 1;
            break;
            case 1:
                maxValue = 75;
                minValue = 20;
            break;
            case 2:
                maxValue = 200;
                minValue = 50;
            break;
        }
        int randomTreeNum = Random.Range(minValue, maxValue);
        treeMaker.treeCount = randomTreeNum;
        int randomTexture = Random.Range(0, 4);
        islandMgr.textureChoice = randomTexture;
        treeMaker.leafTexture = randomTexture;

        islandMgr.RedrawIslands();
        if(treeMaker.clusterNum > 0)
        {
            treeMaker.CreateClusters();
            treeMgr.multipleIslands();
        }
    }
}
