using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonMgr : MonoBehaviour
{

    public IslandsMgr islandMgr;
    public TreeMgr treeMgr;
    public TreeMaker treeMaker;

    [SerializeField] private TMP_Dropdown dropdownTexture;
    [SerializeField] private Slider sliderTree;

    [SerializeField] private TMP_Dropdown dropdownClusters;
    [SerializeField] private TMP_Dropdown dropdownSize;

    bool treeActive = false;
    bool islandActive = false;


    [ContextMenu("TestIsland")]
    public void TestIsland()
    {
        StartCoroutine(IslandGenerate());
    }

    [ContextMenu("TestRandomIsland")]
    public void TestRandomIsland()
    {
        StartCoroutine(RandomIslandGenerate());
    }

    [ContextMenu("TestDelete")]
    public void TestDelete()
    {
        DeleteIslands();
    }

    private void Start()
    {
        DeleteIslands();
        dropdownClusters.gameObject.SetActive(false);
        sliderTree.gameObject.SetActive(false);
        dropdownSize.gameObject.SetActive(false);
        dropdownTexture.gameObject.SetActive(false);
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
        treeMaker.CreateClusters();
        treeMgr.multipleIslands();
    }

    public void DeleteIslands()
    {
        islandMgr.DeleteAllIslands();
        treeMaker.DeleteAllTrees();
    }

    public void TreeDropdowns()
    {
        if(treeActive == true)
        {
            dropdownClusters.gameObject.SetActive(false);
            sliderTree.gameObject.SetActive(false);
            treeActive = false;
        }
        else if(treeActive == false || islandActive == true){
            dropdownClusters.gameObject.SetActive(true);
            sliderTree.gameObject.SetActive(true);
            dropdownSize.gameObject.SetActive(false);
            dropdownTexture.gameObject.SetActive(false);
            treeActive = true;
            islandActive = false;
        }
    }

    public void IslandDropdowns()
    {
        if(islandActive == true)
        {
            dropdownSize.gameObject.SetActive(false);
            dropdownTexture.gameObject.SetActive(false);
            islandActive = false;
        }
        else if(treeActive == true || islandActive == false){
            dropdownClusters.gameObject.SetActive(false);
            sliderTree.gameObject.SetActive(false);
            dropdownSize.gameObject.SetActive(true);
            dropdownTexture.gameObject.SetActive(true);
            islandActive = true;
            treeActive = false;
        }
    }

    public IEnumerator RandomIslandGenerate()
    {
        DeleteIslands();
        yield return null;  //Wait a frame before raycasting
        int randomSize;
        int randomCluster;
        int maxValue = 0;
        int minValue = 0;
        int randomTreeNum;
        int randomformation = Random.Range(0, 2);
        int randomTexture;

        if (randomformation == 0){
            islandMgr.formation = IslandFormation.Single;
            randomSize = Random.Range(0, 2);
            islandMgr.islandParameters[0].islandSizeMenu = randomSize;
            randomCluster = Random.Range(0, 10);
            treeMaker.islandTrees[0].clusterNum = randomCluster;
            sizeSwitch(randomSize, ref maxValue, ref minValue);
            randomTreeNum = Random.Range(minValue, maxValue);
            treeMaker.islandTrees[0].treeCount = randomTreeNum;
            randomTexture = Random.Range(0, 4);
            islandMgr.islandParameters[0].textureChoice = randomTexture;
            treeMaker.leafTexture = randomTexture;
        }

        else if(randomformation == 1 || randomformation == 2){
            islandMgr.formation = IslandFormation.Line;
            for(int i = 0; i <= 2; i++)
            {
                randomSize = Random.Range(0, 2);
                islandMgr.islandParameters[i].islandSizeMenu = randomSize;
                randomCluster = Random.Range(0, 10);
                treeMaker.islandTrees[i].clusterNum = randomCluster;
                sizeSwitch(randomSize, ref maxValue, ref minValue);
                randomTreeNum = Random.Range(minValue, maxValue);
                treeMaker.islandTrees[i].treeCount = randomTreeNum;
                randomTexture = Random.Range(0, 4);
                islandMgr.islandParameters[i].textureChoice = randomTexture;
                treeMaker.leafTexture = randomTexture;
            }
        }
        islandMgr.RedrawIslands();
        treeMaker.CreateClusters();
        treeMgr.multipleIslands();
    }

    public void sizeSwitch(int size, ref int minVal, ref int maxVal)
    {
        switch(size)
        {
            case 0:
                maxVal = 20;
                minVal = 1;
            break;
            case 1:
                maxVal = 75;
                minVal = 20;
            break;
            case 2:
                maxVal = 200;
                minVal = 50;
            break;
        }
    }
}
