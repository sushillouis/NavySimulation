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

    [SerializeField] private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
    public TreeMaker TreeCluster;
    public Material baseMat;
    int formationChoice;

    private void Start()
    {
        TreeCluster.islandChoice = 0;
        GetFormationChoice();
        GetSizeValue();
        GetClusterValue();
        GetTextureValue();
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
                findSize();
                findClusters();
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
        GetIslandChoice();
    }
    
    public void GetIslandChoice()
    {
        StartCoroutine(IslandChoice());
    }

    public IEnumerator IslandChoice()
    {
        int pickedEntryIndex = dropdown2.value;
        TreeCluster.islandChoice = pickedEntryIndex;
        yield return null;
        findSize();
        yield return null;
        findClusters();
        // yield return null;
        // GetTextureValue();
    }

    public void GetSizeValue()
    {
        int pickedEntryIndex = dropdownSize.value;
        FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].islandSizeMenu = pickedEntryIndex;
        switch(pickedEntryIndex)
        {
            case 0:
                baseMat.SetFloat("_HeightCutoff", 86);
            break;
            case 1:
                baseMat.SetFloat("_HeightCutoff", 100);
            break;
            case 2:
                baseMat.SetFloat("_HeightCutoff", 120);
            break;
        }
    }

    public void GetClusterValue()
    {
        int pickedEntryIndex = dropdownClusters.value;
        TreeCluster.islandTrees[TreeCluster.islandChoice].clusterNum = pickedEntryIndex;
    }

    public void GetTextureValue()
    {
        int pickedEntryIndex = dropdown.value;
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
        int size = FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].islandSizeMenu;
        dropdownSize.value = size;
    }

    public void findClusters()
    {
        int clusters = TreeCluster.islandTrees[TreeCluster.islandChoice].clusterNum;
        dropdownClusters.value = clusters;
    }
}
