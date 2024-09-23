using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_Dropdown dropdown2;

    [SerializeField] private TMP_Dropdown dropdownClusters;
    [SerializeField] private TMP_Dropdown dropdownSize;
    [SerializeField] private TMP_Dropdown dropdownTexture;

    [SerializeField] private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
    public TreeMaker TreeCluster;
    public Material baseMat;
    public Material baseMat2;
    public Material baseMat3;
    public SliderMgr slider;
    int formationChoice;

    private void Start()
    {
        TreeCluster.islandChoice = 0;
        GetFormationChoice();
        GetSizeValue();
        GetClusterValue();
        GetTextureValue();
        // slider.SetSizeValues();
    }

    public void GetFormationChoice()
    {
        int pickedEntryIndex = dropdown.value;
        formationChoice = pickedEntryIndex;
        IslandsMgr IslandMgr = FindObjectOfType<IslandsMgr>();
        switch(pickedEntryIndex)
        {
            case 0:
                IslandMgr.formation = IslandFormation.Single;
            break;
            case 1:
                IslandMgr.formation = IslandFormation.Line;
            break;
            case 2:
               IslandMgr.formation = IslandFormation.Triangle;
            break;
        }

        switch(formationChoice)
        {
            case 0:
                dropdown2.options.Clear();
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island1", null));
                TreeCluster.islandChoice = 0;
            break;
            case 1:
                dropdown2.options.Clear();
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island1", null));
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island2", null));
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island3", null));
   
            break;
            case 2:
                dropdown2.options.Clear();
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island1", null));
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island2", null));
                dropdown2.options.Add(new TMP_Dropdown.OptionData("Island3", null));

            break;
        }
        dropdown2.AddOptions(options);
        SetIslandChoice();
        GetIslandChoice();
    }
    
    public void GetIslandChoice()
    {
        StartCoroutine(IslandChoice());
    }

    public void SetIslandChoice()
    {
        dropdown2.value = TreeCluster.islandChoice;
    }

    public IEnumerator IslandChoice()
    {
        int pickedEntryIndex = dropdown2.value;
        TreeCluster.islandChoice = pickedEntryIndex;
        yield return null;
        findSize();
        yield return null;
        findClusters();
        yield return null;
        findTextures();
    }

    public void GetSizeValue()
    {
        int pickedEntryIndex = dropdownSize.value;
        IslandSize chosenSize =  IslandSize.Small;
        switch (pickedEntryIndex)
        {
            case 0:
                chosenSize = IslandSize.Small;
            break;
            case 1:
                chosenSize = IslandSize.Medium;
            break;
            case 2:
                chosenSize = IslandSize.Large;
            break;
        }
        FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].size = chosenSize;
        Material tempMat = baseMat;
        int islandIndex = TreeCluster.islandChoice;
        switch(islandIndex)
        {
            case 0:
                tempMat = baseMat;
            break;
            case 1:
                tempMat = baseMat2;
            break;
            case 2:
                tempMat = baseMat3;
            break;

        }
        switch(pickedEntryIndex)
        {
            case 0:
                tempMat.SetFloat("_HeightCutoff", 86);

            break;
            case 1:
                tempMat.SetFloat("_HeightCutoff", 100);

            break;
            case 2:
                tempMat.SetFloat("_HeightCutoff", 120);

            break;
        }
        slider.SetSizeValues();
    }

    public void GetClusterValue()
    {
        int pickedEntryIndex = dropdownClusters.value;
        TreeCluster.islandTrees[TreeCluster.islandChoice].clusterNum = pickedEntryIndex;
    }

    public void GetTextureValue()
    {
        int pickedEntryIndex = dropdownTexture.value;
        FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].textureChoice = pickedEntryIndex;
        // FindObjectOfType<TreeMaker>().leafTexture = pickedEntryIndex;
    }

    public void GetFormatValue()
    {
        int pickedEntryIndex = dropdown.value;
        switch(pickedEntryIndex)
        {
            case 0:
                FindObjectOfType<IslandsMgr>().formation = IslandFormation.Single;
            break;
            case 1:
                FindObjectOfType<IslandsMgr>().formation = IslandFormation.Line;
            break;
            case 2:
                FindObjectOfType<IslandsMgr>().formation = IslandFormation.Triangle;
            break;
        }
    }

    public void findSize()
    {
        IslandSize sizeChoice = FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].size;
        switch (sizeChoice)
        {
             case IslandSize.Small:
                dropdownSize.value = 0;
            break;
            case IslandSize.Medium:
                dropdownSize.value = 1;
            break;
            case IslandSize.Large:
                dropdownSize.value = 2;
            break;
        }
    }

    public void findClusters()
    {
        int clusters = TreeCluster.islandTrees[TreeCluster.islandChoice].clusterNum;
        dropdownClusters.value = clusters;
    }

    public void findTextures()
    {
        int textures = FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].textureChoice;
        dropdownTexture.value = textures;
    }
}
