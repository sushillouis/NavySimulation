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
    [SerializeField] private TMP_Dropdown dropdownFormation;
    [SerializeField] private TMP_Dropdown dropdownIslandChoice;


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
        IslandSize randomizedSize = IslandSize.Small;


        if (randomformation == 0){
            treeMaker.islandChoice = 0;
            dropdownIslandChoice.value = 0;
            islandMgr.formation = IslandFormation.Single;
            dropdownFormation.value = 0;
            randomSize = Random.Range(0, 2);
            switch (randomSize)
            {
                case 0:
                    randomizedSize = IslandSize.Small;
                break;
                case 1:
                    randomizedSize = IslandSize.Medium;
                break;
                case 2:
                    randomizedSize = IslandSize.Large;
                break;
            }
            islandMgr.islandParameters[0].size = randomizedSize;
            dropdownSize.value = randomSize;
            randomCluster = Random.Range(0, 10);
            treeMaker.islandTrees[0].clusterNum = randomCluster;
            dropdownClusters.value = randomCluster;
            sizeSwitch(randomSize, ref maxValue, ref minValue);
            randomTreeNum = Random.Range(minValue, maxValue);
            treeMaker.islandTrees[0].treeCount = randomTreeNum;
            sliderTree.value = randomTreeNum;
            randomTexture = Random.Range(0, 4);
            islandMgr.islandParameters[0].textureChoice = randomTexture;
            dropdownTexture.value = randomTexture;
            treeMaker.leafTexture = randomTexture;
        }


        else if(randomformation == 1 || randomformation == 2){
            if(randomformation == 1){
                islandMgr.formation = IslandFormation.Line;
                dropdownFormation.value = 1;}
            else{
                islandMgr.formation = IslandFormation.Triangle;
                dropdownFormation.value = 2;}
            
            for(int i = 0; i <= 2; i++)
            {
                randomSize = Random.Range(0, 2);
                switch (randomSize)
                {
                    case 0:
                        randomizedSize = IslandSize.Small;
                        dropdownSize.value = randomSize;
                    break;
                    case 1:
                        randomizedSize = IslandSize.Medium;
                        dropdownSize.value = randomSize;
                    break;
                    case 2:
                        randomizedSize = IslandSize.Large;
                        dropdownSize.value = randomSize;
                    break;
                }
                islandMgr.islandParameters[i].size = randomizedSize;
                randomCluster = Random.Range(0, 10);
                treeMaker.islandTrees[i].clusterNum = randomCluster;
                sizeSwitch(randomSize, ref maxValue, ref minValue);
                randomTreeNum = Random.Range(minValue, maxValue);
                treeMaker.islandTrees[i].treeCount = randomTreeNum;
                randomTexture = Random.Range(0, 4);
                islandMgr.islandParameters[i].textureChoice = randomTexture;
                treeMaker.leafTexture = randomTexture;
            }

            treeMaker.islandChoice = 3;
            dropdownIslandChoice.value = 3;
            dropdownClusters.value = treeMaker.islandTrees[2].clusterNum;
            sliderTree.value = treeMaker.islandTrees[2].treeCount;
            dropdownTexture.value = islandMgr.islandParameters[2].textureChoice;

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
